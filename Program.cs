using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.Text.Json;
using ValuecornAPI.Data;
using System.Data;
using Microsoft.Data.SqlClient;


var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Register IDbConnection for Dapper (scoped per request)
builder.Services.AddScoped<IDbConnection>(sp =>
{
    var config = sp.GetRequiredService<IConfiguration>();
    var conn = new SqlConnection(config.GetConnectionString("DefaultConnection"));
    conn.Open(); // optional, ensures it's ready before use
    return conn;
});

builder.Services.AddScoped<IUserRepository, UserRepository>(); // ✅ this is required
// Register Repositories
builder.Services.AddScoped<ICompanyRepository, CompanyRepository>();

builder.Services.AddScoped<IAddressRepository, AddressRepository>();

// CORS (adjust origin for your Angular app if needed)
builder.Services.AddCors(opt =>
{
    opt.AddPolicy("vc-cors", p => p
        .AllowAnyHeader()
        .AllowAnyMethod()
        .AllowAnyOrigin()); // or .WithOrigins("http://localhost:4200")
});

// JWT
var jwtKey = builder.Configuration["Jwt:Key"]!;
builder.Services
    .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new()
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidAudience = builder.Configuration["Jwt:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey))
        };
    });

builder.Services.AddControllers()
    .AddJsonOptions(opts =>
    {
        opts.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
    });

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("vc-cors");
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
app.Run();
