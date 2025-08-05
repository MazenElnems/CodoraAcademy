using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodoraAcademy.DataAccess.Entities
{
    public class LessonVideo
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public int DurationInMinutes { get; set; }
        public int Order { get; set; }
        public string VideoUrl { get; set; }
        public DateTime CreatedAt { get; set; }

        // Foreign keys
        public Guid SectionId { get; set; }

        // Navigation Properties
        public Section Section { get; set; }
    }
}
