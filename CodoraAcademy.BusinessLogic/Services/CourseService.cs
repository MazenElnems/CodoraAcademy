using CodoraAcademy.BusinessLogic.DTOs;
using CodoraAcademy.BusinessLogic.DTOs.Static;
using CodoraAcademy.BusinessLogic.Services.Interfaces;
using CodoraAcademy.DataAccess.Entities;
using CodoraAcademy.DataAccess.Enums;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodoraAcademy.BusinessLogic.Services
{
    public class CourseService : ICourseService
    {
        private readonly List<Course> _courses;
        public CourseService()
        {
            _courses = new List<Course>();
            //addSomeMockData();
        }

        private void addSomeMockData()
        {
            _courses.AddRange(new List<Course>
            {
                new Course
                {
                    Id = Guid.NewGuid(),
                    Title = "Introduction to C# Programming",
                    Description = "Learn the fundamentals of C# programming language including syntax, data types, control structures, and object-oriented programming concepts.",
                    Status = CourseStatus.Approved,
                    CreatedAt = DateTime.Now.AddDays(-30),
                    Sections = new List<Section>()
                },
                new Course
                {
                    Id = Guid.NewGuid(),
                    Title = "Advanced Web Development with ASP.NET Core",
                    Description = "Master modern web development using ASP.NET Core, including MVC patterns, API development, authentication, and deployment strategies.",
                    Status = CourseStatus.Approved,
                    CreatedAt = DateTime.Now.AddDays(-25),
                    Sections = new List<Section>()
                },
                new Course
                {
                    Id = Guid.NewGuid(),
                    Title = "Database Design and SQL Fundamentals",
                    Description = "Comprehensive course covering database design principles, SQL queries, normalization, and database management systems.",
                    Status = CourseStatus.Approved,
                    CreatedAt = DateTime.Now.AddDays(-20),
                    Sections = new List<Section>()
                },
                new Course
                {
                    Id = Guid.NewGuid(),
                    Title = "Machine Learning with Python",
                    Description = "Explore machine learning algorithms, data preprocessing, model training, and evaluation using Python and popular ML libraries.",
                    Status = CourseStatus.Pending,
                    CreatedAt = DateTime.Now.AddDays(-15),
                    Sections = new List<Section>()
                },
                new Course
                {
                    Id = Guid.NewGuid(),
                    Title = "Frontend Development with React.js",
                    Description = "Build modern, responsive web applications using React.js, including hooks, state management, routing, and component architecture.",
                    Status = CourseStatus.Approved,
                    CreatedAt = DateTime.Now.AddDays(-10),
                    Sections = new List<Section>()
                },
                new Course
                {
                    Id = Guid.NewGuid(),
                    Title = "DevOps and CI/CD Pipeline",
                    Description = "Learn DevOps practices, containerization with Docker, CI/CD pipelines, and cloud deployment strategies.",
                    Status = CourseStatus.Pending,
                    CreatedAt = DateTime.Now.AddDays(-5),
                    Sections = new List<Section>()
                },
                new Course
                {
                    Id = Guid.NewGuid(),
                    Title = "Mobile App Development with Flutter",
                    Description = "Create cross-platform mobile applications using Flutter framework, covering UI design, state management, and app deployment.",
                    Status = CourseStatus.Approved,
                    CreatedAt = DateTime.Now.AddDays(-3),
                    Sections = new List<Section>()
                },
                new Course
                {
                    Id = Guid.NewGuid(),
                    Title = "Cybersecurity Fundamentals",
                    Description = "Understand cybersecurity principles, network security, cryptography, ethical hacking, and security best practices.",
                    Status = CourseStatus.Rejected,
                    CreatedAt = DateTime.Now.AddDays(-1),
                    Sections = new List<Section>()
                }
            });
        }

        public async Task<CourseResultDto> CreateCourseAsync(CourseCreateDto courseCreateDto,CourseStatus status = CourseStatus.Pending)
        {
            if(courseCreateDto == null)
                throw new ArgumentNullException(nameof(courseCreateDto), "Course creation data cannot be null. Please provide valid course information.");

            if(string.IsNullOrEmpty(courseCreateDto.Title))
                throw new ArgumentException("Course title is required and cannot be null or empty. Please provide a valid course title.", nameof(courseCreateDto.Title));

            Course course =  courseCreateDto.ToCourse();
            
            // Initialize course data
            course.Id = Guid.NewGuid();
            course.Status = status;
            course.CreatedAt = DateTime.Now;

            // Insert the course in the data store
            _courses.Add(course);

            await Task.CompletedTask;
            return course.ToCourseResult();
        }

        public async Task<bool> DeleteCourseAsync(Guid courseId)
        {
            if (courseId == Guid.Empty)
                throw new ArgumentException("Course ID cannot be empty. Please provide a valid course identifier.", nameof(courseId));

            Course? courseToDelete = _courses.FirstOrDefault(c => c.Id == courseId);
            
            if (courseToDelete == null)
                return false;

            bool removed = _courses.Remove(courseToDelete);
            await Task.CompletedTask;
            return removed;
        }

        public async Task<IEnumerable<CourseResultDto>> GetAllCoursesAsync()
        {
            IEnumerable<CourseResultDto> courseResults = _courses
                .Select(c => c.ToCourseResult());
            await Task.CompletedTask;
            return courseResults;
        }

        public async Task<CourseResultDto?> GetCourseByIdAsync(Guid courseId)
        {
            Course? course = _courses.FirstOrDefault(c => c.Id.Equals(courseId));
            await Task.CompletedTask;
            return course?.ToCourseResult();
        }

        public async Task<CourseResultDto?> GetCourseByTitleAsync(string title)
        {
            if(title == null)
                throw new ArgumentNullException(nameof(title), "Course title cannot be null. Please provide a valid course title to search for.");

            Course? course = _courses.FirstOrDefault(c => c.Title == title);
            await Task.CompletedTask;
            return course?.ToCourseResult();
        }

        public async Task<IEnumerable<CourseResultDto>> GetFilteredCoursesAsync(CourseQueryDto courseQueryDto)
        {
            if(courseQueryDto == null)
                throw new ArgumentNullException(nameof(courseQueryDto), "Course query parameters cannot be null. Please provide valid search and filter criteria.");

            IEnumerable<Course> filteredCourses = courseQueryDto.SearchBy switch
            {
                nameof(CourseResultDto.Title) => _courses.Where(c => c.Title.ToLower().Contains(courseQueryDto.SearchString.ToLower())),
                nameof(CourseResultDto.Description) => _courses.Where(c => c.Description.ToLower().Contains(courseQueryDto.SearchString.ToLower())),
                nameof(CourseResultDto.Status) => _courses.Where(c => c.Status.ToString().ToLower().StartsWith(courseQueryDto.SearchString.ToLower())),
                _ => _courses
            };

            IEnumerable<Course> sortedCourses = (courseQueryDto.SortBy, courseQueryDto.SortOrder) switch
            {
                (nameof(CourseResultDto.Title), SortOrderOptions.ASC) => filteredCourses.OrderBy(c => c.Title),
                (nameof(CourseResultDto.Title), SortOrderOptions.DESC) => filteredCourses.OrderByDescending(c => c.Title),
                (nameof(CourseResultDto.Description), SortOrderOptions.ASC) => filteredCourses.OrderBy(c => c.Description),
                (nameof(CourseResultDto.Description), SortOrderOptions.DESC) => filteredCourses.OrderByDescending(c => c.Description),
                (nameof(CourseResultDto.Status), SortOrderOptions.ASC) => filteredCourses.OrderBy(c => c.Status),
                (nameof(CourseResultDto.Status), SortOrderOptions.DESC) => filteredCourses.OrderByDescending(c => c.Status),
                (nameof(CourseResultDto.CreatedAt), SortOrderOptions.ASC) => filteredCourses.OrderBy(c => c.CreatedAt),
                (nameof(CourseResultDto.CreatedAt), SortOrderOptions.DESC) => filteredCourses.OrderByDescending(c => c.CreatedAt),
                _ => filteredCourses
            };

            IEnumerable<CourseResultDto> courseResults = sortedCourses
                .Select(c => c.ToCourseResult())
                .ToList();
            await Task.CompletedTask;
            return courseResults;
        }

        public async Task<CourseResultDto> UpdateCourseAsync(CourseUpdateDto courseUpdateDto)
        {
            if(courseUpdateDto == null)
                throw new ArgumentNullException(nameof(courseUpdateDto), "Course update data cannot be null. Please provide valid course information for updating.");

            if (string.IsNullOrWhiteSpace(courseUpdateDto.Title) || string.IsNullOrEmpty(courseUpdateDto.Title))
                throw new ArgumentException("Course title is required and cannot be null, empty, or contain only whitespace. Please provide a valid course title.", nameof(courseUpdateDto.Title));

            if (string.IsNullOrWhiteSpace(courseUpdateDto.Status) ||
                (courseUpdateDto.Status != CourseStatus.Pending.ToString() &&
                 courseUpdateDto.Status != CourseStatus.Approved.ToString() &&
                 courseUpdateDto.Status != CourseStatus.Rejected.ToString()))
            {
                throw new ArgumentException($"Invalid course status: '{courseUpdateDto.Status}'. Valid statuses are: {CourseStatus.Pending}, {CourseStatus.Approved}, or {CourseStatus.Rejected}.", nameof(courseUpdateDto.Status));
            }

            Course? course = _courses.FirstOrDefault(c => c.Id == courseUpdateDto.Id);

            if (course == null)
                throw new InvalidOperationException($"Course with ID '{courseUpdateDto.Id}' was not found. Please provide a valid course identifier for updating.");

            course.Title = courseUpdateDto.Title;
            course.Status = Enum.Parse<CourseStatus>(courseUpdateDto.Status);
            course.Description = courseUpdateDto.Description;
            
            await Task.CompletedTask;
            return course.ToCourseResult();
        }
    }
}
