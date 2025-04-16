using System;
using Microsoft.AspNetCore.Mvc;
using StudentManagementAPI.DTOs;
using StudentManagementAPI.Services.Interfaces;

namespace StudentManagementAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ClassesController : ControllerBase
    {
        private readonly IClassService _classService;
        private readonly ILogger<ClassesController> _logger;

        public ClassesController(IClassService classService, ILogger<ClassesController> logger)
        {
            _classService = classService;
            _logger = logger;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public ActionResult<IEnumerable<ClassDto>> GetClasses()
        {
            try
            {
                var classes = _classService.GetAllClasses();
                return Ok(classes);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Lỗi xảy ra khi lấy danh sách lớp học.");
                return StatusCode(StatusCodes.Status500InternalServerError, "Lỗi máy chủ nội bộ khi xử lý yêu cầu.");
            }
        }

        [HttpGet("{id:guid}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<ClassDto> GetClass(Guid id)
        {
            try
            {
                var cls = _classService.GetClassById(id);
                if (cls == null)
                {
                    return NotFound($"Không tìm thấy lớp học với Id: {id}");
                }
                return Ok(cls);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Lỗi xảy ra khi lấy lớp học với Id: {id}.");
                return StatusCode(StatusCodes.Status500InternalServerError, "Lỗi máy chủ nội bộ khi xử lý yêu cầu.");
            }
        }

        [HttpGet("{id:guid}/students")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<IEnumerable<StudentDto>> GetStudentsInClass(Guid id)
        {
            try
            {
                var clsExists = _classService.GetClassById(id) != null;
                if (!clsExists)
                {
                    return NotFound($"Không tìm thấy lớp học với Id: {id}.");
                }

                var students = _classService.GetStudentsInClass(id);
                return Ok(students);
            }
            catch (KeyNotFoundException ex)
            {
                _logger.LogWarning($"Không tìm thấy lớp học với Id: {id} khi lấy danh sách sinh viên.");
                return NotFound(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Lỗi xảy ra khi lấy danh sách sinh viên của lớp Id: {id}.");
                return StatusCode(StatusCodes.Status500InternalServerError, "Lỗi máy chủ nội bộ khi xử lý yêu cầu.");
            }
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public ActionResult<ClassDto> CreateClass([FromBody] CreateClassDto createClassDto)
        {
            try
            {
                var newClass = _classService.AddClass(createClassDto);
                return CreatedAtAction(nameof(GetClass), new { id = newClass.Id }, newClass);
            }
            catch (ArgumentException ex)
            {
                _logger.LogWarning(ex, "Lỗi khi thêm lớp học mới.");
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Lỗi không mong muốn khi tạo lớp học mới.");
                return StatusCode(StatusCodes.Status500InternalServerError, "Lỗi máy chủ nội bộ khi xử lý yêu cầu.");
            }
        }

        [HttpPost("add-students")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult AddStudentsToClass([FromBody] AddStudentsToClassDto dto)
        {
            try
            {
                bool success = _classService.AddStudentsToClass(dto, out string errorMessage);
                if (!success)
                {
                    if (errorMessage.Contains("Không tìm thấy"))
                    {
                        _logger.LogWarning($"Lỗi thêm sinh viên vào lớp {dto.ClassId}: {errorMessage}");
                        return NotFound(new { message = errorMessage });
                    }
                    else
                    {
                        _logger.LogWarning($"Lỗi thêm sinh viên vào lớp {dto.ClassId}: {errorMessage}");
                        return BadRequest(new { message = errorMessage });
                    }
                }
                _logger.LogInformation($"Thêm thành công {dto.StudentIds.Count} sinh viên vào lớp {dto.ClassId}.");
                return Ok(new { message = "Thêm sinh viên vào lớp thành công." });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Lỗi không mong muốn khi thêm sinh viên vào lớp {dto.ClassId}.");
                return StatusCode(StatusCodes.Status500InternalServerError, "Lỗi máy chủ nội bộ khi xử lý yêu cầu.");
            }
        }

        [HttpPut("{id:guid}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult UpdateClass(Guid id, [FromBody] UpdateClassDto updateClassDto)
        {
            try
            {
                var updatedClass = _classService.UpdateClass(id, updateClassDto);
                if (updatedClass == null)
                {
                    return NotFound($"Không tìm thấy lớp học với Id: {id} để cập nhật.");
                }
                return Ok(updatedClass);
            }
            catch (ArgumentException ex)
            {
                _logger.LogWarning(ex, $"Lỗi khi cập nhật lớp học Id: {id}.");
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Lỗi không mong muốn khi cập nhật lớp học Id: {id}.");
                return StatusCode(StatusCodes.Status500InternalServerError, "Lỗi máy chủ nội bộ khi xử lý yêu cầu.");
            }
        }

        [HttpDelete("{id:guid}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult DeleteClass(Guid id)
        {
            try
            {
                var deleted = _classService.DeleteClass(id);
                if (!deleted)
                {
                    return NotFound($"Không tìm thấy lớp học với Id: {id} để xoá.");
                }
                _logger.LogInformation($"Đã xoá lớp học Id: {id} và các liên kết sinh viên.");
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Lỗi không mong muốn khi xoá lớp học Id: {id}.");
                return StatusCode(StatusCodes.Status500InternalServerError, "Lỗi máy chủ nội bộ khi xử lý yêu cầu.");
            }
        }
    }
}