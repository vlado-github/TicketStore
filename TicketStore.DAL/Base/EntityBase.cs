using System.ComponentModel.DataAnnotations;

namespace TicketStore.DAL.Base;

public class EntityBase
{
    [Key]
    public long Id { get; set; }
}