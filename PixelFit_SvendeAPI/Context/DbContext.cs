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

        public DbSet<TrainingProgram> TrainingPrograms { get; set; }

        public DbSet<TrainingDay> TrainingDays { get; set; }

        public DbSet<MuscleGroup> MuscleGroups { get; set; }

        public DbSet<Exercise> Exercises { get; set; }

        public DbSet<TrainingDayExercise> TrainingDayExercises { get; set; }

        public DbSet<ExerciseSet> ExerciseSets { get; set; }

        public DbSet<TrainingHistory> TrainingHistories { get; set; }


        public DbSet<UserProfile> UserProfiles { get; set; }

        public DbSet<DailyNutritionSummary> DailyNutritionSummaries { get; set; }


        public DbSet<Meal> Meals { get; set; }

        public DbSet<FoodItem> FoodItems { get; set; }

        public DbSet<WeightEntry> WeightEntries { get; set; }


    }
}