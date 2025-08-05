using CodoraAcademy.DataAccess.Entities;
using CodoraAcademy.DataAccess.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodoraAcademy.DataAccess.Configurations
{
    public class CourseEntityConfiguration : IEntityTypeConfiguration<Course>
    {
        public void Configure(EntityTypeBuilder<Course> builder)
        {
            builder
                .ToTable("Courses");

            builder
                .HasKey(c => c.Id);

            builder
                .Property(c => c.Status)
                .HasConversion<string>(x => x.ToString(), x => Enum.Parse<CourseStatus>(x));
        }
    }
}
