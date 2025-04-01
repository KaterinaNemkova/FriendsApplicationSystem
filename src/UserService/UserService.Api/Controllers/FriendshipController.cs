using MediatR;
using Microsoft.AspNetCore.Mvc;
using UserService.Application.UseCases.Friends.Commands.AddFriend;
using UserService.Application.UseCases.Friends.Commands.ChangeData;
using UserService.Application.UseCases.Friends.Commands.DeleteFriend;
using UserService.Application.UseCases.Friends.Commands.EstablishRelationStatus;
using UserService.Application.UseCases.Friends.Queries.GetAllFriends;
using UserService.Domain.Enums;

namespace UserService.Api.Controllers;

[ApiController]
[Route("api/controller")]
public class FriendshipController(IMediator mediator) : ControllerBase
{
    [HttpPost("add-friend")]
    public async Task<IActionResult> AddFriend([FromQuery] AddFriendCommand command)
    {
        var friendship = await mediator.Send(command);
        return Ok(friendship);
    }

    [HttpPost("establish-relation-status/{friendshipId:guid}")]
    public async Task<IActionResult> EstablishRelationStatus([FromRoute] Guid friendshipId,[FromQuery] RelationStatus relationStatus)
    {
        var friendship=await mediator.Send(new EstablishRelationStatusCommand(friendshipId, relationStatus));
        return Ok(friendship);
    }

    [HttpDelete("delete-friend")]
    public async Task<IActionResult> DeleteFriend([FromQuery] DeleteFriendCommand command)
    {
        await mediator.Send(command);
        return Ok();
    }

    [HttpPost("change-start-date/{friendshipId:guid}")]
    public async Task<IActionResult> ChangeStartDate([FromRoute] Guid friendshipId,[FromBody] DateOnly startDate)
    {
        var friendship=await mediator.Send(new ChangeDateCommand(friendshipId, startDate));
        return Ok(friendship);
    }

    [HttpGet("get-all-my-friends/{profileId:guid}")]
    public async Task<IActionResult> GetAllMyFriends([FromRoute] Guid profileId)
    {
        var friends = await mediator.Send(new GetAllFriendsQuery(profileId));
        return Ok(friends);
    }
    
}