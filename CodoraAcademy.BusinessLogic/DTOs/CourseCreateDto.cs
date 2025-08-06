using CodoraAcademy.DataAccess.Entities;
using CodoraAcademy.DataAccess.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodoraAcademy.BusinessLogic.DTOs
{
    public class CourseCreateDto
    {
        public string Title { get; set; }
        public string Description { get; set; }

        public Course ToCourse()
        {
            return new Course
            {
                Title = Title,
                Description = Description
            };
        }
    }
}