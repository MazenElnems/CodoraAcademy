using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodoraAcademy.DataAccess.Entities
{
    public class Section
    {
        public Guid Id { get; set; }
        public string Name { get; set; }    
        public DateTime CreatedAt { get; set; }
        public int Order { get; set; }

        // Foreign keys
        public Guid CourseId { get; set; }

        // Navigation Properties
        public Course Course { get; set; }
        public IEnumerable<LessonVideo> LessonVideos { get; set; }
        public IEnumerable<SectionMaterial> Materials { get; set; }  
    }
}
