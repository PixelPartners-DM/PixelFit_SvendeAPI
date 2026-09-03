using Microsoft.EntityFrameworkCore;
using PixelFit_SvendeAPI.Models;

namespace PixelFit_SvendeAPI.Data
{
    
    /// Hjælpeklasse til at populere databasen med standarddata.
    /// Indeholder logik til at oprette manglende muskelgrupper og eksempelsøvelser.
   
    public static class DbSeeder
    {
        
        
        public static async Task SeedAsync(
            ApplicationDbContext context)
        {

            // Hvis der ingen muskelgrupper findes, opret et sæt standardgrupper.
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



            // Tilføj enkelte standardøvelser — AddExercise sørger for at undgå duplikater.
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
                "images/exercises/legEx.jpg"
            );
        }



        /// Tilføjer en øvelse til databasen hvis den ikke allerede findes.
        /// Søger efter den angivne muskelgruppe og sætter dennes Id på øvelsen.
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


            // Find den tilsvarende muskelgruppe; hvis ikke fundet vil FirstAsync kaste.
            var muscleGroup =
                await context.MuscleGroups
                    .FirstAsync(
                        group =>
                            group.Name == muscleGroupName
                    );


            // Opret og konfigurer øvelsen med den fundne muskelgruppes Id.
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