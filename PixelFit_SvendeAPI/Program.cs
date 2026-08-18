using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.Identity;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Serilog;
using Serilog.Sinks.SystemConsole;
using Serilog.AspNetCore;
using Microsoft.Extensions.DependencyInjection.Extensions;

using System.Text;
using System.Text.Json.Serialization;

using PixelFit_SvendeAPI.Data;
using PixelFit_SvendeAPI.Models;

using PixelFit_SvendeAPI.Repositories;
using PixelFit_SvendeAPI.Repositories.Interfaces;

using PixelFit_SvendeAPI.Services;
using PixelFit_SvendeAPI.Services.Interfaces;

Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Information()
    .WriteTo.Console()
    .WriteTo.File("/var/log/pixelfit/api.log", rollingInterval: RollingInterval.Day)
    .Enrich.FromLogContext()
    .CreateLogger();

var builder = WebApplication.CreateBuilder(args);
builder.Logging.ClearProviders();
builder.Logging.AddSerilog();

Log.Information("Serilog is working!");

// Tilføjer Controllers til REST API'et
// JsonStringEnumConverter gør at enums kan sendes som tekst.
// Fx. "Mandag" i stedet for 0.
builder.Services
    .AddControllers(options =>{})
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.Converters.Add(
            new JsonStringEnumConverter()
        );
    });

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen(options =>
{
    options.AddSecurityDefinition(
        "Bearer",
        new OpenApiSecurityScheme
        {
            Name = "Authorization",

            Type = SecuritySchemeType.Http,

            Scheme = "bearer",

            BearerFormat = "JWT",

            In = ParameterLocation.Header,

            Description = "Indsæt dit JWT-token her."
        }
    );

    options.AddSecurityRequirement(
        new OpenApiSecurityRequirement
        {
            {
                new OpenApiSecurityScheme
                {
                    Reference = new OpenApiReference
                    {
                        Type = ReferenceType.SecurityScheme,
                        Id = "Bearer"
                    }
                },

                Array.Empty<string>()
            }
        }
    );
});

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString(
            "DefaultConnection"
        )
    )
);

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

builder.Services
    .AddAuthentication(options =>
    {
        options.DefaultAuthenticateScheme =
            JwtBearerDefaults.AuthenticationScheme;

        options.DefaultChallengeScheme =
            JwtBearerDefaults.AuthenticationScheme;
    })
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters =
            new TokenValidationParameters
            {
                ValidateIssuer = true,

                ValidateAudience = true,

                ValidateLifetime = true,

                ValidateIssuerSigningKey = true,

                ValidIssuer =
                    builder.Configuration["Jwt:Issuer"],

                ValidAudience =
                    builder.Configuration["Jwt:Audience"],

                IssuerSigningKey =
                    new SymmetricSecurityKey(
                        Encoding.UTF8.GetBytes(
                            builder.Configuration["Jwt:Key"]!
                        )
                    )
            };
    });

builder.Services.AddScoped<
    IUserRepository,
    UserRepository>();

builder.Services.AddScoped<
    IUserService,
    UserService>();

builder.Services.AddScoped<
    ITrainingProgramRepository,
    TrainingProgramRepository>();

builder.Services.AddScoped<
    ITrainingProgramService,
    TrainingProgramService>();

builder.Services.AddScoped<
    ITrainingDayRepository,
    TrainingDayRepository>();

builder.Services.AddScoped<
    ITrainingDayService,
    TrainingDayService>();

builder.Services.AddScoped<
    IMuscleGroupRepository,
    MuscleGroupRepository>();

builder.Services.AddScoped<
    IMuscleGroupService,
    MuscleGroupService>();

// Repository til øvelser
builder.Services.AddScoped<
    IExerciseRepository,
    ExerciseRepository>();

// Service til øvelser
builder.Services.AddScoped<
    IExerciseService,
    ExerciseService>();

// Repository til øvelser valgt på en træningsdag
builder.Services.AddScoped<
    ITrainingDayExerciseRepository,
    TrainingDayExerciseRepository>();

// Service til øvelser valgt på en træningsdag
builder.Services.AddScoped<
    ITrainingDayExerciseService,
    TrainingDayExerciseService>();

// Repository til sæt
builder.Services.AddScoped<
    IExerciseSetRepository,
    ExerciseSetRepository>();

// Service til sæt
builder.Services.AddScoped<
    IExerciseSetService,
    ExerciseSetService>();

builder.Services.AddScoped<JwtService>();

builder.Services.Configure<ForwardedHeadersOptions>(options =>
{
    options.ForwardedHeaders =
        ForwardedHeaders.XForwardedFor |
        ForwardedHeaders.XForwardedProto;

    options.KnownNetworks.Clear();

    options.KnownProxies.Clear();
});

var app = builder.Build();


// Sørger for at databasen bliver migreret
// og faste data bliver seedet når API'et starter.
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;

    var logger =
        services.GetRequiredService<
            ILogger<Program>>();

    var context =
        services.GetRequiredService<
            ApplicationDbContext>();

    const int maxAttempts = 5;

    var attempt = 0;

    var delay =
        TimeSpan.FromSeconds(2);

    while (true)
    {
        try
        {
            attempt++;

            // Kører EF Core migrations
            await context.Database.MigrateAsync();

            // Seeder faste data som
            // muskelgrupper og øvelser
            await DbSeeder.SeedAsync(context);

            logger.LogInformation(
                "Database migration and seeding completed successfully."
            );

            break;
        }
        catch (SqlException ex)
            when (attempt < maxAttempts)
        {
            logger.LogWarning(
                ex,
                "Database migration attempt {Attempt} failed — retrying in {Seconds}s",
                attempt,
                delay.TotalSeconds
            );

            await Task.Delay(delay);

            delay = delay * 2;
        }
        catch (Exception ex)
        {
            logger.LogError(
                ex,
                "An error occurred while migrating or seeding the database."
            );

            throw;
        }
    }
}

app.UseSwagger();

app.UseSwaggerUI(options =>
{
    // Beholder JWT login i Swagger
    options.EnablePersistAuthorization();
});

app.MapGet(
    "/api/health",
    () => Results.Text("Healthy\n")
);

app.UseForwardedHeaders();

app.Use(async (context, next) =>
{
    Log.Information(
        "Incoming request {Method} {Path} from {RemoteIp}",
        context.Request.Method,
        context.Request.Path,
        context.Connection.RemoteIpAddress?.ToString() ?? "unknown"
    );

    await next();
});

// HTTPS håndteres af Nginx
// app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

// Starter API'et
app.Run();