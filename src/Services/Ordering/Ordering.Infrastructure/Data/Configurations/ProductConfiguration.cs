namespace Ordering.Infrastructure.Data.Configurations;

public class ProductConfiguration :IEntityTypeConfiguration<Product>
{
    public void Configure(EntityTypeBuilder<Product> builder)
    {
        builder.HasIndex(x => x.Id); 
        
        builder.Property(x=>x.Id)
            .HasConversion(
                productId => productId.Value,
                dbId => ProductId.Of(dbId));
        
        builder.Property(p => p.Name)
            .HasMaxLength(100)
            .IsRequired();
        
        
    }
}