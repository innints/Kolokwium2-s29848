using System.ComponentModel.DataAnnotations;

namespace Kolokwium2.DTOs;

public class CourseCreateDtoStudent
{
    [Required]
    [MaxLength(50)]
    public string FirstName { get; set; } = null!; 
    
    [Required]
    [MaxLength(100)]
    public string LastName { get; set; } = null!;

    [Required]
    [MaxLength(150)]
    [EmailAddress] //dodatkowo lepsze sprawdzenie
    public string? Email { get; set; }
}