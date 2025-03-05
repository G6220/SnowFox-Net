namespace SnowFox_Net.Common.Redis
{
    using StackExchange.Redis;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    /// <summary>
    /// Redis
    /// </summary>
    public abstract class RedisConfig
    {
        /// <summary>
        /// Defines the _lazyConnection
        /// 懒加载连接
        /// </summary>
        private Lazy<ConnectionMultiplexer> _lazyConnection;

        /// <summary>
        /// Defines the _connection
        /// 连接
        /// </summary>
        private IConnectionMultiplexer _connection;

        /// <summary>
        /// Defines the _database
        /// 数据库
        /// </summary>
        private IDatabase _database;

        /// <summary>
        /// Defines the _subscriber
        /// 订阅
        /// </summary>
        private ISubscriber _subscriber;

        /// <summary>
        /// Initializes a new instance of the <see cref="RedisCluster"/> class.
        /// </summary>
        /// <param name="redisConnectionString">The redisConnectionString<see cref="string"/></param>
        internal RedisConfig(string redisConnectionString)
        {
            _lazyConnection = new Lazy<ConnectionMultiplexer>(() =>
                ConnectionMultiplexer.Connect(redisConnectionString));
            _connection = _lazyConnection.Value;
            _database = _connection.GetDatabase();
            _subscriber = _connection.GetSubscriber();
        }

        /// <summary>
        /// The Get
        /// 获取字符串值
        /// </summary>
        /// <param name="key">The key<see cref="string"/></param>
        /// <returns>The <see cref="string?"/></returns>
        public string Get(string key)
        {
            return _database.StringGet(key);
        }

        /// <summary>
        /// The Set
        /// 设置字符串值
        /// </summary>
        /// <param name="key">The key<see cref="string"/></param>
        /// <param name="value">The value<see cref="string"/></param>
        /// <returns>The <see cref="bool"/></returns>
        public bool Set(string key, string value)
        {
            return _database.StringSet(key, value);
        }

        /// <summary>
        /// The SetEx
        /// 设置字符串值（带过期时间）
        /// </summary>
        /// <param name="key">The key<see cref="string"/></param>
        /// <param name="value">The value<see cref="string"/></param>
        /// <param name="expiry">The expiry<see cref="TimeSpan"/></param>
        /// <returns>The <see cref="bool"/></returns>
        public bool SetEx(string key, string value, TimeSpan expiry)
        {
            return _database.StringSet(key, value, expiry);
        }

        /// <summary>
        /// The Delete
        /// 删除键
        /// </summary>
        /// <param name="key">The key<see cref="string"/></param>
        /// <returns>The <see cref="bool"/></returns>
        public bool Delete(string key)
        {
            return _database.KeyDelete(key);
        }

        /// <summary>
        /// The Exists
        /// 检查键是否存在
        /// </summary>
        /// <param name="key">The key<see cref="string"/></param>
        /// <returns>The <see cref="bool"/></returns>
        public bool Exists(string key)
        {
            return _database.KeyExists(key);
        }

        /// <summary>
        /// The HGet
        /// 获取哈希表中的字段值
        /// </summary>
        /// <param name="hashKey">The hashKey<see cref="string"/></param>
        /// <param name="field">The field<see cref="string"/></param>
        /// <returns>The <see cref="string?"/></returns>
        public string HGet(string hashKey, string field)
        {
            return _database.HashGet(hashKey, field);
        }

        /// <summary>
        /// The HSet
        /// 设置哈希表中的字段值
        /// </summary>
        /// <param name="hashKey">The hashKey<see cref="string"/></param>
        /// <param name="field">The field<see cref="string"/></param>
        /// <param name="value">The value<see cref="string"/></param>
        /// <returns>The <see cref="bool"/></returns>
        public bool HSet(string hashKey, string field, string value)
        {
            return _database.HashSet(hashKey, field, value);
        }

        /// <summary>
        /// The HDelete
        /// 删除哈希表中的字段
        /// </summary>
        /// <param name="hashKey">The hashKey<see cref="string"/></param>
        /// <param name="field">The field<see cref="string"/></param>
        /// <returns>The <see cref="bool"/></returns>
        public bool HDelete(string hashKey, string field)
        {
            return _database.HashDelete(hashKey, field);
        }

        /// <summary>
        /// The HGetAll
        /// 获取哈希表中所有字段和值
        /// </summary>
        /// <param name="hashKey">The hashKey<see cref="string"/></param>
        /// <returns>The <see cref="Dictionary{string, string}"/></returns>
        public Dictionary<string, string> HGetAll(string hashKey)
        {
            var hashEntries = _database.HashGetAll(hashKey);
            var result = new Dictionary<string, string>();

            foreach (var entry in hashEntries)
            {
                if (entry.Name.HasValue && entry.Value.HasValue)
#pragma warning disable CS8604 // 引用类型参数可能为 null。
#pragma warning disable CS8601 // 引用类型赋值可能为 null。
                    result[entry.Name] = entry.Value;
#pragma warning restore CS8601 // 引用类型赋值可能为 null。
#pragma warning restore CS8604 // 引用类型参数可能为 null。
            }
            return result;
        }

        /// <summary>
        /// The LPush
        /// 从左侧推送元素到列表
        /// </summary>
        /// <param name="listKey">The listKey<see cref="string"/></param>
        /// <param name="value">The value<see cref="string"/></param>
        /// <returns>The <see cref="bool"/></returns>
        public bool LPush(string listKey, string value)
        {
            return _database.ListLeftPush(listKey, value) > 0;
        }

        /// <summary>
        /// The RPush
        /// 从右侧推送元素到列表
        /// </summary>
        /// <param name="listKey">The listKey<see cref="string"/></param>
        /// <param name="value">The value<see cref="string"/></param>
        /// <returns>The <see cref="bool"/></returns>
        public bool RPush(string listKey, string value)
        {
            return _database.ListRightPush(listKey, value) > 0;
        }

        /// <summary>
        /// The LPop
        /// 从左侧弹出元素
        /// </summary>
        /// <param name="listKey">The listKey<see cref="string"/></param>
        /// <returns>The <see cref="string?"/></returns>
        public string LPop(string listKey)
        {
            return _database.ListLeftPop(listKey);
        }

        /// <summary>
        /// The RPop
        /// 从右侧弹出元素
        /// </summary>
        /// <param name="listKey">The listKey<see cref="string"/></param>
        /// <returns>The <see cref="string?"/></returns>
        public string RPop(string listKey)
        {
            return _database.ListRightPop(listKey);
        }

        /// <summary>
        /// The LLen
        /// 获取列表的长度
        /// </summary>
        /// <param name="listKey">The listKey<see cref="string"/></param>
        /// <returns>The <see cref="long"/></returns>
        public long LLen(string listKey)
        {
            return _database.ListLength(listKey);
        }

        /// <summary>
        /// The LRange
        /// 获取列表中指定范围的元素
        /// </summary>
        /// <param name="listKey">The listKey<see cref="string"/></param>
        /// <param name="start">The start<see cref="long"/></param>
        /// <param name="stop">The stop<see cref="long"/></param>
        /// <returns>The <see cref="List{string}"/></returns>
        public List<string> LRange(string listKey, long start, long stop)
        {
            var values = _database.ListRange(listKey, start, stop);
            var result = new List<string>();
            foreach (var value in values)
            {
                if (value.HasValue)
#pragma warning disable CS8604 // 引用类型参数可能为 null。
                    result.Add(value);
#pragma warning restore CS8604 // 引用类型参数可能为 null。
            }
            return result;
        }

        /// <summary>
        /// The SAdd
        /// 向集合中添加元素
        /// </summary>
        /// <param name="setKey">The setKey<see cref="string"/></param>
        /// <param name="value">The value<see cref="string"/></param>
        /// <returns>The <see cref="bool"/></returns>
        public bool SAdd(string setKey, string value)
        {
            return _database.SetAdd(setKey, value);
        }

        /// <summary>
        /// The SRem
        /// 从集合中移除元素
        /// </summary>
        /// <param name="setKey">The setKey<see cref="string"/></param>
        /// <param name="value">The value<see cref="string"/></param>
        /// <returns>The <see cref="bool"/></returns>
        public bool SRem(string setKey, string value)
        {
            return _database.SetRemove(setKey, value);
        }

        /// <summary>
        /// The SIsMember
        /// 判断元素是否在集合中
        /// </summary>
        /// <param name="setKey">The setKey<see cref="string"/></param>
        /// <param name="value">The value<see cref="string"/></param>
        /// <returns>The <see cref="bool"/></returns>
        public bool SIsMember(string setKey, string value)
        {
            return _database.SetContains(setKey, value);
        }

        /// <summary>
        /// The SMembers
        /// 获取集合的所有元素
        /// </summary>
        /// <param name="setKey">The setKey<see cref="string"/></param>
        /// <returns>The <see cref="List{string}"/></returns>
        public List<string> SMembers(string setKey)
        {
            var values = _database.SetMembers(setKey);
            var result = new List<string>();
            foreach (var value in values)
            {
                if (value.HasValue)
#pragma warning disable CS8604 // 引用类型参数可能为 null。
                    result.Add(value);
#pragma warning restore CS8604 // 引用类型参数可能为 null。
            }
            return result;
        }



        /// <summary>
        /// The ZAdd
        /// 向有序集合添加元素
        /// </summary>
        /// <param name="sortedSetKey">The sortedSetKey<see cref="string"/></param>
        /// <param name="value">The value<see cref="string"/></param>
        /// <param name="score">The score<see cref="double"/></param>
        /// <returns>The <see cref="bool"/></returns>
        public bool ZAdd(string sortedSetKey, string value, double score)
        {
            return _database.SortedSetAdd(sortedSetKey, value, score);
        }


        /// <summary>
        /// The ZRem
        /// 从有序集合中移除元素
        /// </summary>
        /// <param name="sortedSetKey">The sortedSetKey<see cref="string"/></param>
        /// <param name="value">The value<see cref="string"/></param>
        /// <returns>The <see cref="bool"/></returns>
        public bool ZRem(string sortedSetKey, string value)
        {
            return _database.SortedSetRemove(sortedSetKey, value);
        }

        /// <summary>
        /// The ZRange
        /// 获取有序集合中的元素
        /// </summary>
        /// <param name="sortedSetKey">The sortedSetKey<see cref="string"/></param>
        /// <param name="start">The start<see cref="long"/></param>
        /// <param name="stop">The stop<see cref="long"/></param>
        /// <returns>The <see cref="List{string}"/></returns>
        public List<string> ZRange(string sortedSetKey, long start, long stop)
        {
            var values = _database.SortedSetRangeByRank(sortedSetKey, start, stop);
            var result = new List<string>();
            foreach (var value in values)
            {
                if (value.HasValue)
#pragma warning disable CS8604 // 引用类型参数可能为 null。
                    result.Add(value);
#pragma warning restore CS8604 // 引用类型参数可能为 null。
            }
            return result;
        }

        
        /// <summary>
        /// The ZScore
        /// 获取有序集合元素的分数
        /// </summary>
        /// <param name="sortedSetKey">The sortedSetKey<see cref="string"/></param>
        /// <param name="value">The value<see cref="string"/></param>
        /// <returns>The <see cref="double"/></returns>
        public double ZScore(string sortedSetKey, string value)
        {
            return _database.SortedSetScore(sortedSetKey, value) ?? 0;
        }

        /// <summary>
        /// The Publish
        /// 发布消息
        /// </summary>
        /// <param name="channel">The channel<see cref="string"/></param>
        /// <param name="message">The message<see cref="string"/></param>
        public void Publish(string channel, string message)
        {
            _subscriber.Publish(new RedisChannel(channel, RedisChannel.PatternMode.Literal), message);
        }

        /// <summary>
        /// The Subscribe
        /// 订阅频道
        /// </summary>
        /// <param name="channel">The channel<see cref="string"/></param>
        /// <param name="handler">The handler<see cref="Action{RedisChannel, RedisValue}"/></param>
        public void Subscribe(string channel, Action<RedisChannel, RedisValue> handler)
        {
            _subscriber.Subscribe(new RedisChannel(channel, RedisChannel.PatternMode.Literal), handler);
        }

        /// <summary>
        /// The Unsubscribe
        /// 取消订阅
        /// </summary>
        /// <param name="channel">The channel<see cref="string"/></param>
        public void Unsubscribe(string channel)
        {
            _subscriber.Unsubscribe(new RedisChannel(channel, RedisChannel.PatternMode.Literal));
        }

        /// <summary>
        /// The GetClusterNodes
        /// 获取集群状态
        /// </summary>
        /// <param name="nodel">The nodel<see cref="string"/></param>
        /// <returns>The <see cref="IEnumerable{string}"/></returns>
        public IEnumerable<string> GetClusterNodes(string nodel)
        {
            var server = _connection.GetServer(nodel); // 获取任何一个节点的服务器
            var clusterNodes = server.ClusterNodes();
            if (clusterNodes == null)
            {
                return Enumerable.Empty<string>();
            }
            return clusterNodes.Nodes.Select(node => node.NodeId);
        }

        /// <summary>
        /// The GetNodeInfo
        ///获取节点信息
        /// </summary>
        /// <param name="nodeAddress">The nodeAddress<see cref="string"/></param>
        /// <returns>The <see cref="string"/></returns>
        public string GetNodeInfo(string nodeAddress)
        {
            var server = _connection.GetServer(nodeAddress);
            var info = server.Info();
            var sb = new StringBuilder();
            foreach (var group in info)
            {
                sb.AppendLine(group.Key);
                foreach (var pair in group)
                {
                    sb.AppendLine($"{pair.Key}: {pair.Value}");
                }
            }
            return sb.ToString();
        }
    }

    public class RedisCache : RedisConfig
    {
        public RedisCache(string redisConnectionString) : base(redisConnectionString)
        {
        }
    }
    public class RedisCluster : RedisConfig
    {
        public RedisCluster(string redisConnectionString) : base(redisConnectionString)
        {
        }
    }
}
