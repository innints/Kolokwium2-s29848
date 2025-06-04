using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
namespace Kolokwium2.Models;

[Table("Enrollment")]
[PrimaryKey(nameof(IdStudent), nameof(IdCourse))]
public class Enrollment
{
    
    [Column("Student_ID")]
    public int IdStudent { get; set; }
    
    [Column("Course_ID")]
    public int IdCourse { get; set; }
    public DateTime EnrollmentDate { get; set; }
    
    [ForeignKey(nameof(IdStudent))]
    public virtual Student Student { get; set; } = null!;
    
    [ForeignKey(nameof(IdCourse))]
    public virtual Course Course { get; set; } = null!;
}