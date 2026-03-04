namespace Hospital.BedService.Validators
{
    using FluentValidation;
    using Hospital.BedService.DTOs;

    public class CreateBedValidator : AbstractValidator<CreateBedDto>
    {
        public CreateBedValidator()
        {
            RuleFor(x => x.BedNumber)
                .NotEmpty().WithMessage("Bed number is required")
                .MaximumLength(20)
                .Matches(@"^[A-Za-z0-9\-]+$")
                    .WithMessage("Bed number may only contain letters, digits and hyphens (e.g. A-101)");

            RuleFor(x => x.Ward)
                .NotEmpty().WithMessage("Ward is required")
                .MaximumLength(50);
        }
    }

    public class AssignBedValidator : AbstractValidator<AssignBedDto>
    {
        public AssignBedValidator()
        {
            RuleFor(x => x.PatientId)
                .NotEmpty().WithMessage("PatientId is required");
        }
    }
}