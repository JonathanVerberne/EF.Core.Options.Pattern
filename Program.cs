using EF.Core.Options.Pattern.Entities;
using EF.Core.Options.Pattern.Options;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.ConfigureOptions<DatabaseOptionsSetup>();

builder.Services.AddDbContext<DatabaseContext>(
    (serviceProvider, dbContextOptionsBuilder) =>
    {
        var databaseOptions = serviceProvider.GetService<IOptions<DatabaseOptions>>()!.Value;
        
        dbContextOptionsBuilder.UseNpgsql(databaseOptions.ConnectionString, npgsqlOptionsAction =>
        {
            npgsqlOptionsAction.EnableRetryOnFailure(databaseOptions.MaxRetryCount);
            npgsqlOptionsAction.CommandTimeout(databaseOptions.CommandTimeout);
        });

        dbContextOptionsBuilder.EnableDetailedErrors(databaseOptions.EnableDetailedErrors);
        dbContextOptionsBuilder.EnableSensitiveDataLogging(databaseOptions.EnableSensitiveDataLogging);
    });

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapGet("employees/{employee_id:int}", async (int employee_id, DatabaseContext dbContext) =>
{
    var employee = await dbContext
            .Set<Employee>()
            .AsNoTracking()
            .FirstOrDefaultAsync(e => e.EmployeeId == employee_id);

    if (employee == null)
    {
        return Results.NotFound($"The employee with Id '{employee_id}' was not found.");
    }

    return Results.Ok(employee);
});

app.UseAuthorization();

app.MapControllers();

app.Run();
