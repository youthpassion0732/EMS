using DomainEntities;
using Microsoft.EntityFrameworkCore;

namespace DAL.Database;

public class DBContext : DbContext
{
    public DBContext(DbContextOptions<DBContext> options) : base(options) { }

    public DbSet<Employee> Employees { get; set; }
}
