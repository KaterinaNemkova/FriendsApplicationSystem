using System.IdentityModel.Tokens.Jwt;
using Swashbuckle.AspNetCore.Annotations;
using UserService.Application.Common.Exceptions;
using UserService.Application.UseCases.Friends.Commands.AcceptFriendRequest;
using UserService.Application.UseCases.Friends.Commands.RejectFriendRequest;
using UserService.Application.UseCases.Friends.Queries.GetAllMyFriendsRequests;
using UserService.Application.UseCases.Friends.Queries.GetFriendshipByFriendId;

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
public class FriendshipController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public FriendshipController(IMediator mediator, IHttpContextAccessor httpContextAccessor)
    {
        _mediator = mediator;
        _httpContextAccessor = httpContextAccessor;
    }
    
    [SwaggerOperation(Summary = "Add friend")]
    [HttpPost("new-friend/{friendId}")]
    public async Task<IActionResult> AddFriend([FromRoute] Guid friendId, CancellationToken token)
    {
        var profileId = GetProfileId();
        var friendship = await _mediator.Send(new AddFriendCommand(profileId, friendId),  token);

        return Ok(friendship);
    }

    [SwaggerOperation(Summary = "Accept friend's request")]
    [HttpPost("accept/{friendId}")]
    public async Task<IActionResult> AcceptFriendRequest([FromRoute] Guid friendId, CancellationToken token)
    {
        var profileId = GetProfileId();
        var acceptFriendship = await _mediator.Send(new AcceptFriendRequestCommand(profileId, friendId), token);

        return this.Ok(acceptFriendship);
    }
        
    [SwaggerOperation(Summary = "Reject friend's request")]
    [HttpDelete("reject/{friendId}")]
    public async Task<IActionResult> RejectFriendRequest([FromRoute] Guid friendId, CancellationToken token)
    {
        var profileId = GetProfileId();
        await _mediator.Send(new RejectFriendRequestCommand(profileId, friendId), token);
        return this.Ok();
    }

    [SwaggerOperation(Summary = "Change friendship's status")]
    [HttpPost("status/{friendshipId}")]
    public async Task<IActionResult> EstablishRelationStatus([FromRoute] Guid friendshipId, [FromQuery] RelationStatus relationStatus, CancellationToken token)
    {
        var friendship = await _mediator.Send(new EstablishRelationStatusCommand(friendshipId, relationStatus), token);

        return this.Ok(friendship);
    }

    [SwaggerOperation(Summary = "Delete friend")]
    [HttpDelete("remove/{friendId}")]
    public async Task<IActionResult> DeleteFriend([FromRoute] Guid friendId, CancellationToken token)
    {
        var profileId = GetProfileId();
        await _mediator.Send(new DeleteFriendCommand(profileId, friendId), token);

        return Ok();
    }

    [SwaggerOperation(Summary = "Change friendship's start date")]
    [HttpPut("start-date/{friendshipId}")]
    public async Task<IActionResult> ChangeStartDate([FromRoute] Guid friendshipId, [FromBody] DateOnly startDate, CancellationToken token)
    {
        var friendship = await _mediator.Send(new ChangeDateCommand(friendshipId, startDate), token);

        return this.Ok(friendship);
    }

    [SwaggerOperation(Summary = "Get my friends")]
    [HttpGet("my-friends")]
    public async Task<IActionResult> GetAllMyFriends(CancellationToken token)
    {
        var profileId = GetProfileId();
        var friends = await _mediator.Send(new GetAllFriendsQuery(profileId), token);

        return Ok(friends);
    }

    [SwaggerOperation(Summary = "Get my friend's requests")]
    [HttpGet("my-requests")]
    public async Task<IActionResult> GetAllMyFriendsRequests(CancellationToken token)
    {
        var profileId = GetProfileId();
        var friends = await _mediator.Send(new GetAllMyFriendsRequestsQuery(profileId), token);

        return Ok(friends);
    }
    
    [SwaggerOperation(Summary = "Get friendship by friend profile ID")]
    [HttpGet("by-friend/{friendId}")]
    public async Task<IActionResult> GetFriendshipByFriendId([FromRoute] Guid friendId, CancellationToken token)
    {

        var profileId = GetProfileId();
    
        var friendship = await _mediator.Send(new GetFriendshipByFriendIdQuery(profileId, friendId), token);
    
        return Ok(friendship);
    }

    private Guid GetProfileId()
    {
        var token = _httpContextAccessor.HttpContext?.Request.Cookies["accessToken"];
        if (string.IsNullOrEmpty(token))
            throw new UnauthorizedException("Unauthorized.", "Token not found in cookies.");

        token = token.Replace("Bearer ", "");

        var handler = new JwtSecurityTokenHandler();
        var jwtToken = handler.ReadJwtToken(token);

        var profileIdValue = jwtToken.Claims.First(c => c.Type == "profile_id").Value;

        if (!Guid.TryParse(profileIdValue, out var profileId))
            throw new UnauthorizedException("Unauthorized.", "Invalid Profile ID format.");

        return profileId;
    }

}