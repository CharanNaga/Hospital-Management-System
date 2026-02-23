using Hospital.AuthService.Models;

public interface IAuthRepository
{
    Task<User?> ValidateUserAsync(string username, string password);
    Task AddUserAsync(User user);
    Task SaveChangesAsync();
}