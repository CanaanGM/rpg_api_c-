using Microsoft.EntityFrameworkCore;
using rpg_trial.Models;

public class DataContext : DbContext{

    public DataContext( DbContextOptions<DataContext> opt ) : base(opt)
    {
        
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.Entity<Skill>().HasData(
            new Skill {
                Id = 1,
                Name = "Fire ball",
                Damage = 30
            },
            new Skill {
                Id = 2,
                Name = "Blizzard",
                Damage = 20
            },
            new Skill {
                Id = 3,
                Name = "Rage",
                Damage = 60
            }
        );

    }
    public DbSet<Character> Characters { get; set; }
    public DbSet<User> Users { get; set; }
    public DbSet<Weapon> Weapons { get; set; }
    public DbSet<Skill> Skills { get; set; }
    
}