using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using semaine11_serveur.Data;
var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<semaine11_serveurContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("semaine11_serveurContext") ?? throw new InvalidOperationException("Connection string 'semaine11_serveurContext' not found.")));

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddCors(options =>
{
    options.AddPolicy("Allow all", policy =>
    {
        policy.AllowAnyOrigin();
        policy.AllowAnyMethod();
        policy.AllowAnyHeader();
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("Allow all");

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
