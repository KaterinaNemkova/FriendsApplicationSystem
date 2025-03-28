using MediatR;
using Microsoft.AspNetCore.Mvc;
using UserService.Application.DTOs;
using UserService.Application.UseCases.Profiles.Commands.CreateProfile;
using UserService.Application.UseCases.Profiles.Commands.DeleteImage;
using UserService.Application.UseCases.Profiles.Commands.EstablishStatus;
using UserService.Application.UseCases.Profiles.Commands.UploadImage;
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