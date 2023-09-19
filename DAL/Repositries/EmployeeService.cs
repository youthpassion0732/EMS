using DTO;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using DomainEntities;
using DAL.Interfaces;

namespace DAL.Repositries;

public class EmployeeService : IEmployeeService
{
    private readonly IGenericService<Employee> _repository;

    public EmployeeService(IGenericService<Employee> employeeRepo)
    {
        _repository = employeeRepo;
    }

    #region public methods

    public async Task<EmployeeDto> AddAsync(EmployeeDto dto)
    {
        try
        {
            var employee = await _repository.AddWithSaveAsync(SetEntity(dto));
            return SetDto(employee);
        }
        catch (Exception)
        {
            throw;
        }
    }

    public async Task<EmployeeDto> UpdateAsync(EmployeeDto dto)
    {
        try
        {
            var employee = await _repository.UpdateWithSaveAsync(SetEntity(dto));
            return SetDto(employee);
        }
        catch (Exception)
        {
            throw;
        }
    }

    public async Task<bool> IsExists(EmployeeDto dto)
    {
        try
        {
            return await _repository.IsAny(x => !x.Id.Equals(dto.Id) && dto.Email.ToLower().Trim().Equals(x.Email.ToLower()));
        }
        catch (Exception)
        {
            throw;
        }
    }

    public async Task<bool> DeleteAsync(int id)
    {
        try
        {
            var entity = await _repository.GetAsync(id);
            if (entity is null)
                return false;

            await _repository.DeleteWithSaveAsync(entity);
            return true;
        }
        catch (Exception)
        {
            throw;
        }
    }

    public async Task<EmployeeDto> GetAsync(Expression<Func<Employee, bool>> predicate)
    {
        try
        {
            var entity = await _repository.GetAsync(predicate);
            return SetDto(entity);
        }
        catch (Exception)
        {
            throw;
        }
    }

    public async Task<EmployeeDto> GetByIdAsync(int id)
    {
        try
        {
            var entity = await _repository.GetAsync(id);
            if (entity == null) return null;
            return SetDto(entity);
        }
        catch (Exception)
        {
            throw;
        }
    }

    public async Task<PaginationResponseDto<Employee>> PageListAsync(PaginationRequestDto requestDto)
    {
        try
        {
            var query = _repository.GetAll();

            if (!string.IsNullOrWhiteSpace(requestDto.Search))
                query = query.Where(x => x.Name.ToLower().Contains(requestDto.Search.ToLower().Trim()) || x.Department.ToLower().Contains(requestDto.Search.ToLower().Trim()) || x.Email.ToLower().Contains(requestDto.Search.ToLower().Trim()));

            query = query.OrderByDescending(x => x.Id);

            int total = await query.CountAsync();
            int numberOfPages = (int)Math.Ceiling((double)total / requestDto.PageSize);
            int startValuesCount = (total > 0) ? ((requestDto.PageNo - 1) * requestDto.PageSize) + 1 : total;
            int endValuesCount = (requestDto.PageNo != numberOfPages && total > 0) ? (requestDto.PageNo * requestDto.PageSize) : total;

            if (requestDto.PageNo > 0)
            {
                int skipRecords = requestDto.PageNo <= 1 ? 0 : (requestDto.PageNo - 1) * requestDto.PageSize;
                query = query.Skip(skipRecords).Take(requestDto.PageSize);
            }

            return new PaginationResponseDto<Employee>
            {
                PageIndex = requestDto.PageNo,
                TotalRecords = total,
                StartCount = startValuesCount,
                EndCount = endValuesCount,
                NoOfPages = numberOfPages,
                Data = await query.ToListAsync()
            };
        }
        catch
        {
            return new PaginationResponseDto<Employee>();
        }
    }

    #endregion

    #region Private Methods

    private static EmployeeDto SetDto(Employee dbEmployee) => new()
    {
        Id = dbEmployee.Id,
        Name = dbEmployee.Name,
        DateOfBirth = dbEmployee.DateOfBirth,
        Department = dbEmployee.Department,
        Email = dbEmployee.Email
    };

    private static Employee SetEntity(EmployeeDto employeeDTO) => new()
    {
        Id = employeeDTO.Id,
        Name = employeeDTO.Name,
        DateOfBirth = employeeDTO.DateOfBirth,
        Department = employeeDTO.Department,
        Email = employeeDTO.Email
    };

    #endregion
}

