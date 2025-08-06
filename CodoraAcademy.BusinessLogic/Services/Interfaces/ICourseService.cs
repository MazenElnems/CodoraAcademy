using CodoraAcademy.BusinessLogic.DTOs;
using CodoraAcademy.DataAccess.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodoraAcademy.BusinessLogic.Services.Interfaces
{
    /// <summary>
    /// Defines the contract for course management operations in the CodoraAcademy system.
    /// This interface provides methods for creating, updating, deleting, and retrieving course information.
    /// </summary>
    public interface ICourseService
    {
        /// <summary>
        /// Creates a new course in the system.
        /// </summary>
        /// <param name="courseCreateDto">The course creation data transfer object containing the course information.</param>
        /// <returns>A <see cref="CourseResultDto"/> object containing the created course details including the generated ID and creation timestamp.</returns>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="courseCreateDto"/> is null.</exception>
        /// <exception cref="ArgumentException">Thrown when required course properties are invalid or missing.</exception>
        Task<CourseResultDto> CreateCourseAsync(CourseCreateDto courseCreateDto,CourseStatus status = CourseStatus.Pending);

        /// <summary>
        /// Updates an existing course in the system.
        /// </summary>
        /// <param name="courseUpdateDto">The course update data transfer object containing the course ID and updated information.</param>
        /// <returns>A <see cref="CourseResultDto"/> object containing the updated course details.</returns>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="courseUpdateDto"/> is null.</exception>
        /// <exception cref="ArgumentException">Thrown when the course ID is invalid or course properties are invalid.</exception>
        /// <exception cref="InvalidOperationException">Thrown when the course with the specified ID does not exist.</exception>
        Task<CourseResultDto> UpdateCourseAsync(CourseUpdateDto courseUpdateDto);

        /// <summary>
        /// Deletes a course from the system by its ID.
        /// </summary>
        /// <param name="courseId">The unique identifier of the course to delete.</param>
        /// <returns>True if the course was successfully deleted; otherwise, false.</returns>
        /// <exception cref="ArgumentException">Thrown when <paramref name="courseId"/> is less than or equal to zero.</exception>
        Task<bool> DeleteCourseAsync(Guid courseId);

        /// <summary>
        /// Retrieves a course by its unique identifier.
        /// </summary>
        /// <param name="courseId">The unique identifier of the course to retrieve.</param>
        /// <returns>A <see cref="CourseResultDto"/> object containing the course details if found; otherwise, null.</returns>
        Task<CourseResultDto?> GetCourseByIdAsync(Guid courseId);

        /// <summary>
        /// Retrieves a course by its title.
        /// </summary>
        /// <param name="title">The title of the course to retrieve.</param>
        /// <returns>A <see cref="CourseResultDto"/> object containing the course details if found; otherwise, null.</returns>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="title"/> is null or empty.</exception>
        Task<CourseResultDto?> GetCourseByTitleAsync(string title);

        /// <summary>
        /// Retrieves all courses from the system.
        /// </summary>
        /// <returns>An <see cref="IEnumerable{CourseResultDto}"/> containing all available courses.</returns>
        Task<IEnumerable<CourseResultDto>> GetAllCoursesAsync();

        /// <summary>
        /// Retrieves courses based on specified filtering and sorting criteria.
        /// </summary>
        /// <param name="courseQueryDto">The query data transfer object containing search and sort parameters.</param>
        /// <returns>An <see cref="IEnumerable{CourseResultDto}"/> containing courses that match the specified criteria.</returns>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="courseQueryDto"/> is null.</exception>
        Task<IEnumerable<CourseResultDto>> GetFilteredCoursesAsync(CourseQueryDto courseQueryDto);
    }
}