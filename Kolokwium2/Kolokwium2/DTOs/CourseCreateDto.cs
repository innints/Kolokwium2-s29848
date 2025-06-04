using System.ComponentModel.DataAnnotations;

namespace Kolokwium2.DTOs;

public class CourseCreateDto
{
    [Required]
    [MaxLength(150)]
    public string Title { get; set; } = null!; 
    
   
    [MaxLength(300)]
    public string? Credits { get; set; }

    [Required]
    [MaxLength(150)]
    public string Teacher { get; set; } = null!;

    
    [Required]
    public ICollection<CourseCreateDtoStudent> Students { get; set; }= null!;

}