using System;
using System.Collections.Generic;
using StudentManagementAPI.DTOs;
using StudentManagementAPI.Models;

namespace StudentManagementAPI.Services.Interfaces
{
    public interface IStudentService
    {
        IEnumerable<StudentDto> GetAllStudents();
        StudentDto? GetStudentById(Guid id); 
        StudentDto AddStudent(CreateStudentDto createStudentDto);
        StudentDto? UpdateStudent(Guid id, UpdateStudentDto updateStudentDto);
        bool DeleteStudent(Guid id);
    }
}