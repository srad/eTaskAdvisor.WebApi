using eTaskAdvisor.WebApi.Data.SchemaPoco;
using Microsoft.EntityFrameworkCore;

namespace eTaskAdvisor.WebApi.Data
{
  public class AppDbContext : DbContext
  {
   public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
      Database.EnsureCreated();
    }

/*
    public DbSet<Client> Clients { get; set; }
    public DbSet<Activity> Activities { get; set; }
    public DbSet<Affect> Affects { get; set; }
    public DbSet<Factor> Factors { get; set; }
    public DbSet<Influence> Influences { get; set; }
    public DbSet<Task> Tasks { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
      optionsBuilder.UseMySQL("server=localhost;database=etaskadvisor;user=root;password=root");
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
      base.OnModelCreating(modelBuilder);

      modelBuilder.Entity<Client>().ToTable("clients");
      modelBuilder.Entity<Activity>().ToTable("activities");
      modelBuilder.Entity<Affect>().ToTable("afffects");
      modelBuilder.Entity<Factor>().ToTable("factors");
      modelBuilder.Entity<Influence>().ToTable("influences");
      modelBuilder.Entity<Task>().ToTable("tasks");

      modelBuilder.Entity<Client>(entity =>
      {
        entity.HasKey(e => e.ClientId);
        entity.Property(e => e.Name).IsRequired();
        entity.Property(e => e.Password).IsRequired();
        entity.HasMany(t => t.Tasks);
      });

      modelBuilder.Entity<Activity>(entity =>
      {
        entity.HasKey(e => e.ActivityId);
        entity.Property(e => e.Name).IsRequired();
        entity.Property(e => e.Description).IsRequired();
      });

      modelBuilder.Entity<Factor>(entity =>
      {
        entity.HasKey(e => e.FactorId);
        entity.Property(e => e.Name).IsRequired();
        entity.Property(e => e.Description).IsRequired();
      });

      modelBuilder.Entity<Influence>(entity =>
      {
        entity.HasKey(e => e.InfluenceName);
        entity.Property(e => e.InfluenceDisplay).IsRequired();
      });

      modelBuilder.Entity<Affect>(entity =>
      {
        entity.HasKey(e => new { e.ActivityId, e.FactorId, e.InfluenceName });
        entity.Property(e => e.ActivityId).IsRequired();
        entity.Property(e => e.FactorId).IsRequired();
        entity.Property(e => e.InfluenceName).IsRequired();
        entity.Property(e => e.Source).IsRequired();
        
        entity.HasOne(e => e.Activity);
        entity.HasOne(e => e.Factor);
        entity.HasOne(e => e.Influence);
      });
    }
    */
  }
}