

using Kolokwium2.Data;
using Kolokwium2.DTOs;
using Kolokwium2.Exceptions;
using Kolokwium2.Models;
using Microsoft.EntityFrameworkCore;

namespace Kolokwium2.Services;
public interface IDbService
{
    public Task<IEnumerable<EnrollmentGetDto>> GetAllEnrollmentsWithDetailsAsync();
    
    
    public Task<CourseWithNewEnrollmentsGetDto> CreateNewCourseWithAssignedStudentsAsync(CourseCreateDto courseDto);
    
}

public class DbService(AppDbContext data) : IDbService
{
    public async Task<IEnumerable<EnrollmentGetDto>> GetAllEnrollmentsWithDetailsAsync()
    {

        return await data.Enrollments
           .Select(e => new EnrollmentGetDto
            {
                EnrollmentDate = e.EnrollmentDate,
                
                Student = data.Students
                    .Where(s=>s.IdStudent==e.IdStudent)
                    .Select(s=>new EnrollmentGetDtoStudent
                    {
                        IdStudent = s.IdStudent,
                        FirstName = s.FirstName,
                        LastName = s.LastName,
                        Email = s.Email,
                        
                        
                    }).FirstOrDefault()!,
                Course = data.Courses
                    .Where(c=>c.IdCourse==e.IdCourse)
                    .Select(c=>new EnrollmentGetDtoCourse()
                    {
                        IdCourse = c.IdCourse,
                        Title = c.Title,
                        Teacher = c.Teacher
                        
                        
                    }).FirstOrDefault()!
                
                    
            }).ToListAsync();
    }

    public async Task<CourseWithNewEnrollmentsGetDto> CreateNewCourseWithAssignedStudentsAsync(CourseCreateDto courseDto)
    {
        await using var transaction = await data.Database.BeginTransactionAsync();
        try
        {
            var course = new Course()
            {
                Title = courseDto.Title,
                Credits = courseDto.Credits ?? null,
                Teacher = courseDto.Teacher,
            };
            await data.Courses.AddAsync(course);

            await data.SaveChangesAsync();
            
            foreach (var s in courseDto.Students)
            {
                var student = await data.Students.FirstOrDefaultAsync(p => p.FirstName == s.FirstName && p.LastName == s.LastName && p.Email == s.Email);//nie wiem czy to dla nulli zadziała
                if (student is null)
                {
                    var st = new Student
                    {

                        FirstName = s.FirstName,
                        LastName = s.LastName,
                        Email = s.Email ?? null,
                    };
                    await data.Students.AddAsync(st);
                    student = st;
                }
                await data.SaveChangesAsync();
                await data.Enrollments.AddAsync(new Enrollment
                    {
                        IdCourse = course.IdCourse,
                        IdStudent = student.IdStudent,
                        EnrollmentDate = DateTime.Now
                    }
                );
            }
            await data.SaveChangesAsync();
            
            
            
            
            
            
            
            await transaction.CommitAsync();

            return new CourseWithNewEnrollmentsGetDto
            {
                Message = "Kurs został utworzony i studenci zapisani",
                IdCourse = course.IdCourse,
            };
        }
        catch (Exception)
        {

            await transaction.RollbackAsync();
            throw;
        }
    }

    
    
}