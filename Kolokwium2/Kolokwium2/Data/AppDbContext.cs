using Kolokwium2.DTOs;
using Kolokwium2.Models;
using Microsoft.EntityFrameworkCore;

namespace Kolokwium2.Data;

//dotnet
/*
dotnet tool install --global dotnet-ef 
cd.\Kolokwium2
dotnet ef
dotnet ef migrations add "Add again maybe it will work"
dotnet ef database update
*/

public class AppDbContext : DbContext
{
    // Any class representing a table should be added here as a DbSet to be visible for migrations system
    public DbSet<Course> Courses { get; set; }
    public DbSet<Student> Students { get; set; }
    public DbSet<Enrollment> Enrollments { get; set; }
    
    
    
    
    
    public AppDbContext(DbContextOptions options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        
        var students = new List<Student>
        {
            new()
            {
                IdStudent = 1,
                FirstName = "John",
                LastName = "Doe",
                Email =  "john.doe@gmail.com",
            },
            new()
            {
                IdStudent = 2,
                FirstName = "Jane",
                LastName = "Allen",
                Email =  null,
            }
        };
        var courses = new List<Course>
        {
            new()
            {
                IdCourse = 1,
                Title="Kolokwium",
                Credits = null,
                Teacher = "dsaad"
            },
            new()
            {
                IdCourse = 2,
                Title="LaLaLa",
                Credits = "fafaf",
                Teacher = "so"
            }
        };
        var enrollments = new List<Enrollment>
        {
            new()
            {
                IdStudent=1,
                IdCourse = 1,
                EnrollmentDate = Convert.ToDateTime("2001-01-01")
            },
            new()
            {
                IdStudent=1,
                IdCourse = 2,
                EnrollmentDate = Convert.ToDateTime("2003-01-01")
            },
            new()
            {
                IdStudent=2,
                IdCourse = 1,
                EnrollmentDate = Convert.ToDateTime("2012-01-01")
            },
        };
        
        

        
        
         modelBuilder.Entity<Student>().HasData(students);
         modelBuilder.Entity<Course>().HasData(courses);
         modelBuilder.Entity<Enrollment>().HasData(enrollments);
         
       }
}