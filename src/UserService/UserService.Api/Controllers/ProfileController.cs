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
    
 
    [HttpPost]
    public async Task<IActionResult> CreateProfile([FromBody] CreateProfileCommand command)
    {
        var profile = await _mediator.Send(command);
        return Ok(profile);
    }

    [HttpGet("get-profile/{id}")]
    
    public async Task<IActionResult> GetProfileById([FromRoute] Guid id)
    {
        var profile = await _mediator.Send(new GetProfileByIdQuery(id));
        return Ok(profile);
    }
    
    [HttpGet("get-profile-by-name")]
    public async Task<IActionResult> GetProfileById([FromQuery] string name)
    {
        var profile = await _mediator.Send(new GetProfileByNameQuery(name));
        return Ok(profile);
    }
    
    [HttpGet("get-profiles-by-filter")]
    public async Task<IActionResult> GetProfilesByFilter([FromQuery] GetAllByFilterQuery query)
    {
        var profiles = await _mediator.Send(query);
        return Ok(profiles);
    }

    [HttpGet("get-photo/{id}")]
    public async Task<IActionResult> GetProfilePhoto([FromRoute] Guid id)
    {
        var url = await _mediator.Send(new GetPhotoByIdQuery(id));
        return Ok(url);
    }
    
    [HttpPost("upload-photo/{profileId}")]
    [Consumes("multipart/form-data")]
    public async Task<IActionResult> UploadProfilePhoto([FromRoute] Guid profileId, [FromForm] UploadImageRequest request)
    {
        var result = await _mediator.Send(new UploadImageCommand(profileId, request.File));
        return Ok(result);
    }

    
    [HttpDelete("delete-photo/{profileId}")]
    public async Task<IActionResult> DeletePhoto([FromRoute] Guid profileId)
    {
        var result = await _mediator.Send(new DeleteImageCommand(profileId));
        return Ok(result);
    }

    [HttpPost("establish-status/{profileId}")]
    public async Task<IActionResult> EstablishStatus([FromRoute] Guid profileId, [FromQuery]ActivityStatus activityStatus)
    {
        await _mediator.Send(new EstablishStatusCommand(profileId,activityStatus));
        return Ok();
    }
}