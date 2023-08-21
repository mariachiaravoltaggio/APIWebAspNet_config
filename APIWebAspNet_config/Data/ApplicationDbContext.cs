using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore;
using Microsoft.SqlServer.Server;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace APIWebAspNet_config.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {

        }

        //se non lo indichiamo in Program.cs:
        // protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) { 
        // optionsBuilder.UseSqlServer(MyConnection); }

        /* public DbSet<ModelName> ApplicationUsers { get; set; }
        inserimento manuale nel db, con e migrazioni verranno automaticamente inseriti
       protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<ModelName>().HasData(
                new ModelName()
                {
                  … si inseriscono i dati in formato json
                },
                new ModelName()
                {
                   …  }
                );
        }*/
    }
}


