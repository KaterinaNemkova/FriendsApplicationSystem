using MediatR;
using Microsoft.AspNetCore.Mvc;
using UserService.Application.UseCases.Profiles.Commands.EstablishStatus;

namespace UserService.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProfileController: ControllerBase
{
    private readonly IMediator _mediator;
    
    public ProfileController(IMediator mediator)
    {
        _mediator = mediator;
    }
    
    // [HttpPut("establish-status")]
    //
    // public async Task<IActionResult> EstablishStatus([FromBody] EstablishStatusCommand command, CancellationToken cancellationToken)
    // {
    //     
    // }
    
}