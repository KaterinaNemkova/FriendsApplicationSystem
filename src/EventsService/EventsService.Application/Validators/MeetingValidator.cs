using EventsService.Application.Common.Constants;

namespace EventsService.Application.Validators;

using EventsService.Application.DTOs.Meetings;
using FluentValidation;

public class MeetingValidator : AbstractValidator<MeetingRequestDto>
{
    public MeetingValidator()
    {
        this.RuleFor(m => m.Title)
            .NotEmpty().WithMessage("Title is required.")
            .MaximumLength(ValidationConstants.MaxTitleLength).WithMessage($"Title must be less {ValidationConstants.MaxTitleLength} characters.");
        this.RuleFor(m => m.Description)
            .NotEmpty().WithMessage("Description is required.")
            .MaximumLength(ValidationConstants.MaxDescriptionLength).WithMessage($"Description must be less {ValidationConstants.MaxDescriptionLength} characters.");
        this.RuleFor(m => m.Address)
            .NotEmpty().WithMessage("Address is required.")
            .MaximumLength(100).WithMessage("Address must be less 100 characters.");
        this.RuleFor(m => m.TimeOfMeet)
            .NotEmpty().WithMessage("TimeOfMeet is required.");
        this.RuleFor(m => m.ParticipantIds)
            .NotEmpty().WithMessage("At least one participant");
    }
}