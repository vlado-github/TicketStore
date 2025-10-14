using Marten.Pagination;
using Microsoft.AspNetCore.Mvc;
using TicketStore.Domain.Base;
using TicketStore.Domain.SocialEventFeature.Commands;
using TicketStore.Domain.SocialEventFeature.Queries;
using TicketStore.Domain.SocialEventFeature.Schema.Documents;
using Wolverine;

namespace TicketStore.API.Controllers;

[ApiController]
[Route("[controller]")]
public class SocialEventController : ControllerBase
{
    private readonly IMessageBus _bus;
    private readonly ISocialEventQuery  _query;
    
    public SocialEventController(IMessageBus bus, ISocialEventQuery query)
    {
        _bus = bus;
        _query = query;
    }
    
    [HttpPost]
    public async Task<CommandResult> CreateSocialEvent(CreateSocialEventCommand command)
    {
        return await _bus.InvokeAsync<CommandResult>(command);
    }
    
    [HttpGet("{id}")]
    public async Task<SocialEvent> GetSocialEvent([FromRoute] Guid id)
    {
        return await _query.GetById(id);
    }
    
    [HttpGet("list/{pageNumber}/{pageSize}")]
    public async Task<IPagedList<SocialEvent>> GetSocialEvent([FromRoute] int pageNumber, [FromRoute] int pageSize)
    {
        return await _query.List(pageNumber, pageSize);
    }
}