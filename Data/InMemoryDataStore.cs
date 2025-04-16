using System;
using System.Collections.Generic;
using StudentManagementAPI.Models;

namespace StudentManagementAPI.Data
{
    public static class InMemoryDataStore
    {
        public static List<Student> Students { get; } = new List<Student>();
        public static List<Class> Classes { get; } = new List<Class>();
        public static List<StudentClass> StudentClasses { get; } = new List<StudentClass>();

        static InMemoryDataStore()
        {
            var student1 = new Student { Id = Guid.NewGuid(), Name = "Nguyen Van A", StudentCode = "SV001", DateOfBirth = new DateOnly(2003, 5, 10) };
            var student2 = new Student { Id = Guid.NewGuid(), Name = "Tran Thi B", StudentCode = "SV002", DateOfBirth = new DateOnly(2004, 1, 15) };
            var class1 = new Class { Id = Guid.NewGuid(), Name = "Lap Trinh C#", ClassCode = "CSHARP01", MaxStudents = 30 };

            Students.Add(student1);
            Students.Add(student2);
            Classes.Add(class1);

            StudentClasses.Add(new StudentClass { Id = Guid.NewGuid(), StudentId = student1.Id, ClassId = class1.Id });
        }
    }
}