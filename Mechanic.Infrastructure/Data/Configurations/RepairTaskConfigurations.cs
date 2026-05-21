using MechanicShop.Domin.RepierTask;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;


namespace Mechanic.Infrastructure.Data.Configurations
{
    public class RepairTaskConfigurations : IEntityTypeConfiguration<RepairTask>
    {
        public void Configure(EntityTypeBuilder<RepairTask> builder)
        {
            builder.HasKey(rt => rt.Id).IsClustered(false);

            builder.Property(rt => rt.Id).ValueGeneratedNever();

            builder.Property(rt => rt.Name)
                   .IsRequired()
                   .HasMaxLength(100);

            builder.Property(w => w.RepairDurationInMinutes).HasConversion<string>().IsRequired();

            builder.Property(rt => rt.RepairDurationInMinutes)
           .IsRequired();

            builder.Property(rt => rt.LaborCost)
                   .IsRequired()
                   .HasColumnType("decimal(18,2)");

            builder.HasMany(c => c.Parts)
               .WithOne()
               .HasForeignKey("RepairTaskId")
               .OnDelete(DeleteBehavior.Cascade);

            builder.Navigation(c => c.Parts)
                .UsePropertyAccessMode(PropertyAccessMode.Field);
        }
    }
}
