using System;
using System.Collections.Generic;
using System.Linq;
using StudentManagementAPI.Data;
using StudentManagementAPI.DTOs;
using StudentManagementAPI.Models;
using StudentManagementAPI.Services.Interfaces;

namespace StudentManagementAPI.Services.Implementations
{
    public class StudentService : IStudentService
    {
        public StudentService()
        {
        }

        public IEnumerable<StudentDto> GetAllStudents()
        {
            return InMemoryDataStore.Students.Select(s => new StudentDto
            {
                Id = s.Id,
                Name = s.Name,
                DateOfBirth = s.DateOfBirth,
                StudentCode = s.StudentCode
            });
        }

        public StudentDto? GetStudentById(Guid id)
        {
            var student = InMemoryDataStore.Students.FirstOrDefault(s => s.Id == id);
            if (student == null) return null;

            return new StudentDto
            {
                Id = student.Id,
                Name = student.Name,
                DateOfBirth = student.DateOfBirth,
                StudentCode = student.StudentCode
            };
        }

        public StudentDto AddStudent(CreateStudentDto createStudentDto)
        {
            var studentId = Guid.NewGuid(); 

            var student = new Student
            {
                Id = studentId,
                Name = createStudentDto.Name,
                DateOfBirth = createStudentDto.DateOfBirth,
                StudentCode = createStudentDto.StudentCode
            };

            InMemoryDataStore.Students.Add(student);
            return new StudentDto
            {
                Id = student.Id,
                Name = student.Name,
                DateOfBirth = student.DateOfBirth,
                StudentCode = student.StudentCode
            };
        }

        public StudentDto? UpdateStudent(Guid id, UpdateStudentDto updateStudentDto)
        {
            var student = InMemoryDataStore.Students.FirstOrDefault(s => s.Id == id);
            if (student == null)
            {
                return null;
            }

            student.Name = updateStudentDto.Name;
            student.DateOfBirth = updateStudentDto.DateOfBirth;
            student.StudentCode = updateStudentDto.StudentCode;

            return new StudentDto
            {
                Id = student.Id,
                Name = student.Name,
                DateOfBirth = student.DateOfBirth,
                StudentCode = student.StudentCode
            };
        }

        public bool DeleteStudent(Guid id)
        {
            var student = InMemoryDataStore.Students.FirstOrDefault(s => s.Id == id);
            if (student == null)
            {
                return false;
            }

            var studentClasses = InMemoryDataStore.StudentClasses.Where(sc => sc.StudentId == id).ToList();
            foreach (var studentClass in studentClasses)
            {
                InMemoryDataStore.StudentClasses.Remove(studentClass);
            }

            InMemoryDataStore.Students.Remove(student);
            return true;
        }
    }
}