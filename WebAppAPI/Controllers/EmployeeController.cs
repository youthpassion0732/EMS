using DAL.Interfaces;
using DTO;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers;

[Route("api/[controller]")]
[ApiController]
public class EmployeeController : ControllerBase
{
    private readonly IEmployeeService _employeeService;
    private readonly ILogger<EmployeeController> _logger;

    public EmployeeController(IEmployeeService employeeService, ILogger<EmployeeController> logger)
    {
        _employeeService = employeeService;
        _logger = logger;
    }

    [HttpPost("GetAll")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult> GetAllAsync(PaginationRequestDto requestDto)
    {
        try
        {
            return Ok(await _employeeService.PageListAsync(requestDto));
        }
        catch (Exception ex)
        {
            string error = $"Error occurred in fetching employees. Error : {ex.Message}";
            _logger.LogError(error);
            return BadRequest(error);
        }
    }

    [HttpPost("Add")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<EmployeeDto>> AddAsync(EmployeeDto employee)
    {
        try
        {
            bool isExists = await _employeeService.IsExists(employee);
            if (isExists)
                return Conflict("Employee Already Exist");

            return Ok(await _employeeService.AddAsync(employee));
        }
        catch (Exception ex)
        {
            string error = $"Error occurred in adding employee info. Error : {ex.Message}";
            _logger.LogError(error);
            return BadRequest(error);
        }
    }

    [HttpPut("Update")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<EmployeeDto>> UpdateAsync(EmployeeDto employee)
    {
        try
        {
            bool isExists = await _employeeService.IsExists(employee);
            if (isExists)
                return Conflict("Employee Already Exist");

            EmployeeDto dto = await _employeeService.GetByIdAsync(employee.Id);
            if (dto is null)
                return NotFound("Employee not found");

            return Ok(await _employeeService.UpdateAsync(employee));
        }
        catch (Exception ex)
        {
            string error = $"Error occurred in updating employee info. Error : {ex.Message}";
            _logger.LogError(error);
            return BadRequest(error);
        }
    }

    [HttpDelete]
    [Route("{Id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> DeleteAsync(int id)
    {
        try
        {
            var success = await _employeeService.DeleteAsync(id);

            if (!success)
                return NotFound("Employee not found");

            return Ok();
        }
        catch (Exception ex)
        {
            string error = $"Error occurred in deleting employee. Error : {ex.Message}";
            _logger.LogError(error);
            return BadRequest(error);
        }
    }

    [HttpGet]
    [Route("{Id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> GetByIdAsync(int id)
    {
        try
        {
            var employee = await _employeeService.GetByIdAsync(id);
            return Ok(employee);
        }
        catch (Exception ex)
        {
            string error = $"Error occurred in fetching employee info. Error : {ex.Message}";
            _logger.LogError(error);
            return BadRequest(error);
        }
    }
}
