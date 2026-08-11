using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using PixelFit_SvendeAPI.Models;

namespace PixelFit_SvendeAPI.Data
{
    public class ApplicationDbContext : IdentityDbContext<User, IdentityRole<int>, int>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        // Add other DbSet<T> here if you want to query them via DbContext directly
        public DbSet<TrainingProgram> TrainingPrograms { get; set; }
        public DbSet<TrainingDay> TrainingDays { get; set; }
    }
}