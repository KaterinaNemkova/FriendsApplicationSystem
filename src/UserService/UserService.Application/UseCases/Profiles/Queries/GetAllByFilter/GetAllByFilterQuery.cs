namespace UserService.Application.UseCases.Profiles.Queries.GetAllByFilter;

using MediatR;
using UserService.Application.DTOs.Profiles;
using UserService.Domain.Enums;

public record GetAllByFilterQuery(string? Name, ActivityStatus? ActivityStatus) : IRequest<List<ProfileDto>>;