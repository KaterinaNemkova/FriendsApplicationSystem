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
            .MaximumLength(ValidationConstants.MaxDescriptionLength).WithMessage($"Description must be less {ValidationConstants.MaxDescriptionLength} characters.");
            
        this.RuleFor(m => m.Day)
            .NotEmpty().InclusiveBetween(1, 31);
            
        this.RuleFor(m => m.Month)
            .NotEmpty().InclusiveBetween(1, 12);
            
        // Валидация типа
        this.RuleFor(m => m.Type)
            .NotEmpty().WithMessage("Event type is required (birthday, anniversary, reminder, etc).");
            
        // Год опционален, но если есть - должен быть адекватным
        this.RuleFor(m => m.Year)
            .GreaterThan(1900).When(m => m.Year.HasValue);
    }
}