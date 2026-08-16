using Microsoft.EntityFrameworkCore;
using PixelFit_SvendeAPI.Models;

namespace PixelFit_SvendeAPI.Data
{
    public static class DbSeeder
    {
        public static async Task SeedAsync(
            ApplicationDbContext context)
        {
           

            // Opretter kun muskelgrupper hvis der ikke
            // allerede findes nogen i databasen
            if (!await context.MuscleGroups.AnyAsync())
            {
                var muscleGroups = new List<MuscleGroup>
                {
                    new MuscleGroup
                    {
                        Name = "Bryst"
                    },

                    new MuscleGroup
                    {
                        Name = "Ryg"
                    },

                    new MuscleGroup
                    {
                        Name = "Skuldre"
                    },

                    new MuscleGroup
                    {
                        Name = "Biceps"
                    },

                    new MuscleGroup
                    {
                        Name = "Triceps"
                    },

                    new MuscleGroup
                    {
                        Name = "Ben"
                    },

                    new MuscleGroup
                    {
                        Name = "Mave"
                    },

                    new MuscleGroup
                    {
                        Name = "Underarme"
                    },

                    new MuscleGroup
                    {
                        Name = "Nakke"
                    },

                    new MuscleGroup
                    {
                        Name = "Lægge"
                    },

                    new MuscleGroup
                    {
                        Name = "Balder"
                    }
                };


                await context.MuscleGroups.AddRangeAsync(
                    muscleGroups
                );

                await context.SaveChangesAsync();
            }


            // Henter Bryst fra databasen.
            // Nu har den også et rigtigt database-id.
            var chest =
                await context.MuscleGroups
                    .FirstAsync(
                        muscleGroup =>
                            muscleGroup.Name == "Bryst"
                    );



            // Tjekker om Bench Press allerede findes,
            // så den ikke bliver oprettet flere gange
            var benchPressExists =
                await context.Exercises.AnyAsync(
                    exercise =>
                        exercise.Name == "Bench Press"
                );


            if (!benchPressExists)
            {
                var benchPress = new Exercise
                {
                    Name = "Bench Press",

                    // Kobler Bench Press til Bryst
                    MuscleGroupId = chest.Id,

                    // Billedet ligger i MAUI-projektets wwwroot
                    ImageUrl =
                        "images/exercises/BenchPress.webp"
                };


                await context.Exercises.AddAsync(
                    benchPress
                );

                await context.SaveChangesAsync();
            }
        }
    }
}