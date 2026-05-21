using MechanicShop.Domin.Customers;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Mechanic.Infrastructure.Data.Configurations
{
    public class CustmoerConfigurations : IEntityTypeConfiguration<Customer>
    {
        public void Configure(EntityTypeBuilder<Customer> builder)
        {
            builder.HasKey(c => c.Id).IsClustered(false);

            builder.Property(c => c.Name)
                   .IsRequired()
                   .HasMaxLength(150);

            builder.Property(c => c.PhoneNumber)
                    .IsRequired()
                    .HasMaxLength(20);

            builder.Property(c => c.Email)
                   .HasMaxLength(150);

            builder.HasMany(c => c.Vehicles).WithOne().HasForeignKey(v => v.CustmoerId);

            builder.Navigation(c => c.Vehicles)
           .UsePropertyAccessMode(PropertyAccessMode.Field);
        }



    }
}
