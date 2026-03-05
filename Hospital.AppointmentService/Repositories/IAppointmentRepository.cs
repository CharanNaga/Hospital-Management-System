using Hospital.AppointmentService.Models;

public interface IAppointmentRepository
{
    Task<IEnumerable<Appointment>> GetAllAsync();
    Task<Appointment?> GetByIdAsync(Guid id);
    Task<bool> HasConflictAsync(Guid doctorId, DateTime date, Guid? excludeId = null);

    Task AddAsync(Appointment appointment);
    Task SaveChangesAsync();
}
