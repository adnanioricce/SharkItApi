using Microsoft.EntityFrameworkCore;
using Shark.Domain.CustomerManagement;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
        
    }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<CustomerEntity>(entity => {
            entity.ToTable("customers"); // Set the table name

            entity.HasKey(e => e.CustomerId).HasName("customer_id"); // Define the primary key
            entity.Property(e => e.CustomerId).HasColumnName("customer_id").IsRequired();
            entity.Property(e => e.FirstName)
                .IsRequired()
                .HasMaxLength(100)
                .HasColumnName("first_name");

            entity.Property(e => e.MiddleName)
                .HasMaxLength(100)
                .HasColumnName("middle_name");

            entity.Property(e => e.DateOfBirth)
                .IsRequired()
                .HasColumnName("date_of_birth");

            entity.Property(e => e.CPF)
                .IsRequired()
                .HasMaxLength(20)
                .HasColumnName("cpf");
            // entity.Property(e => e.Addresses)                
            //     .HasColumnType("jsonb")
            //     .HasColumnName("addresses")
            //     .IsRequired();
        });
        
        base.OnModelCreating(modelBuilder);
    }
}