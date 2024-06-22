using TicketStore.DAL.Base;
using TicketStore.Shared.Enums;

namespace TicketStore.DAL.Entities;

public class Event : EntityBase
{
    public string Title { get; set; }
    public EventType Type { get; set; }
    public string Venue { get; set; }
    public DateTimeOffset StartTime { get; set; }
    public DateTimeOffset? EndTime { get; set; }
}