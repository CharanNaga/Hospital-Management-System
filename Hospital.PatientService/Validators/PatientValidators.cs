using FluentValidation;
using Hospital.PatientService.DTOs;

namespace Hospital.PatientService.Validators;

public class CreatePatientValidator : AbstractValidator<CreatePatientDto>
{
    public CreatePatientValidator()
    {
        RuleFor(x => x.FullName)
            .NotEmpty().WithMessage("Full name is required")
            .MaximumLength(100)
            .Matches(@"^[a-zA-Z\s\-\']+$")
                .WithMessage("Name may only contain letters, spaces, hyphens and apostrophes");

        RuleFor(x => x.Age)
            .InclusiveBetween(0, 120).WithMessage("Age must be between 0 and 120");

        RuleFor(x => x.Gender)
            .Must(g => new[] { "Male", "Female", "Other" }.Contains(g))
            .WithMessage("Gender must be Male, Female, or Other");

        RuleFor(x => x.Phone)
            .NotEmpty()
            .Matches(@"^\+?[0-9\-\s]{7,20}$")
                .WithMessage("Phone must be 7-20 digits, optionally prefixed with +");

        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("Email is required")
            .EmailAddress().WithMessage("Must be a valid email address")
            .MaximumLength(150);

        RuleFor(x => x.Address)
            .NotEmpty().WithMessage("Address is required")
            .MaximumLength(200);
    }
}

public class UpdatePatientValidator : AbstractValidator<UpdatePatientDto>
{
    public UpdatePatientValidator()
    {
        RuleFor(x => x.FullName).NotEmpty().MaximumLength(100)
            .Matches(@"^[a-zA-Z\s\-\']+$");
        RuleFor(x => x.Age).InclusiveBetween(0, 120);
        RuleFor(x => x.Gender).Must(g => new[] { "Male", "Female", "Other" }.Contains(g));
        RuleFor(x => x.Phone).NotEmpty().Matches(@"^\+?[0-9\-\s]{7,20}$");
        RuleFor(x => x.Email).NotEmpty().EmailAddress().MaximumLength(150);
        RuleFor(x => x.Address).NotEmpty().MaximumLength(200);
    }
}
