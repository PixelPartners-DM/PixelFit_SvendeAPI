using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

using PixelFit_SvendeAPI.Data;
using PixelFit_SvendeAPI.Models;

using PixelFit_SvendeAPI.Repositories;
using PixelFit_SvendeAPI.Repositories.Interfaces;

using PixelFit_SvendeAPI.Services;
using PixelFit_SvendeAPI.Services.Interfaces;


var builder = WebApplication.CreateBuilder(args);



// Tilføjer Controllers til REST API'et
builder.Services.AddControllers();

// Tilføjer Swagger så API'et kan testes
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();



// Forbinder API'et til SQL Server
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection")
    )
);



// Identity bruges til brugerhåndtering og password hashing
builder.Services.AddIdentity<User, IdentityRole<int>>(options =>
{
    // Email skal være unik
    options.User.RequireUniqueEmail = true;

    // Password regler
    options.Password.RequiredLength = 6;
    options.Password.RequireDigit = false;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequireUppercase = false;
})
.AddEntityFrameworkStores<ApplicationDbContext>()
.AddDefaultTokenProviders();



// Registrerer JWT authentication
builder.Services
    .AddAuthentication(options =>
    {
        // JWT bruges som standard authentication
        options.DefaultAuthenticateScheme =
            JwtBearerDefaults.AuthenticationScheme;

        options.DefaultChallengeScheme =
            JwtBearerDefaults.AuthenticationScheme;
    })
    .AddJwtBearer(options =>
    {
        // Fortæller API'et hvordan JWT-token skal valideres
        options.TokenValidationParameters =
            new TokenValidationParameters
            {
                // Kontrollerer hvem der har udstedt tokenet
                ValidateIssuer = true,

                // Kontrollerer hvem tokenet er beregnet til
                ValidateAudience = true,

                // Kontrollerer om tokenet er udløbet
                ValidateLifetime = true,

                // Kontrollerer JWT-signaturen
                ValidateIssuerSigningKey = true,

                // Skal matche Jwt:Issuer i appsettings.json
                ValidIssuer =
                    builder.Configuration["Jwt:Issuer"],

                // Skal matche Jwt:Audience i appsettings.json
                ValidAudience =
                    builder.Configuration["Jwt:Audience"],

                // Den hemmelige nøgle bruges til at
                // kontrollere tokenets signatur
                IssuerSigningKey =
                    new SymmetricSecurityKey(
                        Encoding.UTF8.GetBytes(
                            builder.Configuration["Jwt:Key"]!
                        )
                    )
            };
    });



// Repository til brugerdata
builder.Services.AddScoped<IUserRepository, UserRepository>();

// Service til brugerhåndtering
builder.Services.AddScoped<IUserService, UserService>();



// Repository til træningsprogrammer
builder.Services.AddScoped<
    ITrainingProgramRepository,
    TrainingProgramRepository>();

// Service til træningsprogrammer
builder.Services.AddScoped<
    ITrainingProgramService,
    TrainingProgramService>();



// Service som laver JWT tokens ved login
builder.Services.AddScoped<JwtService>();




// Bruges fordi API'et kører bag en reverse proxy på serveren
builder.Services.Configure<ForwardedHeadersOptions>(options =>
{
    options.ForwardedHeaders =
        ForwardedHeaders.XForwardedFor |
        ForwardedHeaders.XForwardedProto;

    options.KnownNetworks.Clear();
    options.KnownProxies.Clear();
});


var app = builder.Build();



// Swagger er tilgængeligt så API'et kan testes i browseren
app.UseSwagger();
app.UseSwaggerUI();




// Bruges til at kontrollere om API'et kører
app.MapGet("/health", () => Results.Ok("Healthy"));


// Skal ligge før authentication,
// så API'et ser de korrekte proxy oplysninger
app.UseForwardedHeaders();


// HTTPS aktiveres når serveren er sat op til det
// app.UseHttpsRedirection();


// Finder ud af hvem brugeren er ud fra JWT
app.UseAuthentication();

// Kontrollerer hvad brugeren har adgang til
app.UseAuthorization();


// Aktiverer alle Controllers
app.MapControllers();


// Starter API'et
app.Run();