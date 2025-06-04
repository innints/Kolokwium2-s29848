namespace Kolokwium2.DTOs;

public class EnrollmentGetDto
{
    public EnrollmentGetDtoStudent Student { get; set; } = null!;
    
    public EnrollmentGetDtoCourse Course { get; set; } = null!;

    
    public DateTime EnrollmentDate { get; set; }
}