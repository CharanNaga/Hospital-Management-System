using Hospital.AuthService.Models;

public interface IAuthRepository
{
    Task<User?> ValidateUserAsync(string username, string password);
    Task<bool> UsernameExistsAsync(string username);

    Task AddUserAsync(User user);
    Task SaveChangesAsync();
}