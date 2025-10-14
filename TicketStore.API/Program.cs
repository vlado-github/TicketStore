using JasperFx;
using JasperFx.CodeGeneration;
using JasperFx.Events.Daemon;
using Marten;
using TicketStore.Domain.DependencyInjection;
using TicketStore.Domain.SocialEventFeature.Commands;
using TicketStore.Domain.SocialEventFeature.Schema.Projections;
using Wolverine;

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("TicketStoreDatabase");

builder.Host.UseWolverine(opts =>
{
    opts.CodeGeneration.TypeLoadMode = TypeLoadMode.Auto;
    opts.ApplicationAssembly = typeof(CreateScheduledEventCommandHandler).Assembly;
});

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
app.MapControllers();

app.Run();
