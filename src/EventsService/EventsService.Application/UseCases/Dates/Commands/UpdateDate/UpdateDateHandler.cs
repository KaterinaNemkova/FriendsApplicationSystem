namespace EventsService.Application.UseCases.Dates.Commands.UpdateDate;

using AutoMapper;
using EventsService.Application.Common.Extensions;
using EventsService.Application.DTOs;
using EventsService.Domain.Contracts;
using EventsService.Domain.Entities;
using MediatR;

public class UpdateDateHandler : IRequestHandler<UpdateDateCommand, DateDto>
{
    private readonly IDateRepository _dateRepository;
    private readonly IMapper _mapper;

    public UpdateDateHandler(IDateRepository dateRepository, IMapper mapper)
    {
        this._dateRepository = dateRepository;
        this._mapper = mapper;
    }

    public async Task<DateDto> Handle(UpdateDateCommand request, CancellationToken cancellationToken)
    {
        var date = await this._dateRepository.GetByIdAsync(request.Id, cancellationToken);
        if (date == null)
        {
            throw new EntityNotFoundException(nameof(Date), request.Id);
        }

        var newDate = this._mapper.Map<Date>(request.Dto);
        newDate.Id = request.Id;
        await this._dateRepository.UpdateAsync(newDate, cancellationToken);

        return this._mapper.Map<DateDto>(newDate);
    }
}