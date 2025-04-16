using System;
using System.Collections.Generic;
using StudentManagementAPI.DTOs;
using StudentManagementAPI.Models; // Cần using Models

namespace StudentManagementAPI.Services.Interfaces
{
    public interface IClassService
    {
        IEnumerable<ClassDto> GetAllClasses();
        ClassDto? GetClassById(Guid id);
        ClassDto AddClass(CreateClassDto createClassDto);
        ClassDto? UpdateClass(Guid id, UpdateClassDto updateClassDto);
        bool DeleteClass(Guid id);
        bool AddStudentsToClass(AddStudentsToClassDto dto, out string errorMessage); // Thêm out param để trả về lỗi cụ thể
        IEnumerable<StudentDto> GetStudentsInClass(Guid classId);
    }
}