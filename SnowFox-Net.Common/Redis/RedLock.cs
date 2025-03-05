namespace SnowFox_Net.Common.Redis
{
    using Microsoft.Extensions.Logging;
    using StackExchange.Redis;

    /// <summary>
    /// 锁实体
    /// </summary>
    public class Lock
    {
        /// <summary>
        /// Gets or sets the Key
        /// </summary>
        public string Key { get; set; }

        /// <summary>
        /// Gets or sets the Value
        /// </summary>
        public string Value { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="Lock"/> class.
        /// </summary>
        /// <param name="key">The key<see cref="string"/></param>
        /// <param name="value">The value<see cref="string"/></param>
        public Lock(string key, string value)
        {
            Key = key;
            Value = value;
        }
    }

    /// <summary>
    /// RedLock锁
    /// </summary>
    public class RedLock
    {
        /// <summary>
        /// Defines the _redisConnections
        /// </summary>
        private readonly List<ConnectionMultiplexer> _redisConnections;

        /// <summary>
        /// Defines the _retryInterval
        /// </summary>
        private readonly TimeSpan _retryInterval;

        /// <summary>
        /// Defines the _acquireTimeout
        /// </summary>
        private readonly TimeSpan _acquireTimeout;

        /// <summary>
        /// Defines the _maxRetries
        /// </summary>
        private readonly byte _maxRetries;

        /// <summary>
        /// Defines the _logger
        /// </summary>
        private readonly ILogger _logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="RedLock"/> class.
        /// </summary>
        /// <param name="redisServers">The redisServers<see cref="List{string}"/></param>
        /// <param name="retryInterval">The retryInterval<see cref="TimeSpan"/></param>
        /// <param name="acquireTimeout">The acquireTimeout<see cref="TimeSpan"/></param>
        /// <param name="maxRetries">The maxRetries<see cref="byte"/></param>
        /// <param name="logger">The logger<see cref="ILogger{RedLock}"/></param>
        public RedLock(List<string> redisServers, TimeSpan retryInterval, TimeSpan acquireTimeout, byte maxRetries, ILogger<RedLock> logger)
        {
            _redisConnections = new List<ConnectionMultiplexer>();
            foreach (var server in redisServers)
            {
                var connection = ConnectionMultiplexer.Connect(server);
                _redisConnections.Add(connection);
            }

            _maxRetries = maxRetries;
            _retryInterval = retryInterval;
            _acquireTimeout = acquireTimeout;
            _maxRetries = maxRetries;
            _logger = logger;
        }

        /// <summary>
        /// The Lock
        /// </summary>
        /// <param name="key">The key<see cref="string"/></param>
        /// <param name="lockTimeout">The lockTimeout<see cref="TimeSpan"/></param>
        /// <returns>The <see cref="Task{Lock}"/></returns>
        public async Task<Lock> Lock(string key, TimeSpan lockTimeout)
        {
            var value = Guid.NewGuid().ToString();
            var acquireResults = new List<bool>();

            var luaScript = @"
                if redis.call('SETNX', KEYS[1], ARGV[1]) == 1 then
                    redis.call('EXPIRE', KEYS[1], ARGV[2])
                    return 1
                else
                    return 0
                end";

            var startTime = DateTime.Now;
            int attempt = 0;

            while (attempt < _maxRetries)
            {
                // 检查是否超过了锁获取超时
                if (DateTime.Now - startTime > _acquireTimeout)
                {
                    _logger.LogWarning($"获取超时 {key}");
                    return null;
                }

                acquireResults.Clear();

                foreach (var redis in _redisConnections)
                {
                    var db = redis.GetDatabase();

                    // 执行 Lua 脚本，尝试加锁
                    var result = await db.ScriptEvaluateAsync(luaScript, new RedisKey[] { key }, new RedisValue[] { value, lockTimeout.TotalSeconds }, CommandFlags.None);
                    acquireResults.Add((long)result == 1);

                }

                // 如果大多数 Redis 实例加锁成功，认为加锁成功
                var successCount = acquireResults.Count(r => r);
                if (successCount >= _redisConnections.Count / 2 + 1)
                {
                    return new Lock(key, value);
                }

                // 增加重试间隔
                attempt++;
                if (attempt < _maxRetries)
                {
                    await Task.Delay(_retryInterval + TimeSpan.FromMilliseconds(attempt * 50));  // 等待重试
                }
            }

            return null;  // 超过最大重试次数，返回失败
        }

        /// <summary>
        /// The Unlock
        /// </summary>
        /// <param name="redLock">The redLock<see cref="Lock"/></param>
        /// <returns>The <see cref="Task"/></returns>
        public async Task Unlock(Lock redLock)
        {

            var luaScript = @"
            if redis.call('GET', KEYS[1]) == ARGV[1] then
                redis.call('DEL', KEYS[1])
                return 1
            else
                return 0
            end";

            foreach (var redis in _redisConnections)
            {
                var db = redis.GetDatabase();

                // 执行 Lua 脚本，尝试释放锁
                var result = await db.ScriptEvaluateAsync(luaScript, new RedisKey[] { redLock.Key }, new RedisValue[] { redLock.Value });
                if ((long)result == 1)
                {
                    _logger.LogInformation($"释放成功 {redLock.Key}");
                    return;
                }
            }

            _logger.LogWarning($"释放失败 {redLock.Key}");
        }
    }
}
