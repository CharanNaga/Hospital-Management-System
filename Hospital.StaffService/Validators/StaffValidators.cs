using FluentValidation;
using Hospital.StaffService.DTOs;

namespace Hospital.StaffService.Validators;

public class CreateStaffValidator : AbstractValidator<CreateStaffDto>
{
    private static readonly string[] ValidRoles = ["Nurse", "Technician", "Admin", "Porter", "Pharmacist"];
    private static readonly string[] ValidShifts = ["Day", "Night", "Rotating"];

    public CreateStaffValidator()
    {
        RuleFor(x => x.FullName)
            .NotEmpty().WithMessage("Full name is required")
            .MaximumLength(100)
            .Matches(@"^[a-zA-Z\s\.]+$")
                .WithMessage("Name may only contain letters, spaces and dots");

        RuleFor(x => x.Role)
            .NotEmpty()
            .Must(r => ValidRoles.Contains(r))
                .WithMessage($"Role must be one of: {string.Join(", ", ValidRoles)}");

        RuleFor(x => x.Department)
            .NotEmpty().WithMessage("Department is required")
            .MaximumLength(50);

        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("Email is required")
            .EmailAddress().WithMessage("Must be a valid email address")
            .MaximumLength(150);

        RuleFor(x => x.Phone)
            .NotEmpty()
            .Matches(@"^\+?[0-9\-\s]{7,20}$")
                .WithMessage("Phone must be 7-20 digits, optionally prefixed with +");

        RuleFor(x => x.Shift)
            .NotEmpty()
            .Must(s => ValidShifts.Contains(s))
                .WithMessage($"Shift must be one of: {string.Join(", ", ValidShifts)}");
    }
}

public class UpdateStaffValidator : AbstractValidator<UpdateStaffDto>
{
    private static readonly string[] ValidRoles = ["Nurse", "Technician", "Admin", "Porter", "Pharmacist"];
    private static readonly string[] ValidShifts = ["Day", "Night", "Rotating"];

    public UpdateStaffValidator()
    {
        RuleFor(x => x.FullName).NotEmpty().MaximumLength(100).Matches(@"^[a-zA-Z\s\.]+$");
        RuleFor(x => x.Role).Must(r => ValidRoles.Contains(r))
            .WithMessage($"Role must be one of: {string.Join(", ", ValidRoles)}");
        RuleFor(x => x.Department).NotEmpty().MaximumLength(50);
        RuleFor(x => x.Email).NotEmpty().EmailAddress().MaximumLength(150);
        RuleFor(x => x.Phone).NotEmpty().Matches(@"^\+?[0-9\-\s]{7,20}$");
        RuleFor(x => x.Shift).Must(s => ValidShifts.Contains(s))
            .WithMessage($"Shift must be one of: {string.Join(", ", ValidShifts)}");
    }
}