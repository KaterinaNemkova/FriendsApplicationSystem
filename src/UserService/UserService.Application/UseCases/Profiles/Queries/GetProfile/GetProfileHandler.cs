using MediatR;
using UserService.Application.DTOs;
using UserService.Domain.Contracts;
using UserService.Domain.Entities;

namespace UserService.Application.UseCases.Profiles.Queries.GetProfile;

// public class GetProfileHandler:IRequestHandler<GetProfileQuery,ProfileDto>
// {
//     private readonly IProfileRepository _profileRepository;
//
//     public GetProfileHandler(IProfileRepository profileRepository)
//     {
//         _profileRepository = profileRepository;
//     }
//     public Task<ProfileDto> Handle(GetProfileQuery request, CancellationToken cancellationToken)
//     {
//         return "";
//     }
// }