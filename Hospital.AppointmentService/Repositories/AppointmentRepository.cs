using Microsoft.EntityFrameworkCore;

namespace Hospital.AppointmentService.Repositories
{
    public class AppointmentRepository:IAppointmentRepository
    {
        private readonly AppointmentDbContext _context;

        public AppointmentRepository(AppointmentDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Appointment>> GetAllAsync()
            => await _context.Appointments.ToListAsync();

        public async Task AddAsync(Appointment appointment)
            => await _context.Appointments.AddAsync(appointment);

        public async Task SaveChangesAsync()
            => await _context.SaveChangesAsync();
    }
}
