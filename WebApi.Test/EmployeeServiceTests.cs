using DAL.Interfaces;
using DAL.Repositries;
using DomainEntities;
using DTO;
using Moq;
using System.Linq.Expressions;
using Xunit;

namespace WebApi.Test;

public class _employeeServiceTests
{
    private readonly Mock<IGenericService<Employee>> _employeeRepoMock;
    private readonly IEmployeeService _employeeService;

    public _employeeServiceTests()
    {
        _employeeRepoMock = new Mock<IGenericService<Employee>>();
        _employeeService = new EmployeeService(_employeeRepoMock.Object);
    }

    [Fact]
    public async Task AddAsync_ShouldAddEmployee()
    {
        // Arrange
        EmployeeDto employeeDto = GetEmployeeDto();
        var employeeEntity = GetEmployeeEntity();

        _employeeRepoMock
            .Setup(repo => repo.AddWithSaveAsync(It.IsAny<Employee>()))
            .ReturnsAsync(employeeEntity);

        // Act
        var result = await _employeeService.AddAsync(employeeDto);

        // Assert
        Assert.NotNull(result);
        Assert.True(result.Id > 0);
    }

    [Fact]
    public async Task UpdateAsync_ShouldUpdateEmployee()
    {
        // Arrange
        EmployeeDto employeeDto = GetEmployeeDto();
        var employeeEntity = GetEmployeeEntity();

        _employeeRepoMock
            .Setup(repo => repo.UpdateWithSaveAsync(It.IsAny<Employee>()))
            .ReturnsAsync(employeeEntity);

        // Act
        var result = await _employeeService.UpdateAsync(employeeDto);

        // Assert
        Assert.NotNull(result);
    }

    [Fact]
    public async Task IsExists_ShouldCheckIfEmployeeExists()
    {
        // Arrange
        EmployeeDto employeeDto = GetEmployeeDto();
        var employeeEntity = GetEmployeeEntity();

        _employeeRepoMock
            .Setup(repo => repo.IsAny(It.IsAny<Expression<Func<Employee, bool>>>()))
            .ReturnsAsync(true);

        // Act
        var result = await _employeeService.IsExists(employeeDto);

        // Assert
        Assert.True(result);
    }

    [Fact]
    public async Task DeleteAsync_ShouldDeleteEmployee()
    {
        // Arrange
        EmployeeDto employeeDto = GetEmployeeDto();
        var employeeEntity = GetEmployeeEntity();

        _employeeRepoMock
            .Setup(repo => repo.GetAsync(It.IsAny<int>()))
            .ReturnsAsync(employeeEntity);

        // Act
        var success = await _employeeService.DeleteAsync(employeeDto.Id);

        // Assert
        Assert.True(success);
    }

    [Fact]
    public async Task GetAsync_ShouldGetEmployee()
    {
        // Arrange
        EmployeeDto employeeDto = GetEmployeeDto();
        var employeeEntity = GetEmployeeEntity();

        _employeeRepoMock
            .Setup(repo => repo.GetAsync(It.IsAny<Expression<Func<Employee, bool>>>()))
            .ReturnsAsync(employeeEntity);

        // Act
        var result = await _employeeService.GetAsync(x => x.Id == employeeDto.Id);

        // Assert
        Assert.NotNull(result);
    }

    [Fact]
    public async Task GetByIdAsync_ShouldGetEmployeeById()
    {
        // Arrange
        EmployeeDto employeeDto = GetEmployeeDto();
        var employeeEntity = GetEmployeeEntity();

        _employeeRepoMock
            .Setup(repo => repo.GetAsync(It.IsAny<int>()))
            .ReturnsAsync(employeeEntity);

        // Act
        var result = await _employeeService.GetByIdAsync(employeeDto.Id);

        // Assert
        Assert.NotNull(result);
    }

    private static Employee GetEmployeeEntity() => new()
    {
        Id = 1,
        Name = "Test User",
        Department = "Test Department",
        Email = "Test@gmail.com",
        DateOfBirth = DateTime.Now,
    };

    private static EmployeeDto GetEmployeeDto() => new()
    {
        Id = 1,
        Name = "Test User",
        Department = "Test Department",
        Email = "Test@gmail.com",
        DateOfBirth = DateTime.Now,
    };
}