namespace EventsService.Application.UseCases.Dates.Commands.DeleteDate;

using EventsService.Application.Common.Extensions;
using EventsService.Application.Contracts;
using EventsService.Domain.Entities;
using MediatR;

public class DeleteDateHandler : IRequestHandler<DeleteDateCommand>
{
    private readonly IDateRepository _dateRepository;

    public DeleteDateHandler(IDateRepository dateRepository)
    {
        this._dateRepository = dateRepository;
    }

    public async Task Handle(DeleteDateCommand request, CancellationToken cancellationToken)
    {
        var date = await this._dateRepository.GetByIdAsync(request.Id, cancellationToken)
                   ?? throw new EntityNotFoundException(nameof(Date), request.Id);

        await this._dateRepository.DeleteAsync(date.Id, cancellationToken);
    }
}