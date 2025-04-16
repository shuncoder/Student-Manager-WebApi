using System;
using System.ComponentModel.DataAnnotations;

namespace StudentManagementAPI.DTOs
{
    public class StudentDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string StudentCode { get; set; } = string.Empty;
        public DateOnly DateOfBirth { get; set; }
        // Class association will be retrieved through StudentClass if needed
    }

    public class CreateStudentDto
    {
        [Required(ErrorMessage = "Tên sinh viên là bắt buộc.")]
        [MaxLength(50, ErrorMessage = "Tên sinh viên không được vượt quá 50 ký tự.")]
        public string Name { get; set; } = string.Empty;

        [Required(ErrorMessage = "Mã số sinh viên là bắt buộc.")]
        public string StudentCode { get; set; } = string.Empty;

        [Required(ErrorMessage = "Ngày sinh là bắt buộc.")]
        public DateOnly DateOfBirth { get; set; }
    }

    public class UpdateStudentDto
    {
        [Required(ErrorMessage = "Tên sinh viên là bắt buộc.")]
        [MaxLength(50, ErrorMessage = "Tên sinh viên không được vượt quá 50 ký tự.")]
        public string Name { get; set; } = string.Empty;

        [Required(ErrorMessage = "Mã số sinh viên là bắt buộc.")]
        public string StudentCode { get; set; } = string.Empty;

        [Required(ErrorMessage = "Ngày sinh là bắt buộc.")]
        public DateOnly DateOfBirth { get; set; }
    }
}