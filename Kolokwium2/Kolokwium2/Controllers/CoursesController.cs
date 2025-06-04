using Kolokwium2.DTOs;
using Kolokwium2.Models;
using Kolokwium2.Services;



using Microsoft.AspNetCore.Mvc;
namespace Kolokwium2.Controllers;

[ApiController]
[Route("/api/[controller]")]
public class CoursesController(IDbService service) : ControllerBase
{
    [HttpPost("/with-enrollments")]
    public async Task<IActionResult> CreateNewCourseWithAssignedStudents([FromBody] CourseCreateDto courseDto)
    {
        var course=await service.CreateNewCourseWithAssignedStudentsAsync(courseDto);
        return Created($"api/courses/{course.IdCourse}",course);
    }
    
    
}