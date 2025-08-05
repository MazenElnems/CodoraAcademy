using CodoraAcademy.DataAccess.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodoraAcademy.DataAccess.Configurations
{
    public class SectionMaterialConfiguration : IEntityTypeConfiguration<SectionMaterial>
    {
        public void Configure(EntityTypeBuilder<SectionMaterial> builder)
        {
            builder
                .ToTable("Materials");

            builder 
                .HasKey(m => m.Id);

            builder
                .Property(m => m.FileUrl)
                .HasColumnType("nvarchar(max)")
                .IsRequired();

            builder
                .HasOne(m => m.Section)
                .WithMany(s => s.Materials)
                .HasForeignKey(m => m.SectionId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
