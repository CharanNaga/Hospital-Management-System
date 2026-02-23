using Hospital.AuthService.Models;
using Microsoft.EntityFrameworkCore;

namespace Hospital.AuthService.Data;

public class AuthDbContext : DbContext
{
    public AuthDbContext(DbContextOptions<AuthDbContext> options)
        : base(options) { }

    public DbSet<User> Users => Set<User>();
}