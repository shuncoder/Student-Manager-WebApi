using System;
using System.Collections.Generic;

namespace StudentManagementAPI.Models
{
    public class Class
    {
        public Guid Id { get; set; }
        public required string Name { get; set; } 
        public string ClassCode { get; set; } = string.Empty;
        public int MaxStudents { get; set; }
    }
}