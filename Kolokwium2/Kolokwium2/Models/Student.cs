using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace Kolokwium2.Models;

[Table("Student")]
public class Student
{
    [Key]
    [Column("ID")]
    public int IdStudent { get; set; } 
    
    [MaxLength(50)]
    public string FirstName { get; set; } = null!; 
    
    [MaxLength(100)]
    public string LastName { get; set; } = null!;

    [MaxLength(150)]
    [EmailAddress] //dodatkowo lepsze sprawdzenie
    public string? Email { get; set; }
    
    
    public virtual ICollection<Enrollment> Enrollments { get; set; } = null!; // => navigation property. Needed to map relationships between tables.
}