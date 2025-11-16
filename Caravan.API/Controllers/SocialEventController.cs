using Caravan.Domain.Base;
using Caravan.Domain.Shared.Enums;
using Caravan.Domain.SocialEventFeature.Commands;
using Caravan.Domain.SocialEventFeature.Queries;
using Caravan.Domain.SocialEventFeature.Schema.Aggregates;
using Caravan.Domain.SocialEventFeature.Schema.Projections;
using Microsoft.AspNetCore.Mvc;
using Wolverine;

namespace Caravan.API.Controllers;

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
    
    [HttpPut("publish")]
    public async Task PublishSocialEvent(PublishSocialEventCommand command)
    {
        await _bus.InvokeAsync<CommandResult>(command);
    }
    
    [HttpPut("cancel")]
    public async Task CancelSocialEvent(CancelSocialEventCommand command)
    {
        await _bus.InvokeAsync<CommandResult>(command);
    }
    
    [HttpGet("{id:guid}")]
    public async Task<SocialEvent> GetSocialEvent([FromRoute] Guid id)
    {
        return await _query.GetById(id);
    }
    
    [HttpGet("list/{pageNumber}/{pageSize}")]
    public async Task<PagedResult<SocialEventProfileDetails>> GetSocialEvents([FromRoute] int pageNumber, [FromRoute] int pageSize)
    {
        return await _query.List(EventStatus.Published, pageNumber, pageSize);
    }
}