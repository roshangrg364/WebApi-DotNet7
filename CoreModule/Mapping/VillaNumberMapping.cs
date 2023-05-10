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
    public class VillaNumberMapping : IEntityTypeConfiguration<VillaNumber>
    {
        public void Configure(EntityTypeBuilder<VillaNumber> builder)
        {
            builder.HasKey(a => a.Id);
            builder.Property<string>(a => a.VillaNo).IsRequired(true).HasMaxLength(200);
            builder.Property<string?>(a => a.Details).HasMaxLength(500).IsRequired(false);
            builder.Property<DateTime>(a => a.CreatedDate).IsRequired();
            builder.Property<DateTime>(a => a.UpdatedDate).IsRequired();
            builder.HasOne(a => a.Villa).WithMany(a=>a.VillaNumbers).HasForeignKey(a => a.VillaId);
        }
    }
}
