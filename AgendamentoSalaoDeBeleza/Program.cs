using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Domain.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddDbContext<SalonContext>(options =>
    options.UseMySql(builder.Configuration.GetConnectionString("DefaultConnection"),
    new MySqlServerVersion(new Version(8, 0, 21))));

builder.Services.AddScoped<AppointmentService>();
builder.Services.AddScoped<EmployeeService>();
builder.Services.AddScoped<ServicesService>();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "AgendamentoSalaoDeBeleza API",
        Version = "v1",
        Description = "API para agendamento de salão de beleza",
        Contact = new OpenApiContact
        {
            Name = "Mariana Rodrigues",
            Email = "mariana12v@gmail.com",
        }
    });
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
