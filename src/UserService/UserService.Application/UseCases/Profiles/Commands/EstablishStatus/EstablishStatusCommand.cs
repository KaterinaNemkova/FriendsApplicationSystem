namespace UserService.Application.UseCases.Profiles.Commands.EstablishStatus;

using MediatR;
using UserService.Domain.Enums;

public record EstablishStatusCommand(Guid ProfileId, ActivityStatus ActivityStatus) : IRequest;

