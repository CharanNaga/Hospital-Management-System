using Hospital.AppointmentService.Data;
using Hospital.AppointmentService.Models;
using Microsoft.EntityFrameworkCore;

public class AppointmentRepository : IAppointmentRepository
{
    private readonly AppointmentDbContext _context;

    public AppointmentRepository(AppointmentDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Appointment>> GetAllAsync()
        => await _context.Appointments.ToListAsync();

    public async Task<Appointment?> GetByIdAsync(Guid id)
        => await _context.Appointments.FindAsync(id);

    public async Task<bool> HasConflictAsync(Guid doctorId, DateTime date, Guid? excludeId = null)
    {
        var window = TimeSpan.FromMinutes(30);
        return await _context.Appointments.AnyAsync(a =>
            a.DoctorId == doctorId
            && a.Status == "Scheduled"
            && a.Id != excludeId
            && a.AppointmentDate >= date.Subtract(window)
            && a.AppointmentDate <= date.Add(window));
    }

    public async Task AddAsync(Appointment appointment)
        => await _context.Appointments.AddAsync(appointment);

    public async Task SaveChangesAsync()
        => await _context.SaveChangesAsync();
}
