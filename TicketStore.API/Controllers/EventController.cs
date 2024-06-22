using Microsoft.AspNetCore.Mvc;
using TicketStore.Domain.EventFeature.Commands;
using Wolverine;

namespace TicketStore.API.Controllers;

[ApiController]
[Route("[controller]")]
public class EventController : ControllerBase
{
    private readonly IMessageBus _bus;
    
    public EventController(IMessageBus bus)
    {
        _bus = bus;
    }
    
    [HttpPost]
    public async Task CreateEvent(CreateEventCommand command)
    {
        await _bus.InvokeAsync(command);
    }
}