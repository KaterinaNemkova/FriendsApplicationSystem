namespace UserService.Api.Controllers;

using System.Security.Claims;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using UserService.Application.UseCases.Friends.Commands.AcceptFriendRequest;
using UserService.Application.UseCases.Friends.Commands.AddFriend;
using UserService.Application.UseCases.Friends.Commands.ChangeData;
using UserService.Application.UseCases.Friends.Commands.DeleteFriend;
using UserService.Application.UseCases.Friends.Commands.EstablishRelationStatus;
using UserService.Application.UseCases.Friends.Commands.RejectFriendRequest;
using UserService.Application.UseCases.Friends.Queries.GetAllFriends;
using UserService.Domain.Enums;

[ApiController]
[Route("api/friends")]
public class FriendshipController(IMediator mediator) : ControllerBase
{
    [HttpPost("{profileId}/{friendId}/new-friend")]
    public async Task<IActionResult> AddFriend([FromRoute] Guid profileId, [FromRoute] Guid friendId, CancellationToken token)
    {
        var friendship = await mediator.Send(new AddFriendCommand(profileId, friendId), token);

        return this.Ok(friendship);
    }

    [HttpPost("{profileId}/{friendId}/accept-friend")]
    public async Task<IActionResult> AcceptFriendRequest([FromRoute] Guid profileId, [FromRoute] Guid friendId, CancellationToken token)
    {
        // var profileId = this.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
        var acceptFriendship = await mediator.Send(new AcceptFriendRequestCommand(profileId, friendId), token);

        return this.Ok(acceptFriendship);
    }

    [HttpDelete("{profileId}/{friendId}/reject-friend")]
    public async Task<IActionResult> RejectFriendRequest([FromRoute] Guid profileId, [FromRoute] Guid friendId, CancellationToken token)
    {
        //var profileId = this.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
        await mediator.Send(new RejectFriendRequestCommand(profileId, friendId), token);
        return this.Ok();
    }

    [HttpPost("{friendshipId:guid}/status")]
    public async Task<IActionResult> EstablishRelationStatus([FromRoute] Guid friendshipId, [FromQuery] RelationStatus relationStatus,CancellationToken token)
    {
        var friendship = await mediator.Send(new EstablishRelationStatusCommand(friendshipId, relationStatus), token);

        return this.Ok(friendship);
    }

    [HttpDelete]
    public async Task<IActionResult> DeleteFriend([FromQuery] DeleteFriendCommand command, CancellationToken token)
    {
        await mediator.Send(command, token);

        return this.Ok();
    }

    [HttpPost("{friendshipId:Guid}/start-date")]
    public async Task<IActionResult> ChangeStartDate([FromRoute] Guid friendshipId, [FromBody] DateOnly startDate,CancellationToken token)
    {
        var friendship = await mediator.Send(new ChangeDateCommand(friendshipId, startDate), token);
        return Ok(friendship);
    }

    [HttpGet("{profileId:Guid}")]
    public async Task<IActionResult> GetAllMyFriends([FromRoute] Guid profileId, CancellationToken token)
    {
        var friends = await mediator.Send(new GetAllFriendsQuery(profileId), token);
        return this.Ok(friends);
    }
}