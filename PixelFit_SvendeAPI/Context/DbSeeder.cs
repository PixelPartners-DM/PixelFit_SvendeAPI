using Microsoft.EntityFrameworkCore;
using PixelFit_SvendeAPI.Models;

namespace PixelFit_SvendeAPI.Data
{
    public static class DbSeeder
    {
        public static async Task SeedAsync(
            ApplicationDbContext context)
        {
            // Hvis der allerede findes muskelgrupper,
            // opretter vi dem ikke igen
            if (await context.MuscleGroups.AnyAsync())
            {
                return;
            }



            var chest = new MuscleGroup
            {
                Name = "Bryst"
            };

            var back = new MuscleGroup
            {
                Name = "Ryg"
            };

            var shoulders = new MuscleGroup
            {
                Name = "Skuldre"
            };

            var biceps = new MuscleGroup
            {
                Name = "Biceps"
            };

            var triceps = new MuscleGroup
            {
                Name = "Triceps"
            };

            var legs = new MuscleGroup
            {
                Name = "Ben"
            };

            var abs = new MuscleGroup
            {
                Name = "Mave"
            };

            var forearms = new MuscleGroup
            {
                Name = "Underarme"
            };

            var neck = new MuscleGroup
            {
                Name = "Nakke"
            };

            var calves = new MuscleGroup
            {
                Name = "Lægge"
            };

            var glutes = new MuscleGroup
            {
                Name = "Balder"
            };


            // Tilføjer alle muskelgrupperne til databasen
            await context.MuscleGroups.AddRangeAsync(
                chest,
                back,
                shoulders,
                biceps,
                triceps,
                legs,
                abs,
                forearms,
                neck,
                calves,
                glutes
            );


            // Gemmer ændringerne i SQL Server
            await context.SaveChangesAsync();
        }
    }
}