using MediatR;
using UserService.Domain.Enums;

namespace UserService.Application.UseCases.Profiles.Commands.EstablishStatus;

public record EstablishStatusCommand(Guid Id, ActivityStatus activityStatus) : IRequest;
