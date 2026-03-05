using Hospital.AuthService.Data;
using Hospital.AuthService.Models;
using Microsoft.EntityFrameworkCore;

public class AuthRepository : IAuthRepository
{
    private readonly AuthDbContext _context;

    public AuthRepository(AuthDbContext context)
    {
        _context = context;
    }

    public async Task<User?> ValidateUserAsync(string username, string password)
    {
        var user = await _context.Users
            .FirstOrDefaultAsync(x => x.Username == username);

        if (user == null)
            return null;

        return BCrypt.Net.BCrypt.Verify(password, user.Password) ? user : null;
    }

    public async Task<bool> UsernameExistsAsync(string username)
        => await _context.Users.AnyAsync(u =>
            u.Username.ToLower() == username.ToLower());


    public async Task AddUserAsync(User user)
    {
        await _context.Users.AddAsync(user);
    }

    public async Task SaveChangesAsync()
    {
        await _context.SaveChangesAsync();
    }
}
