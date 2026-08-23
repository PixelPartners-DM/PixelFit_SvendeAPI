using Microsoft.EntityFrameworkCore;
using PixelFit_SvendeAPI.Models;

namespace PixelFit_SvendeAPI.Data
{
    public static class DbSeeder
    {
        public static async Task SeedAsync(
            ApplicationDbContext context)
        {

            if (!await context.MuscleGroups.AnyAsync())
            {
                var muscleGroups = new List<MuscleGroup>
                {
                    new() { Name = "Bryst" },
                    new() { Name = "Ryg" },
                    new() { Name = "Skuldre" },
                    new() { Name = "Biceps" },
                    new() { Name = "Triceps" },
                    new() { Name = "Ben" },
                    new() { Name = "Mave" },
                    new() { Name = "Underarme" },
                    new() { Name = "Nakke" },
                    new() { Name = "Lægge" },
                    new() { Name = "Balder" }
                };

                await context.MuscleGroups.AddRangeAsync(
                    muscleGroups
                );

                await context.SaveChangesAsync();
            }



            await AddExercise(
                context,
                "Bench Press",
                "Bryst",
                "images/exercises/BenchPress.webp"
            );


            await AddExercise(
                context,
                "Leg Extension",
                "Ben",
                "images/legEx.jpg"
            );
        }



        private static async Task AddExercise(
            ApplicationDbContext context,
            string exerciseName,
            string muscleGroupName,
            string imageUrl)
        {
            // Stop hvis øvelsen allerede findes
            var exerciseExists =
                await context.Exercises.AnyAsync(
                    exercise =>
                        exercise.Name == exerciseName
                );


            if (exerciseExists)
            {
                return;
            }


            // Finder muskelgruppen
            var muscleGroup =
                await context.MuscleGroups
                    .FirstAsync(
                        group =>
                            group.Name == muscleGroupName
                    );


            // Opretter øvelsen
            var exercise = new Exercise
            {
                Name = exerciseName,

                MuscleGroupId =
                    muscleGroup.Id,

                ImageUrl =
                    imageUrl
            };


            await context.Exercises.AddAsync(
                exercise
            );


            await context.SaveChangesAsync();
        }
    }
}