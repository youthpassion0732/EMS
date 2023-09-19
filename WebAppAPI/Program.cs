using DAL.Database;
using DAL.Interfaces;
using DAL.Repositries;
using Microsoft.EntityFrameworkCore;

namespace WebAPI;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.

        builder.Services.AddControllers();
        builder.Services.AddDbContext<DBContext>(item =>
        {
            item.UseInMemoryDatabase(databaseName: "EmployeeDB");
            item.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
        });

        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddTransient(typeof(IGenericService<>), typeof(GenericService<>));
        builder.Services.AddTransient<IEmployeeService, EmployeeService>();
        builder.Services.AddSwaggerGen();

        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseHttpsRedirection();
        app.UseAuthorization();
        app.MapControllers();
        app.Run();
    }
}