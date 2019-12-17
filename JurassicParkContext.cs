using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace jurassic_park
{
  public partial class JurassicParkContext : DbContext
  {
    public DbSet<Dinosaur> Dinosaurs { get; set; }

    public JurassicParkContext()
    {
    }

    public JurassicParkContext(DbContextOptions<JurassicParkContext> options)
        : base(options)
    {
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
      if (!optionsBuilder.IsConfigured)
      {
        // #warning To protect potentially sensitive information in your connection string, you should move it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.
        optionsBuilder.UseNpgsql("server=localhost;database=jurassic_park;user id=postgres;password=");
      }
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
      OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
  }
}
