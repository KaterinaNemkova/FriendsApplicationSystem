using MediatR;
using Microsoft.AspNetCore.Mvc;
using UserService.Application.UseCases.Friends.Commands.AddFriend;

namespace UserService.Api.Controllers;

[ApiController]
[Route("api/controller")]
public class FriendshipController:ControllerBase
{
    private readonly IMediator _mediator;
    
    public FriendshipController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost("add-friend")]
    public async Task<IActionResult> AddFriend([FromQuery] AddFriendCommand command)
    {
        var frienship = await _mediator.Send(command);
        return Ok(frienship);
    }
}