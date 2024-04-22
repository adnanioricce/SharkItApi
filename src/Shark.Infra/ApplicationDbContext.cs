using Microsoft.EntityFrameworkCore;
using Shark.Domain.CustomerManagement;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
        
    }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {        
        
        // modelBuilder.Entity<CustomerEntity>(entity => {
        //     entity.ToTable("customers");

        //     entity.HasKey(e => e.CustomerId).HasName("customer_id");     
        //     // entity       
        //     entity.Property(e => e.CustomerId).HasColumnName("customer_id").IsRequired();
        //     entity.Property(e => e.FirstName)
        //         .IsRequired()
        //         .HasMaxLength(100)
        //         .HasColumnName("first_name");

        //     entity.Property(e => e.MiddleName)
        //         .HasMaxLength(100)
        //         .HasColumnName("middle_name");

        //     entity.Property(e => e.DateOfBirth)
        //         .IsRequired()                
        //         .HasColumnName("date_of_birth");

        //     entity.Property(e => e.CPF)
        //         .IsRequired()
        //         .HasMaxLength(20)
        //         .HasColumnName("cpf");
        //     entity.HasMany(e => e.Addresses)
        //         .WithOne()
        //         // .HasForeignKey("customer_id")
        //         .IsRequired(false);
        //     // entity.OwnsMany(e => e.Addresses)            
        //     //     .WithOwner(e => e.Customer)
        //     //     .HasForeignKey("customer_id");
        //         // .OnDelete(DeleteBehavior.Cascade);
        //     // entity.Navigation(e => e.Addresses).AutoInclude();
        // });
        // modelBuilder.Entity<AddressEntity>(entity => {
        //     entity.ToTable("customer_addresses");            
        //     entity.HasKey(e => e.AddressId)
        //         .HasName("address_id");    

        //     // entity.HasOne(e => e.Customer)            
        //     //     .WithMany(e => e.Addresses)
        //     //     .HasForeignKey("customer_id")                
        //     //     .IsRequired();
        //     // entity.Property(e => e.CustomerId)
        //     //     .HasColumnName("customer_id")
        //     //     .IsRequired();            
        //     entity.Property(e => e.AddressLine1)
        //         .HasColumnName("address_line_1")
        //         .HasMaxLength(255)                
        //         .IsRequired();
        //     entity.Property(e => e.AddressLine2)
        //         .HasColumnName("address_line_2")
        //         .HasMaxLength(255);                
        //     entity.Property(e => e.Number)
        //         .HasColumnName("number")
        //         .HasColumnType("INT")
        //         .IsRequired();
        //     entity.Property(e => e.District)
        //         .HasColumnName("district")                
        //         .HasMaxLength(100)
        //         .IsRequired();
        //     entity.Property(e => e.City)
        //         .HasColumnName("city")
        //         .HasMaxLength(100)
        //         .IsRequired();
        //     entity.Property(e => e.State)
        //         .HasColumnName("state")
        //         .HasMaxLength(100)
        //         .IsRequired();
        //     entity.Property(e => e.PostalCode)
        //         .HasColumnName("postal_code")
        //         .HasMaxLength(20)
        //         .IsRequired();            
        // });
        modelBuilder.Entity<CustomerEntity>(entity =>
        {
            entity.HasKey(e => e.CustomerId).HasName("customers_pkey");

            entity.ToTable("customers");

            entity.Property(e => e.CustomerId)
                .ValueGeneratedNever()
                .HasColumnName("customer_id");
            entity.Property(e => e.CPF)
                .HasMaxLength(20)
                .HasColumnName("cpf");
            entity.Property(e => e.DateOfBirth).HasColumnName("date_of_birth");
            entity.Property(e => e.FirstName)
                .HasMaxLength(255)
                .HasColumnName("first_name");
            entity.Property(e => e.MiddleName)
                .HasMaxLength(255)
                .HasColumnName("middle_name");
            entity.Navigation(e => e.CustomerAddresses).AutoInclude();
        });

        modelBuilder.Entity<CustomerAddress>(entity =>
        {
            entity.HasKey(e => e.AddressId).HasName("customer_addresses_pkey");

            entity.ToTable("customer_addresses");

            entity.Property(e => e.AddressId)
                .ValueGeneratedNever()
                .HasColumnName("address_id");
            entity.Property(e => e.AddressLine1)
                .HasMaxLength(255)
                .HasColumnName("address_line_1");
            entity.Property(e => e.AddressLine2)
                .HasMaxLength(255)
                .HasColumnName("address_line_2");
            entity.Property(e => e.City)
                .HasMaxLength(100)
                .HasColumnName("city");
            entity.Property(e => e.CustomerId).HasColumnName("customer_id");
            entity.Property(e => e.District)
                .HasMaxLength(100)
                .HasColumnName("district");
            entity.Property(e => e.Number).HasColumnName("number");
            entity.Property(e => e.PostalCode)
                .HasMaxLength(20)
                .HasColumnName("postal_code");
            entity.Property(e => e.State)
                .HasMaxLength(100)
                .HasColumnName("state");

            entity.HasOne(d => d.Customer).WithMany(p => p.CustomerAddresses)
                .HasForeignKey(d => d.CustomerId)
                .HasConstraintName("customer_addresses_customer_id_fkey");
        });

        base.OnModelCreating(modelBuilder);
    }
}