using FluentValidation;
using Hospital.DischargeService.DTOs;

namespace Hospital.DischargeService.Validators
{
    public class DischargeSummaryValidator : AbstractValidator<DischargeSummaryDto>
    {
        private static readonly string[] ValidGenders = ["Male", "Female", "Other"];

        public DischargeSummaryValidator()
        {
            RuleFor(x => x.PatientId)
                .NotEmpty().WithMessage("PatientId is required.");

            RuleFor(x => x.PatientName)
                .NotEmpty().WithMessage("PatientName is required.")
                .MaximumLength(150);

            RuleFor(x => x.PatientAge)
                .InclusiveBetween(0, 150).WithMessage("PatientAge must be between 0 and 150.");

            // Gender is optional — if provided it must be one of the valid values
            RuleFor(x => x.PatientGender)
                .Must(g => string.IsNullOrEmpty(g) || ValidGenders.Contains(g))
                .WithMessage("PatientGender must be Male, Female, or Other (or leave empty).");

            RuleFor(x => x.Diagnosis)
                .NotEmpty().WithMessage("Diagnosis is required.")
                .MaximumLength(500);

            RuleFor(x => x.Treatment)
                .NotEmpty().WithMessage("Treatment is required.")
                .MaximumLength(1000);

            RuleFor(x => x.Medications)
                .MaximumLength(1000)
                .When(x => !string.IsNullOrEmpty(x.Medications));

            RuleFor(x => x.FollowUpInstructions)
                .MaximumLength(1000)
                .When(x => !string.IsNullOrEmpty(x.FollowUpInstructions));

            RuleFor(x => x.DischargingDoctorId)
                .NotEmpty().WithMessage("DischargingDoctorId is required.");
        }
    }
}
