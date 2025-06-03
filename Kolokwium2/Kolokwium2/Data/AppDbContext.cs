using Kolokwium2.Models;
using Microsoft.EntityFrameworkCore;

namespace Kolokwium2.Data;

public class AppDbContext : DbContext
{
    // Any class representing a table should be added here as a DbSet to be visible for migrations system
    public DbSet<Doctor> Doctors { get; set; }
    
    
    
    
    
    public AppDbContext(DbContextOptions options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        
        var doctors = new List<Doctor>
        {
            new()
            {
                IdDoctor = 1,
                FirstName = "John",
                LastName = "Doe",
                Email =  "john.doe@gmail.com",
            },
            new()
            {
                IdDoctor = 2,
                FirstName = "Jane",
                LastName = "Allen",
                Email =  "jane.allen@gmail.com",
            }
        };
        

        
        
         modelBuilder.Entity<Doctor>().HasData(doctors);
         
       }
}