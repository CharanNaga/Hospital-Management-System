using FluentValidation;
using Hospital.AppointmentService.DTOs;

namespace Hospital.AppointmentService.Validators
{
    public class CreateAppointmentDtoValidator : AbstractValidator<CreateAppointmentDto>
    {
        public CreateAppointmentDtoValidator()
        {
            RuleFor(x => x.PatientId)
                .NotEmpty().WithMessage("PatientId is required.");

            RuleFor(x => x.DoctorId)
                .NotEmpty().WithMessage("DoctorId is required.");

            RuleFor(x => x.AppointmentDate)
                .NotEmpty().WithMessage("AppointmentDate is required.")
                .GreaterThan(DateTime.UtcNow)
                    .WithMessage("Appointment must be scheduled in the future.");

            RuleFor(x => x.Notes)
                .MaximumLength(500)
                    .WithMessage("Notes cannot exceed 500 characters.")
                .When(x => x.Notes is not null);

        }
    }

    public class UpdateStatusDtoValidator : AbstractValidator<UpdateAppointmentStatusDto>
    {
        private static readonly string[] AllowedStatuses = ["Scheduled", "Completed", "Cancelled"];

        public UpdateStatusDtoValidator()
        {
            RuleFor(x => x.Status)
                .NotEmpty().WithMessage("Status is required.")
                .Must(s => AllowedStatuses.Contains(s))
                    .WithMessage($"Status must be one of: {string.Join(", ", AllowedStatuses)}.");
        }
    }

}
