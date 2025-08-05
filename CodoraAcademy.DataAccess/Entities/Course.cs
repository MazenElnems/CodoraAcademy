using CodoraAcademy.DataAccess.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodoraAcademy.DataAccess.Entities
{
    public class Course
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public CourseStatus Status { get; set; }
        public DateTime CreatedAt { get; set; }

        // Navigation Properties
        public IEnumerable<Section> Sections { get; set; }
    }
}
