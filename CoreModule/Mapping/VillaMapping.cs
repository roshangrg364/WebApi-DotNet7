using CoreModule.Src;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreModule.Mapping
{
    public class VillaMapping : IEntityTypeConfiguration<Villa>
    {
        public void Configure(EntityTypeBuilder<Villa> builder)
        {
            builder.HasKey(a => a.Id);
            builder.Property<string>(a => a.Name).IsRequired().HasMaxLength(100);
            builder.Property<string?>(a => a.Details).HasMaxLength(5000);
            builder.Property<decimal>(a => a.Rate).HasPrecision(18,2).IsRequired();
            builder.Property<string?>(a => a.ImageUrl).HasMaxLength(100);
            builder.Property<decimal?>(a => a.Area).HasPrecision(18,2);
            builder.Property<string?>(a => a.ImageUrl).HasMaxLength(100);
            builder.Property<string?>(a => a.Occupancy).HasMaxLength(100);
            builder.Property<string?>(a => a.Amenity).HasMaxLength(100);
            builder.Property<DateTime>(a => a.CreatedDate);
            builder.Property<DateTime>(a => a.UpdatedDate);
            builder.HasMany(a => a.VillaNumbers).WithOne(a => a.Villa).HasForeignKey(a => a.VillaId);

        }
    }
}
