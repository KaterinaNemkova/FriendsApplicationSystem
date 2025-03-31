using MediatR;
using UserService.Application.DTOs;
using UserService.Domain.Enums;

namespace UserService.Application.UseCases.Profiles.Queries.GetAllByFilter;

public record GetAllByFilterQuery : IRequest<List<ProfileDto>>
{
    public string? Name {get; set;}
    
    public ActivityStatus? ActivityStatus {get; set;}
   
}
