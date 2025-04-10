namespace EventsService.Application.Validators;

using EventsService.Application.Common.Constants;
using EventsService.Application.DTOs;
using FluentValidation;

public class DateValidator : AbstractValidator<DateRequestDto>
{
    public DateValidator()
    {
        this.RuleFor(m => m.Title)
            .NotEmpty().WithMessage("Title is required.")
            .MaximumLength(ValidationConstants.MaxTitleLength).WithMessage($"Title must be less {ValidationConstants.MaxTitleLength} characters.");
        this.RuleFor(m => m.Description)
            .NotEmpty().WithMessage("Description is required.")
            .MaximumLength(ValidationConstants.MaxDescriptionLength).WithMessage($"Description must be less {ValidationConstants.MaxDescriptionLength} characters.");
        this.RuleFor(m => m.ImportantDate)
            .NotEmpty().WithMessage("ImportantDate is required.");
        this.RuleFor(m => m.ParticipantIds)
            .NotEmpty().WithMessage("At least one participant");
    }
}