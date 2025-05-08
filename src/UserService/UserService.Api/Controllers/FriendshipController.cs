namespace UserService.Api.Controllers;

using MediatR;
using Microsoft.AspNetCore.Mvc;
using UserService.Application.UseCases.Friends.Commands.AddFriend;
using UserService.Application.UseCases.Friends.Commands.ChangeData;
using UserService.Application.UseCases.Friends.Commands.DeleteFriend;
using UserService.Application.UseCases.Friends.Commands.EstablishRelationStatus;
using UserService.Application.UseCases.Friends.Queries.GetAllFriends;
using UserService.Domain.Enums;

[ApiController]
[Route("api/friendship")]
public class FriendshipController(IMediator mediator) : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> AddFriend([FromQuery] AddFriendCommand command, CancellationToken token)
    {
        var friendship = await mediator.Send(command, token);

        return Ok(friendship);
    }

    [HttpPost("status/{friendshipId:Guid}")]
    public async Task<IActionResult> EstablishRelationStatus([FromRoute] Guid friendshipId, [FromQuery] RelationStatus relationStatus,CancellationToken token)
    {
        var friendship=await mediator.Send(new EstablishRelationStatusCommand(friendshipId, relationStatus), token);
        return Ok(friendship);
    }

    [HttpDelete]
    public async Task<IActionResult> DeleteFriend([FromQuery] DeleteFriendCommand command, CancellationToken token)
    {
        await mediator.Send(command, token);
        return Ok();
    }

    [HttpPost("start-date/{friendshipId:Guid}")]
    public async Task<IActionResult> ChangeStartDate([FromRoute] Guid friendshipId, [FromBody] DateOnly startDate, CancellationToken token)
    {
        var friendship = await mediator.Send(new ChangeDateCommand(friendshipId, startDate), token);
        return Ok(friendship);
    }

    [HttpGet("my-friends/{profileId:Guid}")]
    public async Task<IActionResult> GetAllMyFriends([FromRoute] Guid profileId, CancellationToken token)
    {
        var friends = await mediator.Send(new GetAllFriendsQuery(profileId), token);
        return Ok(friends);
    }

}