using System.Text;
using ActivityTracker.Application.Extensions;
using ActivityTracker.Application.Helpers;
using ActivityTracker.Domain.Entities;
using ActivityTracker.Infrastructure.Data;
using ActivityTracker.Infrastructure.Extensions;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddApplication();
builder.Services.AddInfrastructure(builder.Configuration);

var jwtSection = builder.Configuration.GetSection("Jwt");
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer           = true,
            ValidateAudience         = true,
            ValidateLifetime         = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer              = jwtSection["Issuer"],
            ValidAudience            = jwtSection["Audience"],
            IssuerSigningKey         = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(jwtSection["Key"]!))
        };
    });

builder.Services.AddCors(options =>
    options.AddPolicy("AngularDev", policy =>
        policy.WithOrigins(
                "http://localhost:4200",
                "https://jestr-activity-tracking-app.vercel.app")
              .AllowAnyHeader()
              .AllowAnyMethod()));

var app = builder.Build();

// Apply pending migrations and seed global users on startup
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    db.Database.Migrate();

    if (!db.AppUsers.Any(u => u.Username == "admin"))
    {
        db.AppUsers.Add(new AppUser
        {
            Username     = "admin",
            PasswordHash = PasswordHelper.Hash("Admin123!"),
            Role         = "admin",
            CompanyId    = null,
            IsActive     = true,
            CreatedAt    = DateTime.UtcNow
        });
    }

    if (!db.AppUsers.Any(u => u.Username == "viewer"))
    {
        db.AppUsers.Add(new AppUser
        {
            Username     = "viewer",
            PasswordHash = PasswordHelper.Hash("Viewer123!"),
            Role         = "viewer",
            CompanyId    = null,
            IsActive     = true,
            CreatedAt    = DateTime.UtcNow
        });
    }

    db.SaveChanges();
}

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("AngularDev");
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.Run();
