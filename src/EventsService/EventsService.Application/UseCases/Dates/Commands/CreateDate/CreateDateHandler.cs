namespace EventsService.Application.UseCases.Dates.Commands.CreateDate;

using AutoMapper;
using EventsService.Application.DTOs;
using EventsService.Domain.Contracts;
using EventsService.Domain.Entities;
using MediatR;

public class CreateDateHandler : IRequestHandler<CreateDateCommand, DateDto>
{
    private readonly IDateRepository _dateRepository;
    private readonly IMapper _mapper;

    public CreateDateHandler(IDateRepository dateRepository, IMapper mapper)
    {
        this._dateRepository = dateRepository;
        this._mapper = mapper;
    }

    public async Task<DateDto> Handle(CreateDateCommand request, CancellationToken cancellationToken)
    {
        Date date = new Date
        {
            Id = Guid.NewGuid(),
            ImportantDate = request.ImportantDate,
            Title = request.Title,
            Description = request.Description,
            ParticipantIds = request.ParticipantIds,
        };

        await this._dateRepository.CreateAsync(date, cancellationToken);

        return this._mapper.Map<DateDto>(date);
    }
}