using JasperFx;
using JasperFx.CodeGeneration;
using JasperFx.Events;
using JasperFx.Events.Daemon;
using Marten;
using Serilog;
using TicketStore.API.Middlewares;
using TicketStore.Domain.DependencyInjection;
using TicketStore.Domain.SocialEventFeature.Commands;
using TicketStore.Domain.SocialEventFeature.Schema.Indexes;
using TicketStore.Domain.SocialEventFeature.Schema.Projections;
using Wolverine;
using Wolverine.FluentValidation;
using Wolverine.Marten;

var builder = WebApplication.CreateBuilder(args);

var configuration = new ConfigurationBuilder()
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile("appsettings.json")
    .AddJsonFile($"appsettings.{Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Production"}.json", true)
    .Build();

Log.Logger = new LoggerConfiguration()
    .ReadFrom.Configuration(configuration) 
    .CreateLogger();
builder.Host.UseSerilog();

var connectionString = builder.Configuration.GetConnectionString("TicketStoreDatabase");

builder.Host.UseWolverine(opts =>
{
    opts.UseFluentValidation();
    opts.Policies.MessageExecutionLogLevel(LogLevel.None);
    opts.CodeGeneration.TypeLoadMode = TypeLoadMode.Auto;
    opts.ApplicationAssembly = typeof(CreateScheduledEventCommandHandler).Assembly;
});

builder.Services.AddMarten(opts =>
{
    opts.Connection(connectionString!);
    opts.DisableNpgsqlLogging = true;
    opts.CreateDatabasesForTenants(c =>
    {
        c.ForTenant().CheckAgainstPgDatabase().ConnectionLimit(500);
    });
    opts.AutoCreateSchemaObjects = AutoCreate.CreateOrUpdate;
    opts.Events.StreamIdentity = StreamIdentity.AsGuid;
    opts.Projections.AddSocialEventProjections();
    // Indexes
    opts.AddSocialEventIndexes();
}).UseLightweightSessions()
    .AddAsyncDaemon(DaemonMode.HotCold)
    .IntegrateWithWolverine(cfg =>
    {
        cfg.UseWolverineManagedEventSubscriptionDistribution = true;
    });

builder.Services.AddControllers();
builder.Services.AddDomain();

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseMiddleware<ExceptionMiddleware>();
app.MapControllers();

app.Run();
