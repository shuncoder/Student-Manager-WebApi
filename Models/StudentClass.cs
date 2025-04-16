using System;

namespace StudentManagementAPI.Models
{
    public class StudentClass
    {
        public Guid Id { get; set; }
        public Guid StudentId { get; set; }
        public Guid ClassId { get; set; }
    }
}