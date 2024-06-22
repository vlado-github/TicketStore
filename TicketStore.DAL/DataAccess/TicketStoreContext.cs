using Microsoft.EntityFrameworkCore;
using TicketStore.DAL.Entities;

namespace TicketStore.DAL.DataAccess;

public class TicketStoreContext : DbContext
{
    public TicketStoreContext(DbContextOptions options) : base(options)
    {
    }
    
    public DbSet<Event> Events { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        #region Event

        var eventEntity = modelBuilder.Entity<Event>();
        eventEntity.HasKey(x => x.Id);
        eventEntity.Property(x => x.StartTime).IsRequired();

        #endregion
        
        base.OnModelCreating(modelBuilder);
    }
}