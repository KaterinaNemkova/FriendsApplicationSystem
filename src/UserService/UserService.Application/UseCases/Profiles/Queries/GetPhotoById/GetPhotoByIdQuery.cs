using MediatR;

namespace UserService.Application.UseCases.Profiles.Queries.GetPhoto;

public record GetPhotoByIdQuery(Guid Id) : IRequest<string>;
