using Microsoft.EntityFrameworkCore;
using crud_web_api_mvc.Models;

namespace crud_web_api_mvc.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }

    public DbSet<Cliente> Clientes { get; set; } = null!;
}