using System;
using Microsoft.AspNetCore.Mvc;
using StudentManagementAPI.DTOs;
using StudentManagementAPI.Services.Interfaces;

namespace StudentManagementAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class StudentsController : ControllerBase
    {
        private readonly IStudentService _studentService;
        private readonly ILogger<StudentsController> _logger;

        public StudentsController(IStudentService studentService, ILogger<StudentsController> logger)
        {
            _studentService = studentService;
            _logger = logger;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public ActionResult<IEnumerable<StudentDto>> GetStudents()
        {
            try
            {
                var students = _studentService.GetAllStudents();
                return Ok(students);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while retrieving students.");
                return StatusCode(StatusCodes.Status500InternalServerError, "Internal server error.");
            }
        }

        [HttpGet("{id:guid}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<StudentDto> GetStudent(Guid id)
        {
            try
            {
                var student = _studentService.GetStudentById(id);
                if (student == null)
                {
                    return NotFound($"Student with Id: {id} not found.");
                }
                return Ok(student);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error occurred while retrieving student with Id: {id}.");
                return StatusCode(StatusCodes.Status500InternalServerError, "Internal server error.");
            }
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public ActionResult<StudentDto> CreateStudent([FromBody] CreateStudentDto createStudentDto)
        {
            try
            {
                var newStudent = _studentService.AddStudent(createStudentDto);
                return CreatedAtAction(nameof(GetStudent), new { id = newStudent.Id }, newStudent);
            }
            catch (ArgumentException ex)
            {
                _logger.LogWarning(ex, "Error occurred while adding a new student.");
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error occurred while creating a new student.");
                return StatusCode(StatusCodes.Status500InternalServerError, "Internal server error.");
            }
        }

        [HttpPut("{id:guid}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult UpdateStudent(Guid id, [FromBody] UpdateStudentDto updateStudentDto)
        {
            try
            {
                var updatedStudent = _studentService.UpdateStudent(id, updateStudentDto);
                if (updatedStudent == null)
                {
                    return NotFound($"Student with Id: {id} not found for update.");
                }
                return Ok(updatedStudent);
            }
            catch (ArgumentException ex)
            {
                _logger.LogWarning(ex, $"Error occurred while updating student Id: {id}.");
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Unexpected error occurred while updating student Id: {id}.");
                return StatusCode(StatusCodes.Status500InternalServerError, "Internal server error.");
            }
        }

        [HttpDelete("{id:guid}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult DeleteStudent(Guid id)
        {
            try
            {
                var deleted = _studentService.DeleteStudent(id);
                if (!deleted)
                {
                    return NotFound($"Student with Id: {id} not found for deletion.");
                }
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Unexpected error occurred while deleting student Id: {id}.");
                return StatusCode(StatusCodes.Status500InternalServerError, "Internal server error.");
            }
        }
    }
}