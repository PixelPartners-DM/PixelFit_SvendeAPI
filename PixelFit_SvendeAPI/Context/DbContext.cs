using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using PixelFit_SvendeAPI.Models;

namespace PixelFit_SvendeAPI.Data
{
    public class ApplicationDbContext
        : IdentityDbContext<User, IdentityRole<int>, int>
    {
        public ApplicationDbContext(
            DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        // Træningsprogrammer
        public DbSet<TrainingProgram> TrainingPrograms { get; set; }

        // Træningsdage
        public DbSet<TrainingDay> TrainingDays { get; set; }

        // Muskelgrupper
        public DbSet<MuscleGroup> MuscleGroups { get; set; }

        // Øvelsesbibliotek
        public DbSet<Exercise> Exercises { get; set; }

        // Øvelser som brugeren har valgt til en træningsdag
        public DbSet<TrainingDayExercise> TrainingDayExercises { get; set; }

        // Sæt til de valgte øvelser
        public DbSet<ExerciseSet> ExerciseSets { get; set; }
    }
}