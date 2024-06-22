using Microsoft.EntityFrameworkCore;
using TicketStore.DAL.DataAccess;
using TicketStore.Domain.EventFeature.Commands;
using Wolverine;
using Wolverine.EntityFrameworkCore;
using Wolverine.Postgresql;

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString(nameof(TicketStoreContext));

builder.Host.UseWolverine(opts =>
{
    opts.PersistMessagesWithPostgresql(connectionString);
    opts.UseEntityFrameworkCoreTransactions();
    opts.ApplicationAssembly = typeof(CreateEventCommandHandler).Assembly;
});

builder.Services.AddDbContext<TicketStoreContext>(options =>
{
    options.UseNpgsql(connectionString);
}, ServiceLifetime.Singleton); //Wolverine docs says weirdly Singleton

builder.Services.AddControllers(); 

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
