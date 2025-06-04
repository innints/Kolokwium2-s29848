namespace Kolokwium2.DTOs;

public class EnrollmentGetDtoCourse
{
    public int IdCourse { get; set; }
    public string Title { get; set; } = null!;
    public string Teacher { get; set; } = null!;
}