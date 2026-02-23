namespace Hospital.AppointmentService.Repositories
{
    public interface IAppointmentRepository
    {
        Task<IEnumerable<Appointment>> GetAllAsync();
        Task AddAsync(Appointment appointment);
        Task SaveChangesAsync();
    }
}
