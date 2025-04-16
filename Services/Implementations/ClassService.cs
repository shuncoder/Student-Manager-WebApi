using System;
using System.Collections.Generic;
using System.Linq;
using StudentManagementAPI.Data;
using StudentManagementAPI.DTOs;
using StudentManagementAPI.Models;
using StudentManagementAPI.Services.Interfaces;

namespace StudentManagementAPI.Services.Implementations
{
    public class ClassService : IClassService
    {
        public ClassService()
        {
        }

        public IEnumerable<ClassDto> GetAllClasses()
        {
            return InMemoryDataStore.Classes.Select(c => new ClassDto
            {
                Id = c.Id,
                Name = c.Name,
                ClassCode = c.ClassCode,
                MaxStudents = c.MaxStudents
            });
        }

        public ClassDto? GetClassById(Guid id)
        {
            var classEntity = InMemoryDataStore.Classes.FirstOrDefault(c => c.Id == id);
            if (classEntity == null) return null;

            return new ClassDto
            {
                Id = classEntity.Id,
                Name = classEntity.Name,
                ClassCode = classEntity.ClassCode,
                MaxStudents = classEntity.MaxStudents
            };
        }

        public ClassDto AddClass(CreateClassDto createClassDto)
        {
            var classId = Guid.NewGuid();

            var classEntity = new Class
            {
                Id = classId,
                Name = createClassDto.Name,
                ClassCode = createClassDto.ClassCode,
                MaxStudents = createClassDto.MaxStudents
            };

            InMemoryDataStore.Classes.Add(classEntity);
            return new ClassDto
            {
                Id = classEntity.Id,
                Name = classEntity.Name,
                ClassCode = classEntity.ClassCode,
                MaxStudents = classEntity.MaxStudents
            };
        }

        public ClassDto? UpdateClass(Guid id, UpdateClassDto updateClassDto)
        {
            var classEntity = InMemoryDataStore.Classes.FirstOrDefault(c => c.Id == id);
            if (classEntity == null)
            {
                return null;
            }

            classEntity.Name = updateClassDto.Name;
            classEntity.ClassCode = updateClassDto.ClassCode;
            classEntity.MaxStudents = updateClassDto.MaxStudents;

            return new ClassDto
            {
                Id = classEntity.Id,
                Name = classEntity.Name,
                ClassCode = classEntity.ClassCode,
                MaxStudents = classEntity.MaxStudents
            };
        }

        public bool DeleteClass(Guid id)
        {
            var classEntity = InMemoryDataStore.Classes.FirstOrDefault(c => c.Id == id);
            if (classEntity == null)
            {
                return false;
            }

            var studentClasses = InMemoryDataStore.StudentClasses.Where(sc => sc.ClassId == id).ToList();
            foreach (var studentClass in studentClasses)
            {
                InMemoryDataStore.StudentClasses.Remove(studentClass);
            }

            InMemoryDataStore.Classes.Remove(classEntity);
            return true;
        }

        public bool AddStudentsToClass(AddStudentsToClassDto dto, out string errorMessage)
        {
            var classEntity = InMemoryDataStore.Classes.FirstOrDefault(c => c.Id == dto.ClassId);
            if (classEntity == null)
            {
                errorMessage = "Lớp học không tồn tại.";
                return false;
            }

            var currentStudentCount = InMemoryDataStore.StudentClasses.Count(sc => sc.ClassId == dto.ClassId);
            if (currentStudentCount + dto.StudentIds.Count > classEntity.MaxStudents)
            {
                errorMessage = $"Lớp học chỉ còn {classEntity.MaxStudents - currentStudentCount} chỗ trống.";
                return false;
            }

            foreach (var studentId in dto.StudentIds)
            {
                var student = InMemoryDataStore.Students.FirstOrDefault(s => s.Id == studentId);
                if (student == null)
                {
                    errorMessage = $"Sinh viên với ID {studentId} không tồn tại.";
                    return false;
                }

                if (InMemoryDataStore.StudentClasses.Any(sc => sc.StudentId == studentId && sc.ClassId == dto.ClassId))
                {
                    continue;
                }

                InMemoryDataStore.StudentClasses.Add(new StudentClass
                {
                    Id = Guid.NewGuid(),
                    StudentId = studentId,
                    ClassId = dto.ClassId
                });
            }

            errorMessage = string.Empty;
            return true;
        }

        public IEnumerable<StudentDto> GetStudentsInClass(Guid classId)
        {
            var studentIds = InMemoryDataStore.StudentClasses
                .Where(sc => sc.ClassId == classId)
                .Select(sc => sc.StudentId);

            return InMemoryDataStore.Students
                .Where(s => studentIds.Contains(s.Id))
                .Select(s => new StudentDto
                {
                    Id = s.Id,
                    Name = s.Name,
                    DateOfBirth = s.DateOfBirth,
                    StudentCode = s.StudentCode
                });
        }
    }
}