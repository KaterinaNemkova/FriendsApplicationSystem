namespace EventsService.Application.UseCases.Dates.Commands.DeleteDate;

using MediatR;

public record DeleteDateCommand(Guid Id) : IRequest;