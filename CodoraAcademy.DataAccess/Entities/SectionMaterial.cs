using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodoraAcademy.DataAccess.Entities
{
    public class SectionMaterial
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string FileUrl { get; set; }
        public DateTime CreatedAt { get; set; }

        // Foreign keys
        public Guid SectionId { get; set; }

        // Navigation Properties
        public Section Section { get; set; }
    }
}
