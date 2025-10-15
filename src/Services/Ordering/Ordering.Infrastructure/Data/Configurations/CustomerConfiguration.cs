namespace Ordering.Infrastructure.Data.Configurations;

public class CustomerConfiguration :IEntityTypeConfiguration<Customer>
{
    public void Configure(EntityTypeBuilder<Customer> builder)
    {
        builder.HasKey(x => x.Id);

        // Configure the conversion for CustomerId value object
        builder.Property(x=>x.Id)
            .HasConversion(
                customerId => customerId.Value,
                dbId => CustomerId.Of(dbId));
        
        builder.Property(c => c.Name)
            .HasMaxLength(100)
            .IsRequired();

        builder.Property(c => c.Email)
            .HasMaxLength(225);

        builder.HasIndex(c => c.Email).IsUnique();
    }
}