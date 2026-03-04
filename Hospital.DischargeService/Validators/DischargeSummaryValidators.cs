using FluentValidation;
using Hospital.DischargeService.DTOs;

namespace Hospital.DischargeService.Validators
{
    public class DischargeSummaryValidator : AbstractValidator<DischargeSummaryDto>
    {
        public DischargeSummaryValidator()
        {
            RuleFor(x => x.PatientId)
                .NotEmpty().WithMessage("PatientId is required.");

            RuleFor(x => x.PatientName)
                .NotEmpty().WithMessage("PatientName is required.")
                .MaximumLength(150).WithMessage("PatientName cannot exceed 150 characters.");

            RuleFor(x => x.PatientAge)
                .InclusiveBetween(0, 150).WithMessage("PatientAge must be between 0 and 150.");

            RuleFor(x => x.Diagnosis)
                .NotEmpty().WithMessage("Diagnosis is required.")
                .MaximumLength(500).WithMessage("Diagnosis cannot exceed 500 characters.");

            RuleFor(x => x.Treatment)
                .NotEmpty().WithMessage("Treatment is required.")
                .MaximumLength(1000).WithMessage("Treatment cannot exceed 1000 characters.");

            RuleFor(x => x.Medications)
                .MaximumLength(1000).WithMessage("Medications cannot exceed 1000 characters.")
                .When(x => !string.IsNullOrEmpty(x.Medications));

            RuleFor(x => x.DischargingDoctorId)
                .NotEmpty().WithMessage("DischargingDoctorId is required.");
        }
    }

}
