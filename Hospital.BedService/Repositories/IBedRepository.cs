namespace Hospital.BedService.Repositories
{
    public interface IBedRepository
    {
        Task<IEnumerable<Bed>> GetAllAsync();
        Task AddAsync(Bed bed);
        Task SaveChangesAsync();
    }
}
