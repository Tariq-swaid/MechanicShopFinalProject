using MechanicShop.Domin.Employees;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Mechanic.Infrastructure.Data.Configurations
{
    public class EmpolyeeConfigurations : IEntityTypeConfiguration<Employe>
    {
      

        public void Configure(EntityTypeBuilder<Employe> builder)
        {

            builder.HasKey(e => e.Id).IsClustered(false);

            builder.Property(e => e.FisrtName)
                   .IsRequired()
                   .HasMaxLength(50);

            builder.Property(e => e.LastName)
                   .IsRequired()
                   .HasMaxLength(50);

            builder.Property(e => e.Role).HasConversion<string>().IsRequired();
        }
    }
}
