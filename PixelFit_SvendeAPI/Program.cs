using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.IdentityModel.Tokens;
using PixelFit_SvendeAPI.Data;
using PixelFit_SvendeAPI.Models;
using PixelFit_SvendeAPI.Repositories;
using PixelFit_SvendeAPI.Repositories.Interfaces;
using PixelFit_SvendeAPI.Services;

var builder = WebApplication.CreateBuilder(args);

// Tilføjer Controllers til REST API'et
builder.Services.AddControllers();

// Tilføjer Swagger til test af API'et
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Connection string for SQL Server database
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// ASP.NET Identity bruges til brugere og password hashing
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
        // Fortæller API'et hvordan JWT-tokenet skal valideres
        options.TokenValidationParameters =
            new TokenValidationParameters
            {
                // Kontrollerer hvem der har lavet tokenet
                ValidateIssuer = true,

                // Kontrollerer hvem tokenet er lavet til
                ValidateAudience = true,

                // Kontrollerer at tokenet ikke er udløbet
                ValidateLifetime = true,

                // Kontrollerer JWT-signaturen
                ValidateIssuerSigningKey = true,

                // Skal matche værdien i appsettings.json
                ValidIssuer =
                    builder.Configuration["Jwt:Issuer"],

                // Skal matche værdien i appsettings.json
                ValidAudience =
                    builder.Configuration["Jwt:Audience"],

                // Den hemmelige nøgle bruges til at validere signaturen
                IssuerSigningKey =
                    new SymmetricSecurityKey(
                        Encoding.UTF8.GetBytes(
                            builder.Configuration["Jwt:Key"]!
                        )
                    )
            };
    });


// Dependency Injection til bruger repository
builder.Services.AddScoped<IUserRepository, UserRepository>();

// Dependency Injection til bruger service
builder.Services.AddScoped<IUserService, UserService>();

// Configure forwarded headers so app sees correct scheme and remote IPs when behind a reverse proxy
builder.Services.Configure<ForwardedHeadersOptions>(options =>
{
    options.ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto;
    // If you run multiple front proxies, add KnownProxies or KnownNetworks.
    options.KnownNetworks.Clear();
    options.KnownProxies.Clear();
});

// Registrerer vores service som laver JWT tokens
builder.Services.AddScoped<JwtService>();
var app = builder.Build();


// Det her gør swagger tilgængeligt i produktion, så kan man tilgå API'et i en browser.
app.UseSwagger();
app.UseSwaggerUI();

// Health check endpoint
app.MapGet("/health", () => Results.Ok("Healthy"));

//test3

// Must run before calls that rely on scheme/IP
app.UseForwardedHeaders();

// Til https senere.
//app.UseHttpsRedirection();


// Tjekker hvem brugeren er ud fra fx. JWT-token
app.UseAuthentication();

// Tjekker hvad brugeren har adgang til
app.UseAuthorization();


// Aktiverer vores Controllers
app.MapControllers();


// Starter API'et
app.Run();