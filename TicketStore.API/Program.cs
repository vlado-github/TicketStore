using FluentValidation.Results;
using JasperFx;
using JasperFx.CodeGeneration;
using JasperFx.Events.Daemon;
using Marten;
using Serilog;
using TicketStore.API.Middlewares;
using TicketStore.Domain.DependencyInjection;
using TicketStore.Domain.SocialEventFeature.Commands;
using TicketStore.Domain.SocialEventFeature.Schema.Projections;
using Wolverine;
using Wolverine.FluentValidation;

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("TicketStoreDatabase");

builder.Host.UseWolverine(opts =>
{
    opts.UseFluentValidation();
    opts.CodeGeneration.TypeLoadMode = TypeLoadMode.Auto;
    opts.ApplicationAssembly = typeof(CreateScheduledEventCommandHandler).Assembly;
});

Log.Logger = new LoggerConfiguration()     
    .MinimumLevel.Debug()     
    .WriteTo.Console()          
    .CreateLogger();

builder.Host.UseSerilog();

builder.Services.AddMarten(opts =>
{
    opts.Connection(connectionString!);
    opts.AutoCreateSchemaObjects = AutoCreate.CreateOrUpdate;
    opts.Projections.AddSocialEventProjections();
}).UseLightweightSessions().AddAsyncDaemon(DaemonMode.HotCold);

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
