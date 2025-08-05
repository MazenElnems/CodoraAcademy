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
    public class LessonVideoEntityConfiguration : IEntityTypeConfiguration<LessonVideo>
    {
        public void Configure(EntityTypeBuilder<LessonVideo> builder)
        {
            builder
                .ToTable("LessonVideos");

            builder
                .HasKey(l => l.Id);

            builder
                .Property(l => l.VideoUrl)
                .HasColumnType("nvarchar(max)")
                .IsRequired();

            builder
                .HasOne(l => l.Section)
                .WithMany(s => s.LessonVideos)
                .HasForeignKey(l => l.SectionId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
