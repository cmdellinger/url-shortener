using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Infrastructure.Identity;
using Core.Interfaces;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

// Framework Services
builder.Services.AddControllers();

// Database
builder.Services.AddDbContext<AppDbContext>(opt =>
{
    opt.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
});

// Identity
builder.Services.AddIdentityServices(builder.Configuration);

// CORS
builder.Services.AddCors(opt =>
{
    opt.AddPolicy("CorsPolicy", policy =>
    {
        policy.AllowAnyHeader().AllowAnyMethod().WithOrigins(
            "http://localhost:4200"
        );
    });
});

// App services / repositories
builder.Services.AddScoped<ITokenService, TokenService>();

var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseHttpsRedirection();
app.UseCors("CorsPolicy");
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.Run();
