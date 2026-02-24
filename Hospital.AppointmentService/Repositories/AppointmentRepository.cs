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

    public async Task AddAsync(Appointment appointment)
        => await _context.Appointments.AddAsync(appointment);

    public async Task SaveChangesAsync()
        => await _context.SaveChangesAsync();
}
