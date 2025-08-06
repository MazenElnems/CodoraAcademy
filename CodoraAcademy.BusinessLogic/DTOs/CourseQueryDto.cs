using CodoraAcademy.BusinessLogic.DTOs.Static;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodoraAcademy.BusinessLogic.DTOs
{
    public class CourseQueryDto
    {
        public string SearchBy { get; set; } = nameof(CourseResultDto.Title);
        public string SearchString { get; set; } = string.Empty;
        public string SortBy { get; set; } = nameof(CourseResultDto.Title);
        public string SortOrder { get; set; } = SortOrderOptions.ASC;
    }
}
