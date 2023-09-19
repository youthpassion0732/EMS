using DomainEntities;
using DTO;
using System.Linq.Expressions;

namespace DAL.Interfaces;

public interface IEmployeeService
{

    /// <summary>
    /// Add new employee
    /// </summary>
    /// <param name="dto"></param>
    /// <returns>EmployeeDto</returns>
    Task<EmployeeDto> AddAsync(EmployeeDto dto);

    /// <summary>
    /// Update an existing employee
    /// </summary>
    /// <param name="dto"></param>
    /// <returns>EmployeeDto</returns>
    Task<EmployeeDto> UpdateAsync(EmployeeDto dto);

    /// <summary>
    /// Delete employee by id
    /// </summary>
    /// <param name="id"></param>
    /// <returns>bool</returns>
    Task<bool> DeleteAsync(int id);

    /// <summary>
    /// Get employee
    /// </summary>
    /// <param name="predicate"></param>
    /// <returns>EmployeeDto</returns>
    Task<EmployeeDto> GetAsync(Expression<Func<Employee, bool>> predicate);

    /// <summary>
    /// Get employee by id
    /// </summary>
    /// <param name="id"></param>
    /// <returns>EmployeeDto</returns>
    Task<EmployeeDto> GetByIdAsync(int id);

    /// <summary>
    /// Get employees list
    /// </summary>
    /// <param name="requestDto"></param>
    /// <returns>PaginationResponseDto</returns>
    Task<PaginationResponseDto<Employee>> PageListAsync(PaginationRequestDto requestDto);

    /// <summary>
    /// Check if employee exists or not
    /// </summary>
    /// <param name="dto"></param>
    /// <returns>bool</returns>
    Task<bool> IsExists(EmployeeDto dto);
}
