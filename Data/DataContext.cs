using Microsoft.EntityFrameworkCore;

public class DataContext : DbContext{

    public DataContext( DbContextOptions<DataContext> opt ) : base(opt)
    {
        
    }

    public DbSet<Character> Characters { get; set; }
    public DbSet<User> Users { get; set; }
    
}