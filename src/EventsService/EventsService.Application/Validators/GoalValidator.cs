namespace EventsService.Application.Validators;

using EventsService.Application.DTOs.Goals;
using FluentValidation;

public class GoalValidator : AbstractValidator<GoalRequestDto>
{
    public GoalValidator()
    {
        this.RuleFor(m => m.Title)
            .NotEmpty().WithMessage("Title is required.")
            .MaximumLength(20).WithMessage("Title must be less 15 characters.");
        this.RuleFor(m => m.Description)
            .NotEmpty().WithMessage("Description is required.")
            .MaximumLength(500).WithMessage("Description must be less 500 characters.");
        this.RuleFor(m => m.TargetDate)
            .NotEmpty().WithMessage("TargetDate is required.");
        this.RuleFor(m => m.ParticipantIds)
            .NotEmpty().WithMessage("At least one participant");
    }
}