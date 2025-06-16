using DotNetEnv;
using Microsoft.IdentityModel.Tokens;
using FluentValidation;
using FluentValidation.AspNetCore;
using System.Text;
using Microsoft.OpenApi.Models;



using FinFlowApi.Services;
using FinFlowApi.DTOs;
using FinFlowApi.Validators;
using FinFlowApi.Repositories;
using FinFlowApi.Middleware;

var builder = WebApplication.CreateBuilder(args);

// Load .env file
Env.Load();

// Oracle connection string setup (if needed)
string connectionString =
    $"User Id={Environment.GetEnvironmentVariable("DB_USER")};" +
    $"Password={Environment.GetEnvironmentVariable("DB_PASSWORD")};" +
    $"Data Source=(DESCRIPTION=(ADDRESS=(PROTOCOL=TCP)(HOST={Environment.GetEnvironmentVariable("DB_HOST")})(PORT={Environment.GetEnvironmentVariable("DB_PORT")}))" +
    $"(CONNECT_DATA=(SERVICE_NAME={Environment.GetEnvironmentVariable("DB_NAME")})));";

var jwtSecret = Environment.GetEnvironmentVariable("JWT_SECRET");


builder.Services.AddSingleton(connectionString);


// JWT
var jwtSettings = new JwtSettings
{
    Secret = Environment.GetEnvironmentVariable("JWT_SECRET")!,
    ExpiryMinutes = 60,
    Issuer = "FinFlowApi",
    Audience = "FinFlowApiUsers"
};

builder.Services.AddSingleton(jwtSettings);
builder.Services.AddAuthentication("Bearer")
    .AddJwtBearer("Bearer", options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidIssuer = jwtSettings.Issuer,
            ValidateAudience = true,
            ValidAudience = jwtSettings.Audience,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(jwtSettings.Secret)!)
        };
    });

builder.Services.AddAuthorization();

// Repo
builder.Services.AddScoped<ICardRepository, CardRepository>();
builder.Services.AddScoped<IUserRepository, UserRepository>();

// Add services to the container
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo { Title = "FinFlow API", Version = "v1" });

    // 🔐 Add JWT Auth to Swagger
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "Enter 'Bearer' [space] and then your valid token.\nExample: Bearer eyJhbGciOiJIUzI1NiIsInR..."
    });

    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = "Bearer" }
            },
            Array.Empty<string>()
        }
    });
});
//  Dependency Injection
builder.Services.AddScoped<ICardService, CardService>();
builder.Services.AddScoped<IUserService, UserService>();


// FluentValidation
builder.Services.AddFluentValidationAutoValidation();
builder.Services.AddValidatorsFromAssemblyContaining<OtpDtoRequestValidator>();
builder.Services.AddValidatorsFromAssemblyContaining<VerifyDtoRequestValidator>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
