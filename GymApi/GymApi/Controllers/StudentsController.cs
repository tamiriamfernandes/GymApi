using GymApi.Application.DTOs;
using GymApi.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace GymApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class StudentsController : ControllerBase
{
    private readonly IStudentService _studentService;

    public StudentsController(IStudentService studentService)
    {
        _studentService = studentService;
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateStudentDto dto)
    {
        var id = await _studentService.CreateAsync(dto);
        return Ok(new { id });
    }

    [HttpGet]
    public async Task<IActionResult> GetPaged(
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 10)
    {
        var result = await _studentService.GetPagedAsync(page, pageSize);

        return Ok(new
        {
            data = result.Items,
            total = result.TotalCount,
            page,
            pageSize
        });
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var student = await _studentService.GetByIdAsync(id);

        if (student is null)
            return NotFound();

        return Ok(student);
    }
}
