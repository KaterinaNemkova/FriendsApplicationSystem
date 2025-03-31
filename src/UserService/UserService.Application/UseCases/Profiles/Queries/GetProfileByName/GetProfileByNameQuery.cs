using MediatR;
using UserService.Application.DTOs;

namespace UserService.Application.UseCases.Profiles.Queries.GetProfileByName;

public record GetProfileByNameQuery(string Name):IRequest<ProfileDto>;