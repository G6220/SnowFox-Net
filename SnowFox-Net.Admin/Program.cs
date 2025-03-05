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

//������־
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

//��ȡ�����ļ�
builder.Configuration.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
    .AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json", optional: false, reloadOnChange: true);
builder.Configuration.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
    .AddJsonFile($"DBConfig.{builder.Environment.EnvironmentName}.json", optional: false, reloadOnChange: true);
builder.Configuration.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
    .AddJsonFile($"RedisCache.{builder.Environment.EnvironmentName}.json", optional: false, reloadOnChange: true);
builder.Configuration.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
    .AddJsonFile($"RedisCluster.{builder.Environment.EnvironmentName}.json", optional: false, reloadOnChange: true);

//����ע��
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// ��ȡ���ݿ�����
var dbConnectionStrings = builder.Configuration.GetSection("DBConfig:DBConnectionStrings").Get<Dictionary<string, string>>();
// ע�� IdleBus
builder.Services.AddSingleton(provider =>
{
    var idleBus = new IdleBus<DBEnum, IFreeSql>();

    // ע��Ĭ�����ݿ�����
    if (builder.Configuration["DBConfig:ConnectionString"] is string defaultDbConfig)
    {
        idleBus.Register(DBEnum.Default, () => new FreeSqlBuilder()
            .UseConnectionString(DataType.MySql, defaultDbConfig)
            .Build());
    }
    if(dbConnectionStrings!=null&&dbConnectionStrings.Count!=0)
    {
        // ע���������ݿ�����
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

//ע��Argon2
builder.Services.AddSingleton<Argon2Tool>();

// ע�� RedisCache
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

//ע��RedisCluster
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

//ע�� RedLock
builder.Services.AddSingleton(provider =>
{
    var redisConnections = new List<string>();
    foreach (var item in redisCacheConfig)
    {
        redisConnections.Add(item);
    }
    return new RedLock(redisConnections, TimeSpan.FromMilliseconds(50), TimeSpan.FromSeconds(2), 3, provider.GetRequiredService<ILogger<RedLock>>());
});

// ע�� IdSegment
builder.Services.AddScoped(provider =>
{
    return new IdSegment(provider.GetRequiredService<ILogger<IdSegment>>(), provider.GetRequiredService<RedLock>(), provider.GetRequiredService<IdleBus<DBEnum,IFreeSql>>(), provider.GetRequiredService<RedisCache>());
});
builder.Services.AddScoped<UserContext>();

//ע��JwtTool
builder.Services.AddSingleton<JwtTool>(provider =>
{
    var secretKey = builder.Configuration["SecretKey"];
    return new JwtTool(secretKey, "SnowFox", "SnowFox");
});



builder.Services.Scan(scan => scan
    .FromAssemblyOf<Program>() // �� IUserService ���ڵĳ���ɨ��
    .AddClasses() // ���������
    .AsImplementedInterfaces() // ���ӿ�ע��
    .WithScopedLifetime());

var app = builder.Build();
//����Swagger
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
