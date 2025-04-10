using EventsService.Application.Common.Constants;

namespace EventsService.Application.Validators;

using EventsService.Application.DTOs.Goals;
using FluentValidation;

public class GoalValidator : AbstractValidator<GoalRequestDto>
{
    public GoalValidator()
    {
        this.RuleFor(m => m.Title)
            .NotEmpty().WithMessage("Title is required.")
            .MaximumLength(ValidationConstants.MaxTitleLength).WithMessage($"Title must be less {ValidationConstants.MaxTitleLength} characters.");
        this.RuleFor(m => m.Description)
            .NotEmpty().WithMessage("Description is required.")
            .MaximumLength(ValidationConstants.MaxDescriptionLength).WithMessage($"Description must be less {ValidationConstants.MaxDescriptionLength} characters.");
        this.RuleFor(m => m.TargetDate)
            .NotEmpty().WithMessage("TargetDate is required.");
        this.RuleFor(m => m.ParticipantIds)
            .NotEmpty().WithMessage("At least one participant");
    }
}