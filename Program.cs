using Microsoft.EntityFrameworkCore;
using BookApi.Data;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using DotNetEnv;
using System.Text;
using BookApi.Helpers;

var builder = WebApplication.CreateBuilder(args);

// Load environment variables
Env.Load();

var key = Encoding.ASCII.GetBytes(Env.GetString("JWT_KEY") ?? "VotreCléSecrèteTrèsSécurisée");

// Add services to the container.
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.RequireHttpsMetadata = false;
    options.SaveToken = true;
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(key),
        ValidateIssuer = false, // No issuer
        ValidateAudience = false, // No audience
        ClockSkew = TimeSpan.Zero
    };
});

builder.Services.AddControllers(options =>
{
    options.Filters.Add<UserIdFilter>();
});

builder.Services.AddHttpClient();

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")).UseSnakeCaseNamingConvention());

// Register the LoggingHelper
builder.Services.AddSingleton<LoggingHelper>();

// Register the EmailHelper
builder.Services.AddTransient<EmailHelper>();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
