using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace StudentManagementAPI.DTOs
{
    public class ClassDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string ClassCode { get; set; } = string.Empty;
        public int MaxStudents { get; set; }
    }

    public class CreateClassDto
    {
        [Required(ErrorMessage = "Tên lớp là bắt buộc.")]
        [MaxLength(50, ErrorMessage = "Tên lớp không được vượt quá 50 ký tự.")]
        public string Name { get; set; } = string.Empty;

        [Required(ErrorMessage = "Mã lớp là bắt buộc.")]
        [MinLength(5, ErrorMessage = "Mã lớp phải có ít nhất 5 ký tự.")]
        public string ClassCode { get; set; } = string.Empty;

        [Required(ErrorMessage = "Số sinh viên tối đa là bắt buộc.")]
        [Range(1, int.MaxValue, ErrorMessage = "Số sinh viên tối đa phải lớn hơn 0.")]
        public int MaxStudents { get; set; }
    }

    public class UpdateClassDto
    {
        [Required(ErrorMessage = "Tên lớp là bắt buộc.")]
        [MaxLength(50, ErrorMessage = "Tên lớp không được vượt quá 50 ký tự.")]
        public string Name { get; set; } = string.Empty;

        [Required(ErrorMessage = "Mã lớp là bắt buộc.")]
        [MinLength(5, ErrorMessage = "Mã lớp phải có ít nhất 5 ký tự.")]
        public string ClassCode { get; set; } = string.Empty;

        [Required(ErrorMessage = "Số sinh viên tối đa là bắt buộc.")]
        [Range(1, int.MaxValue, ErrorMessage = "Số sinh viên tối đa phải lớn hơn 0.")]
        public int MaxStudents { get; set; }
    }

    public class AddStudentsToClassDto
    {
        [Required(ErrorMessage = "Id lớp học là bắt buộc.")]
        public Guid ClassId { get; set; }

        [Required(ErrorMessage = "Danh sách Id sinh viên là bắt buộc.")]
        [MinLength(1, ErrorMessage = "Phải có ít nhất một sinh viên để thêm vào lớp.")]
        public List<Guid> StudentIds { get; set; } = new List<Guid>();
    }
}