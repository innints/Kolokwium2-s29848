namespace Kolokwium2.DTOs;

public class EnrollmentGetDtoStudent
{
    public int IdStudent { get; set; }
    public string FirstName { get; set; } = null!;
    public string LastName { get; set; } = null!;
    public string? Email { get; set; }
}