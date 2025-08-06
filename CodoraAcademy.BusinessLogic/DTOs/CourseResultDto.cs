using CodoraAcademy.DataAccess.Entities;
using CodoraAcademy.DataAccess.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodoraAcademy.BusinessLogic.DTOs
{
    public class CourseResultDto : IEquatable<CourseResultDto>
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public CourseStatus Status { get; set; }
        public DateTime CreatedAt { get; set; }

        public override bool Equals(object obj)
        {
            return Equals(obj as CourseResultDto);
        }

        public bool Equals(CourseResultDto other)
        {
            if (other is null)
                return false;

            if (ReferenceEquals(this, other))
                return true;

            return Id.Equals(other.Id) &&
                   Title == other.Title &&
                   Description == other.Description &&
                   Status == other.Status &&
                   CreatedAt.Equals(other.CreatedAt);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Id, Title, Description, Status, CreatedAt);
        }

        public static bool operator ==(CourseResultDto left, CourseResultDto right)
        {
            return EqualityComparer<CourseResultDto>.Default.Equals(left, right);
        }

        public static bool operator !=(CourseResultDto left, CourseResultDto right)
        {
            return !(left == right);
        }
    }

    public static class CourseExtensions
    {
        public static CourseResultDto ToCourseResult(this Course course)
        {
            return new CourseResultDto
            {
                Id = course.Id,
                Title = course.Title,
                Description = course.Description,
                Status = course.Status,
                CreatedAt = course.CreatedAt,
            };
        }
    }
}
