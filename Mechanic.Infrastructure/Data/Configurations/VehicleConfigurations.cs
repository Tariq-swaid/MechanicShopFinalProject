

using MechanicShop.Domin.Customer.Vehicles;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Mechanic.Infrastructure.Data.Configurations
{
    public class VehicleConfigurations : IEntityTypeConfiguration<Vehicle>
    {
        public void Configure(EntityTypeBuilder<Vehicle> builder)
        {

            builder.HasKey(v => v.Id).IsClustered(false);

            builder.Property(v => v.Id).ValueGeneratedNever();

            builder.Property(v => v.Make)
                   .IsRequired()
                   .HasMaxLength(100);

            builder.Property(v => v.Model)
                   .IsRequired()
                   .HasMaxLength(100);

            builder.HasOne(v => v.Customer).WithMany(c => c.Vehicles).HasForeignKey(v => v.CustmoerId);

            builder.Property(v => v.Year).IsRequired();

            builder.Property(v => v.LicensePlate).IsRequired();
        }
    }
}
