using System;

namespace StudentManagementAPI.Models
{
    public class Student
    {
        public Guid Id { get; set; }
        public required string Name { get; set; }
        public DateOnly DateOfBirth { get; set; }
        public string StudentCode { get; set; } = string.Empty;
    }
}