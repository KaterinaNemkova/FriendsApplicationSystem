using MediatR;
using Microsoft.AspNetCore.Mvc;
using UserService.Application.DTOs;
using UserService.Application.UseCases.Profiles.Commands.CreateProfile;
using UserService.Application.UseCases.Profiles.Commands.DeleteImage;
using UserService.Application.UseCases.Profiles.Commands.EstablishStatus;
using UserService.Application.UseCases.Profiles.Commands.UploadImage;
using UserService.Application.UseCases.Profiles.Queries.GetAllByFilter;
using UserService.Application.UseCases.Profiles.Queries.GetPhoto;
using UserService.Application.UseCases.Profiles.Queries.GetProfileById;
using UserService.Application.UseCases.Profiles.Queries.GetProfileByName;
using UserService.Domain.Enums;

namespace UserService.Api.Controllers;

[ApiController]
[Route("api/controller")]
public class ProfileController: ControllerBase
{
    private readonly IMediator _mediator;
    
    public ProfileController(IMediator mediator)
    {
        _mediator = mediator;
    }
    
    [HttpPost("profile")]
    public async Task<IActionResult> CreateProfile([FromBody] CreateProfileCommand command, CancellationToken token)
    {
        var profile = await _mediator.Send(command, token);
        return Ok(profile);
    }

    [HttpGet("{id:guid}")]
    
    public async Task<IActionResult> GetProfileById([FromRoute] Guid id, CancellationToken token)
    {
        var profile = await _mediator.Send(new GetProfileByIdQuery(id),token);
        return Ok(profile);
    }
    
    [HttpGet("name")]
    public async Task<IActionResult> GetProfileByName([FromQuery] string name, CancellationToken token)
    {
        var profile = await _mediator.Send(new GetProfileByNameQuery(name), token);
        return Ok(profile);
    }
    
    [HttpGet]
    public async Task<IActionResult> GetProfilesByFilter([FromQuery] GetAllByFilterQuery query, CancellationToken token)
    {
        var profiles = await _mediator.Send(query,token);
        return Ok(profiles);
    }

    [HttpGet("{id:guid}/photo")]
    public async Task<IActionResult> GetProfilePhoto([FromRoute] Guid id,CancellationToken token)
    {
        var url = await _mediator.Send(new GetPhotoByIdQuery(id),token);
        return Ok(url);
    }
    
    [HttpPost("{profileId:guid}/photo")]
    [Consumes("multipart/form-data")]
    public async Task<IActionResult> UploadProfilePhoto([FromRoute] Guid profileId, [FromForm] UploadImageRequest request,CancellationToken token)
    {
        var result = await _mediator.Send(new UploadImageCommand(profileId, request.File), token);
        return Ok(result);
    }

    
    [HttpDelete("{profileId:guid}")]
    public async Task<IActionResult> DeletePhoto([FromRoute] Guid profileId,CancellationToken token)
    {
        var result = await _mediator.Send(new DeleteImageCommand(profileId), token);
        return Ok(result);
    }

    [HttpPost("{profileId:guid}/status")]
    public async Task<IActionResult> EstablishStatus([FromRoute] Guid profileId, [FromQuery]ActivityStatus activityStatus,CancellationToken token)
    {
        await _mediator.Send(new EstablishStatusCommand(profileId,activityStatus), token);
        return Ok();
    }
}