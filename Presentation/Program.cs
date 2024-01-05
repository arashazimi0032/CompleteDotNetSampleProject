using Application;
using Application.Identity;
using Carter;
using Domain.ApplicationUsers;
using infrastructure;
using infrastructure.Persistence;
using Microsoft.AspNetCore.Identity;
using Presentation;
using Microsoft.EntityFrameworkCore;
using Presentation.Middlewares;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
});

builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddDefaultTokenProviders()
    .AddUserManager<CustomUserManager<ApplicationUser>>();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGenWhitAuthorize();

builder.Services
    .AddApplication()
    .AddInfrastructure()
    .AddPresentation(builder.Configuration);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "CompleteDotNetSampleProject v1");
    });
}

app.UseHttpsRedirection();

app.UseRouting();

app.UseAuthentication();

app.UseAuthorization();

app.UseMiddleware<GlobalExceptionHandlingMiddleware>();

app.MapControllers();

app.MapCarter();

app.Run();
