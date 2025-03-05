using FreeSql;
using Serilog;
using SnowFox_Net.Admin.Interfaces;
using SnowFox_Net.Common.Encrypt;
using SnowFox_Net.Common.Middleware;
using SnowFox_Net.Common.Redis;
using SnowFox_Net.Shared.DTOs;
using SnowFox_Net.Shared.Enums;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

//配置日志
Log.Logger = new LoggerConfiguration()
    .Enrich.FromLogContext()
    .MinimumLevel.Information()
    .WriteTo.Console()
    /*  .WriteTo.Http(requestUri: "http://logserver.local/logs", 
      queueLimitBytes: 10_000_000,                
      logEventLimitBytes: 100_000,                
      logEventsInBatchLimit: 500,                 
      batchSizeLimitBytes: 2_000_000,             
      period: TimeSpan.FromSeconds(2),           
      flushOnClose: true,                        
      textFormatter: new CompactJsonFormatter(),       
      batchFormatter: new ArrayBatchFormatter(), 
      restrictedToMinimumLevel: LogEventLevel.Information 
  )*/
    .CreateLogger();
builder.Logging.ClearProviders();
builder.Logging.AddSerilog();

//读取配置文件
builder.Configuration.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
    .AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json", optional: false, reloadOnChange: true);
builder.Configuration.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
    .AddJsonFile($"DBConfig.{builder.Environment.EnvironmentName}.json", optional: false, reloadOnChange: true);
builder.Configuration.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
    .AddJsonFile($"RedisCache.{builder.Environment.EnvironmentName}.json", optional: false, reloadOnChange: true);
builder.Configuration.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
    .AddJsonFile($"RedisCluster.{builder.Environment.EnvironmentName}.json", optional: false, reloadOnChange: true);

//依赖注入
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// 读取数据库配置
var dbConnectionStrings = builder.Configuration.GetSection("DBConfig:DBConnectionStrings").Get<Dictionary<string, string>>();
// 注册 IdleBus
builder.Services.AddSingleton(provider =>
{
    var idleBus = new IdleBus<DBEnum, IFreeSql>();

    // 注册默认数据库连接
    if (builder.Configuration["DBConfig:ConnectionString"] is string defaultDbConfig)
    {
        idleBus.Register(DBEnum.Default, () => new FreeSqlBuilder()
            .UseConnectionString(DataType.MySql, defaultDbConfig)
            .Build());
    }
    if(dbConnectionStrings!=null&&dbConnectionStrings.Count!=0)
    {
        // 注册其他数据库连接
        foreach (DBEnum dbEnum in Enum.GetValues(typeof(DBEnum)))
        {
            if (dbConnectionStrings.TryGetValue(dbEnum.ToString(), out var connectionString))
            {
                idleBus.Register(dbEnum, () => new FreeSqlBuilder()
                    .UseConnectionString(DataType.MySql, connectionString)
                    .Build());
            }
        }
    }

    return idleBus;
});

//注册Argon2
builder.Services.AddSingleton<Argon2Tool>();

// 注册 RedisCache
var redisCacheConfig = builder.Configuration.GetSection("RedisCache").Get<List<string>>();
builder.Services.AddSingleton(provider =>
{
    var str = string.Empty;
    foreach (var item in redisCacheConfig)
    {
        str += item;
        if (item != redisCacheConfig.Last())
        {
            str += ",";
        }
    }
    str=str+ ",abortConnect=false";
    return new RedisCache(str);
});

//注册RedisCluster
var redisClusterConfig = builder.Configuration.GetSection("RedisCluster").Get<List<List<string>>>();
builder.Services.AddSingleton(provider =>
{
    var str = string.Empty;
    foreach (var item in redisClusterConfig)
    {

        foreach (var server in item)
        {
            str += server;
            if (server != item.Last())
            {
                str += ",";
            }
        }
    }
    str = str + ",abortConnect=false";
    return new RedisCluster(str);
});

builder.Services.AddSingleton<ILogger<RedLock>>(provider =>
{
    return LoggerFactory.Create(builder => builder.AddConsole()).CreateLogger<RedLock>();
});

//注册 RedLock
builder.Services.AddSingleton(provider =>
{
    var redisConnections = new List<string>();
    foreach (var item in redisCacheConfig)
    {
        redisConnections.Add(item);
    }
    return new RedLock(redisConnections, TimeSpan.FromMilliseconds(50), TimeSpan.FromSeconds(2), 3, provider.GetRequiredService<ILogger<RedLock>>());
});

// 注册 IdSegment
builder.Services.AddScoped(provider =>
{
    return new IdSegment(provider.GetRequiredService<ILogger<IdSegment>>(), provider.GetRequiredService<RedLock>(), provider.GetRequiredService<IdleBus<DBEnum,IFreeSql>>(), provider.GetRequiredService<RedisCache>());
});
builder.Services.AddScoped<UserContext>();

//注册JwtTool
builder.Services.AddSingleton<JwtTool>(provider =>
{
    var secretKey = builder.Configuration["SecretKey"];
    return new JwtTool(secretKey, "SnowFox", "SnowFox");
});



builder.Services.Scan(scan => scan
    .FromAssemblyOf<Program>() // 从 IUserService 所在的程序集扫描
    .AddClasses() // 添加所有类
    .AsImplementedInterfaces() // 按接口注册
    .WithScopedLifetime());

var app = builder.Build();
//配置Swagger
if (app.Environment.IsDevelopment())
{
    app.UseSwagger(options =>
    {
        options.RouteTemplate = "{documentName}/swagger.json";
    });
    app.UseSwaggerUI(options =>
    {
        options.RoutePrefix = string.Empty;
    });
}

app.UseHttpsRedirection();
app.UseMiddleware<UserContextMiddleware>();
app.UseAuthorization();

app.MapControllers();

app.Run();
