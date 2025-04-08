namespace EventsService.Application.Validators;

using EventsService.Application.DTOs;
using FluentValidation;

public class DateValidator : AbstractValidator<DateRequestDto>
{
    public DateValidator()
    {
        this.RuleFor(m => m.Title)
            .NotEmpty().WithMessage("Title is required.")
            .MaximumLength(20).WithMessage("Title must be less 15 characters.");
        this.RuleFor(m => m.Description)
            .NotEmpty().WithMessage("Description is required.")
            .MaximumLength(500).WithMessage("Description must be less 500 characters.");
        this.RuleFor(m => m.ImportantDate)
            .NotEmpty().WithMessage("ImportantDate is required.");
        this.RuleFor(m => m.ParticipantIds)
            .NotEmpty().WithMessage("At least one participant");
    }
}