using Gateway.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Gateway.DomainData;

public class PackageConfiguration : IEntityTypeConfiguration<Package>
{
    public void Configure(EntityTypeBuilder<Package> builder)
    {
        builder.HasKey(p => p.Id);
        
        builder.Property(p => p.DeliveryCost)
            .HasPrecision(10, 2)
            .IsRequired();
        
        builder.Property(p => p.OrderId)
            .IsRequired();
    }
}