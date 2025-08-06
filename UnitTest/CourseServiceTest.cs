using CodoraAcademy.BusinessLogic.DTOs;
using CodoraAcademy.BusinessLogic.DTOs.Static;
using CodoraAcademy.BusinessLogic.Services;
using CodoraAcademy.BusinessLogic.Services.Interfaces;
using CodoraAcademy.DataAccess.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnitTest
{
    public class CourseServiceTest
    {
        private readonly ICourseService _courseService;

        public CourseServiceTest()
        {
            _courseService = new CourseService();
        }

        #region CreateCourseAsync

        // if the parameter CourseCreateDto is null the service must throws an ArgumentNullException
        [Fact]
        public async Task CreateCourseAsync_NullParameter_ThrowsArgumentNullException()
        {
            await Assert.ThrowsAsync<ArgumentNullException>(async () => await _courseService.CreateCourseAsync(null,CourseStatus.Pending));
        }

        // if the title is null then service must throws an exception ArgumentException

        [Fact]
        public async Task CreateCourseAsync_NullCourseTitle_ThrowsArgumentException()
        {
            await Assert.ThrowsAsync<ArgumentException>(async () => await _courseService.CreateCourseAsync(new CourseCreateDto
            {
                Title = null,
                Description = string.Empty
            }, CourseStatus.Pending));
        }

        [Fact]
        public async Task CreateCourseAsync_CreateValidCourse_ReturnCourseResultWithId()
        {
            CourseCreateDto courseCreateDto = new CourseCreateDto
            {
                Title = "ASP.NET Core MVC",
                Description = "Detailed Course With Practical Project"
            };

            CourseResultDto courseResultDto = await _courseService.CreateCourseAsync(courseCreateDto,CourseStatus.Pending);
            Assert.True(courseResultDto.Id != Guid.Empty);
        }
        #endregion

        #region GetCourseByIdAsync

        [Fact]
        public async Task GetCourseByIdAsync_CourseIdNotFound_ReturnNull()
        {
            CourseCreateDto courseCreateDto1 = new CourseCreateDto
            {
                Title = "Test Course 1",
                Description = "Test Course Description 1"
            };
            CourseCreateDto courseCreateDto2 = new CourseCreateDto
            {
                Title = "Test Course 2",
                Description = "Test Course Description 2"
            };

            await _courseService.CreateCourseAsync(courseCreateDto1);
            await _courseService.CreateCourseAsync(courseCreateDto2);
            
            CourseResultDto? courseResultDto = await _courseService.GetCourseByIdAsync(Guid.NewGuid());
            
            Assert.Null(courseResultDto);
        }

        [Fact]
        public async Task GetCourseByIdAsync_CourseIdFound_ReturnValidCourse()
        {
            CourseCreateDto courseCreateDto1 = new CourseCreateDto
            {
                Title = "Test Course 1",
                Description = "Test Course Description 1"
            };
            CourseCreateDto courseCreateDto2 = new CourseCreateDto
            {
                Title = "Test Course 2",
                Description = "Test Course Description 2"
            };

            CourseResultDto? expectedCourseResultDto = await _courseService.CreateCourseAsync(courseCreateDto1);
            await _courseService.CreateCourseAsync(courseCreateDto2);

            CourseResultDto? actualCourseResultDto = await _courseService.GetCourseByIdAsync(expectedCourseResultDto.Id);
            Assert.Equal(expectedCourseResultDto, actualCourseResultDto);
        }


        #endregion

        #region GetCourseByTitleAsync
        [Fact]
        public async Task GetCourseByTitleAsync_NullCourseTitle_ThrowsArgumentNullException()
        {
            await Assert.ThrowsAsync<ArgumentNullException>(async () => await _courseService.GetCourseByTitleAsync(null));
        }

        [Fact]
        public async Task GetCourseByTitleAsync_NotFoundCourseTitle_ReturnNull()
        {
            CourseCreateDto courseCreateDto1 = new CourseCreateDto
            {
                Title = "Test Course 1",
                Description = "Test Course Description 1"
            };
            CourseCreateDto courseCreateDto2 = new CourseCreateDto
            {
                Title = "Test Course 2",
                Description = "Test Course Description 2"
            };

            await _courseService.CreateCourseAsync(courseCreateDto1);
            await _courseService.CreateCourseAsync(courseCreateDto2);

            CourseResultDto? courseResultDto = await _courseService.GetCourseByTitleAsync("InValid Course Title");
            Assert.Null(courseResultDto);
        }

        [Fact]
        public async Task GetCourseByTitleAsync_FoundCourseTitle_ReturnNull()
        {
            CourseCreateDto courseCreateDto1 = new CourseCreateDto
            {
                Title = "Test Course 1",
                Description = "Test Course Description 1"
            };
            CourseCreateDto courseCreateDto2 = new CourseCreateDto
            {
                Title = "Test Course 2",
                Description = "Test Course Description 2"
            };

            CourseResultDto? expectedCourseResultDto = await _courseService.CreateCourseAsync(courseCreateDto1);
            await _courseService.CreateCourseAsync(courseCreateDto2);

            CourseResultDto? actualCourseResultDto = await _courseService.GetCourseByTitleAsync("Test Course 1");
            Assert.Equal(expectedCourseResultDto, actualCourseResultDto);
        }
        #endregion

        #region GetAllCoursesAsync
        [Fact]
        public async Task GetAllCoursesAsync_EmptyCourses_ReturnEmptyList()
        {
            IEnumerable<CourseResultDto> courseResultDtos = await _courseService.GetAllCoursesAsync();
            Assert.Empty(courseResultDtos);
        }

        [Fact]
        public async Task GetAllCoursesAsync_GetCoursesInDataSource_ReturnCoursesList()
        {
            CourseCreateDto courseCreateDto1 = new CourseCreateDto
            {
                Title = "Test Course 1",
                Description = "Test Course Description 1"
            };
            CourseCreateDto courseCreateDto2 = new CourseCreateDto
            {
                Title = "Test Course 2",
                Description = "Test Course Description 2"
            };

            CourseResultDto courseResultDto1 = await _courseService.CreateCourseAsync(courseCreateDto1);
            CourseResultDto courseResultDto2 = await _courseService.CreateCourseAsync(courseCreateDto2);

            IEnumerable<CourseResultDto> expectedCourseList = new List<CourseResultDto>()
            {courseResultDto1,courseResultDto2};

            IEnumerable<CourseResultDto> actualCourseList = await _courseService.GetAllCoursesAsync();
            Assert.True(expectedCourseList.SequenceEqual(actualCourseList));
        }

        #endregion

        #region GetFilteredCoursesAsync

        [Fact]
        public async Task GetFilteredCoursesAsync_NullCourseQueryParameter_ThrowsArgumentNullException()
        {
            await Assert.ThrowsAsync<ArgumentNullException>(async () => await _courseService.GetFilteredCoursesAsync(null));
        }

        [Fact]
        public async Task GetFilteredCoursesAsync_EmptyCourses_ReturnEmptyList()
        {
            var queryDto = new CourseQueryDto
            {
                SearchBy = nameof(CourseResultDto.Title),
                SearchString = "Test",
                SortBy = nameof(CourseResultDto.Title),
                SortOrder = SortOrderOptions.ASC
            };

            var result = await _courseService.GetFilteredCoursesAsync(queryDto);
            Assert.Empty(result);
        }

        [Fact]
        public async Task GetFilteredCoursesAsync_SearchByTitle_ReturnMatchingCourses()
        {
            // Arrange
            var course1 = await _courseService.CreateCourseAsync(new CourseCreateDto
            {
                Title = "C# Programming",
                Description = "Learn C# programming"
            });

            var course2 = await _courseService.CreateCourseAsync(new CourseCreateDto
            {
                Title = "Java Programming",
                Description = "Learn Java programming"
            });

            var course3 = await _courseService.CreateCourseAsync(new CourseCreateDto
            {
                Title = "Python Programming",
                Description = "Learn Python programming"
            });

            var queryDto = new CourseQueryDto
            {
                SearchBy = nameof(CourseResultDto.Title),
                SearchString = "Programming",
                SortBy = nameof(CourseResultDto.Title),
                SortOrder = SortOrderOptions.ASC
            };

            // Act
            var result = await _courseService.GetFilteredCoursesAsync(queryDto);

            // Assert
            Assert.Equal(3, result.Count());
            Assert.Contains(result, c => c.Title == "C# Programming");
            Assert.Contains(result, c => c.Title == "Java Programming");
            Assert.Contains(result, c => c.Title == "Python Programming");
        }

        [Fact]
        public async Task GetFilteredCoursesAsync_SearchByDescription_ReturnMatchingCourses()
        {
            // Arrange
            var course1 = await _courseService.CreateCourseAsync(new CourseCreateDto
            {
                Title = "Web Development",
                Description = "Learn web development with ASP.NET"
            });

            var course2 = await _courseService.CreateCourseAsync(new CourseCreateDto
            {
                Title = "Mobile Development",
                Description = "Learn mobile development with Flutter"
            });

            var course3 = await _courseService.CreateCourseAsync(new CourseCreateDto
            {
                Title = "Database Design",
                Description = "Learn database design principles"
            });

            var queryDto = new CourseQueryDto
            {
                SearchBy = nameof(CourseResultDto.Description),
                SearchString = "development",
                SortBy = nameof(CourseResultDto.Title),
                SortOrder = SortOrderOptions.ASC
            };

            // Act
            var result = await _courseService.GetFilteredCoursesAsync(queryDto);

            // Assert
            Assert.Equal(2, result.Count());
            Assert.Contains(result, c => c.Title == "Web Development");
            Assert.Contains(result, c => c.Title == "Mobile Development");
        }

        [Fact]
        public async Task GetFilteredCoursesAsync_SearchByStatus_ReturnMatchingCourses()
        {
            // Arrange
            var course1 = await _courseService.CreateCourseAsync(new CourseCreateDto
            {
                Title = "Approved Course",
                Description = "This course is approved"
            }, CourseStatus.Approved);

            var course2 = await _courseService.CreateCourseAsync(new CourseCreateDto
            {
                Title = "Pending Course",
                Description = "This course is pending"
            }, CourseStatus.Pending);

            var course3 = await _courseService.CreateCourseAsync(new CourseCreateDto
            {
                Title = "Rejected Course",
                Description = "This course is rejected"
            }, CourseStatus.Rejected);

            var queryDto = new CourseQueryDto
            {
                SearchBy = nameof(CourseResultDto.Status),
                SearchString = CourseStatus.Approved.ToString(),
                SortBy = nameof(CourseResultDto.Title),
                SortOrder = SortOrderOptions.ASC
            };

            // Act
            var result = await _courseService.GetFilteredCoursesAsync(queryDto);

            // Assert
            Assert.Single(result);
            Assert.Equal("Approved Course", result.First().Title);
        }

        [Fact]
        public async Task GetFilteredCoursesAsync_SortByTitleAscending_ReturnSortedCourses()
        {
            // Arrange
            var course1 = await _courseService.CreateCourseAsync(new CourseCreateDto
            {
                Title = "Zebra Course",
                Description = "Zebra course description"
            });

            var course2 = await _courseService.CreateCourseAsync(new CourseCreateDto
            {
                Title = "Alpha Course",
                Description = "Alpha course description"
            });

            var course3 = await _courseService.CreateCourseAsync(new CourseCreateDto
            {
                Title = "Beta Course",
                Description = "Beta course description"
            });

            var queryDto = new CourseQueryDto
            {
                SearchBy = nameof(CourseResultDto.Title),
                SearchString = "",
                SortBy = nameof(CourseResultDto.Title),
                SortOrder = SortOrderOptions.ASC
            };

            // Act
            var result = await _courseService.GetFilteredCoursesAsync(queryDto);

            // Assert
            var resultList = result.ToList();
            Assert.Equal(3, resultList.Count);
            Assert.Equal("Alpha Course", resultList[0].Title);
            Assert.Equal("Beta Course", resultList[1].Title);
            Assert.Equal("Zebra Course", resultList[2].Title);
        }

        [Fact]
        public async Task GetFilteredCoursesAsync_SortByTitleDescending_ReturnSortedCourses()
        {
            // Arrange
            var course1 = await _courseService.CreateCourseAsync(new CourseCreateDto
            {
                Title = "Alpha Course",
                Description = "Alpha course description"
            });

            var course2 = await _courseService.CreateCourseAsync(new CourseCreateDto
            {
                Title = "Beta Course",
                Description = "Beta course description"
            });

            var course3 = await _courseService.CreateCourseAsync(new CourseCreateDto
            {
                Title = "Zebra Course",
                Description = "Zebra course description"
            });

            var queryDto = new CourseQueryDto
            {
                SearchBy = nameof(CourseResultDto.Title),
                SearchString = "",
                SortBy = nameof(CourseResultDto.Title),
                SortOrder = SortOrderOptions.DESC
            };

            // Act
            var result = await _courseService.GetFilteredCoursesAsync(queryDto);

            // Assert
            var resultList = result.ToList();
            Assert.Equal(3, resultList.Count);
            Assert.Equal("Zebra Course", resultList[0].Title);
            Assert.Equal("Beta Course", resultList[1].Title);
            Assert.Equal("Alpha Course", resultList[2].Title);
        }

        [Fact]
        public async Task GetFilteredCoursesAsync_SortByCreatedAtAscending_ReturnSortedCourses()
        {
            // Arrange
            var course1 = await _courseService.CreateCourseAsync(new CourseCreateDto
            {
                Title = "First Course",
                Description = "First course description"
            });

            await Task.Delay(100); // Ensure different timestamps

            var course2 = await _courseService.CreateCourseAsync(new CourseCreateDto
            {
                Title = "Second Course",
                Description = "Second course description"
            });

            await Task.Delay(100); // Ensure different timestamps

            var course3 = await _courseService.CreateCourseAsync(new CourseCreateDto
            {
                Title = "Third Course",
                Description = "Third course description"
            });

            var queryDto = new CourseQueryDto
            {
                SearchBy = nameof(CourseResultDto.Title),
                SearchString = "",
                SortBy = nameof(CourseResultDto.CreatedAt),
                SortOrder = SortOrderOptions.ASC
            };

            // Act
            var result = await _courseService.GetFilteredCoursesAsync(queryDto);

            // Assert
            var resultList = result.ToList();
            Assert.Equal(3, resultList.Count);
            Assert.Equal("First Course", resultList[0].Title);
            Assert.Equal("Second Course", resultList[1].Title);
            Assert.Equal("Third Course", resultList[2].Title);
        }

        [Fact]
        public async Task GetFilteredCoursesAsync_SearchAndSortCombined_ReturnFilteredAndSortedCourses()
        {
            // Arrange
            var course1 = await _courseService.CreateCourseAsync(new CourseCreateDto
            {
                Title = "Advanced C# Programming",
                Description = "Advanced C# course"
            });

            var course2 = await _courseService.CreateCourseAsync(new CourseCreateDto
            {
                Title = "Basic C# Programming",
                Description = "Basic C# course"
            });

            var course3 = await _courseService.CreateCourseAsync(new CourseCreateDto
            {
                Title = "Java Programming",
                Description = "Java course"
            });

            var queryDto = new CourseQueryDto
            {
                SearchBy = nameof(CourseResultDto.Title),
                SearchString = "C#",
                SortBy = nameof(CourseResultDto.Title),
                SortOrder = SortOrderOptions.ASC
            };

            // Act
            var result = await _courseService.GetFilteredCoursesAsync(queryDto);

            // Assert
            var resultList = result.ToList();
            Assert.Equal(2, resultList.Count);
            Assert.Equal("Advanced C# Programming", resultList[0].Title);
            Assert.Equal("Basic C# Programming", resultList[1].Title);
        }

        [Fact]
        public async Task GetFilteredCoursesAsync_EmptySearchString_ReturnAllCourses()
        {
            // Arrange
            var course1 = await _courseService.CreateCourseAsync(new CourseCreateDto
            {
                Title = "Course 1",
                Description = "Description 1"
            });

            var course2 = await _courseService.CreateCourseAsync(new CourseCreateDto
            {
                Title = "Course 2",
                Description = "Description 2"
            });

            var queryDto = new CourseQueryDto
            {
                SearchBy = nameof(CourseResultDto.Title),
                SearchString = "",
                SortBy = nameof(CourseResultDto.Title),
                SortOrder = SortOrderOptions.ASC
            };

            // Act
            var result = await _courseService.GetFilteredCoursesAsync(queryDto);

            // Assert
            Assert.Equal(2, result.Count());
        }

        [Fact]
        public async Task GetFilteredCoursesAsync_NoMatchingSearch_ReturnEmptyList()
        {
            // Arrange
            var course1 = await _courseService.CreateCourseAsync(new CourseCreateDto
            {
                Title = "C# Programming",
                Description = "C# course"
            });

            var course2 = await _courseService.CreateCourseAsync(new CourseCreateDto
            {
                Title = "Java Programming",
                Description = "Java course"
            });

            var queryDto = new CourseQueryDto
            {
                SearchBy = nameof(CourseResultDto.Title),
                SearchString = "Python",
                SortBy = nameof(CourseResultDto.Title),
                SortOrder = SortOrderOptions.ASC
            };

            // Act
            var result = await _courseService.GetFilteredCoursesAsync(queryDto);

            // Assert
            Assert.Empty(result);
        }

        [Fact]
        public async Task GetFilteredCoursesAsync_CaseInsensitiveSearch_ReturnMatchingCourses()
        {
            // Arrange
            var course1 = await _courseService.CreateCourseAsync(new CourseCreateDto
            {
                Title = "C# Programming",
                Description = "C# course"
            });

            var course2 = await _courseService.CreateCourseAsync(new CourseCreateDto
            {
                Title = "Java Programming",
                Description = "Java course"
            });

            var queryDto = new CourseQueryDto
            {
                SearchBy = nameof(CourseResultDto.Title),
                SearchString = "c#",
                SortBy = nameof(CourseResultDto.Title),
                SortOrder = SortOrderOptions.ASC
            };

            // Act
            var result = await _courseService.GetFilteredCoursesAsync(queryDto);

            // Assert
            Assert.Single(result);
            Assert.Equal("C# Programming", result.First().Title);
        }

        [Fact]
        public async Task GetFilteredCoursesAsync_InvalidSortBy_ReturnDefaultSortedCourses()
        {
            // Arrange
            var course1 = await _courseService.CreateCourseAsync(new CourseCreateDto
            {
                Title = "Zebra Course",
                Description = "Zebra course"
            });

            var course2 = await _courseService.CreateCourseAsync(new CourseCreateDto
            {
                Title = "Alpha Course",
                Description = "Alpha course"
            });

            var queryDto = new CourseQueryDto
            {
                SearchBy = nameof(CourseResultDto.Title),
                SearchString = "",
                SortBy = "InvalidProperty",
                SortOrder = SortOrderOptions.ASC
            };

            // Act
            var result = await _courseService.GetFilteredCoursesAsync(queryDto);

            // Assert
            Assert.Equal(2, result.Count());
        }

        [Fact]
        public async Task GetFilteredCoursesAsync_InvalidSortOrder_ReturnDefaultSortedCourses()
        {
            // Arrange
            var course1 = await _courseService.CreateCourseAsync(new CourseCreateDto
            {
                Title = "Zebra Course",
                Description = "Zebra course"
            });

            var course2 = await _courseService.CreateCourseAsync(new CourseCreateDto
            {
                Title = "Alpha Course",
                Description = "Alpha course"
            });

            var queryDto = new CourseQueryDto
            {
                SearchBy = nameof(CourseResultDto.Title),
                SearchString = "",
                SortBy = nameof(CourseResultDto.Title),
                SortOrder = "INVALID"
            };

            // Act
            var result = await _courseService.GetFilteredCoursesAsync(queryDto);

            // Assert
            Assert.Equal(2, result.Count());
        }

        #endregion

        #region UpdateCourseAsync

        [Fact]
        public async Task UpdateCourseAsync_NullCourseUpdateDto_ThrowsArgumentNullException()
        {
            await Assert.ThrowsAsync<ArgumentNullException>(async () => await _courseService.UpdateCourseAsync(null));
        }

        [Fact]
        public async Task UpdateCourseAsync_NonExistentCourseId_ThrowsInvalidOperationException()
        {
            var courseUpdateDto = new CourseUpdateDto
            {
                Id = Guid.NewGuid(),
                Title = "Updated Course",
                Description = "Updated Description",
                Status = CourseStatus.Approved.ToString()
            };

            await Assert.ThrowsAsync<InvalidOperationException>(async () => await _courseService.UpdateCourseAsync(courseUpdateDto));
        }

        [Fact]
        public async Task UpdateCourseAsync_NullTitle_ThrowsArgumentException()
        {
            // First create a course
            var courseCreateDto = new CourseCreateDto
            {
                Title = "Original Course",
                Description = "Original Description"
            };
            var createdCourse = await _courseService.CreateCourseAsync(courseCreateDto);

            var courseUpdateDto = new CourseUpdateDto
            {
                Id = createdCourse.Id,
                Title = null,
                Description = "Updated Description",
                Status = CourseStatus.Approved.ToString()
            };

            await Assert.ThrowsAsync<ArgumentException>(async () => await _courseService.UpdateCourseAsync(courseUpdateDto));
        }

        [Fact]
        public async Task UpdateCourseAsync_EmptyTitle_ThrowsArgumentException()
        {
            // First create a course
            var courseCreateDto = new CourseCreateDto
            {
                Title = "Original Course",
                Description = "Original Description"
            };
            var createdCourse = await _courseService.CreateCourseAsync(courseCreateDto);

            var courseUpdateDto = new CourseUpdateDto
            {
                Id = createdCourse.Id,
                Title = "",
                Description = "Updated Description",
                Status = CourseStatus.Approved.ToString()
            };

            await Assert.ThrowsAsync<ArgumentException>(async () => await _courseService.UpdateCourseAsync(courseUpdateDto));
        }

        [Fact]
        public async Task UpdateCourseAsync_WhitespaceTitle_ThrowsArgumentException()
        {
            // First create a course
            var courseCreateDto = new CourseCreateDto
            {
                Title = "Original Course",
                Description = "Original Description"
            };
            var createdCourse = await _courseService.CreateCourseAsync(courseCreateDto);

            var courseUpdateDto = new CourseUpdateDto
            {
                Id = createdCourse.Id,
                Title = "   ",
                Description = "Updated Description",
                Status = CourseStatus.Approved.ToString()
            };

            await Assert.ThrowsAsync<ArgumentException>(async () => await _courseService.UpdateCourseAsync(courseUpdateDto));
        }

        [Fact]
        public async Task UpdateCourseAsync_InvalidStatus_ThrowsArgumentException()
        {
            // First create a course
            var courseCreateDto = new CourseCreateDto
            {
                Title = "Original Course",
                Description = "Original Description"
            };
            var createdCourse = await _courseService.CreateCourseAsync(courseCreateDto);

            var courseUpdateDto = new CourseUpdateDto
            {
                Id = createdCourse.Id,
                Title = "Updated Course",
                Description = "Updated Description",
                Status = "InvalidStatus"
            };

            await Assert.ThrowsAsync<ArgumentException>(async () => await _courseService.UpdateCourseAsync(courseUpdateDto));
        }

        [Fact]
        public async Task UpdateCourseAsync_ValidUpdate_ReturnUpdatedCourse()
        {
            // First create a course
            var courseCreateDto = new CourseCreateDto
            {
                Title = "Original Course",
                Description = "Original Description"
            };
            var createdCourse = await _courseService.CreateCourseAsync(courseCreateDto);

            var courseUpdateDto = new CourseUpdateDto
            {
                Id = createdCourse.Id,
                Title = "Updated Course",
                Description = "Updated Description",
                Status = CourseStatus.Approved.ToString()
            };

            var updatedCourse = await _courseService.UpdateCourseAsync(courseUpdateDto);

            Assert.Equal(createdCourse.Id, updatedCourse.Id); // ID should not change
            Assert.Equal("Updated Course", updatedCourse.Title);
            Assert.Equal("Updated Description", updatedCourse.Description);
            Assert.Equal(CourseStatus.Approved, updatedCourse.Status);
            Assert.Equal(createdCourse.CreatedAt, updatedCourse.CreatedAt); // CreatedAt should not change
        }

        [Fact]
        public async Task UpdateCourseAsync_UpdateOnlyTitle_ReturnPartiallyUpdatedCourse()
        {
            // First create a course
            var courseCreateDto = new CourseCreateDto
            {
                Title = "Original Course",
                Description = "Original Description"
            };
            var createdCourse = await _courseService.CreateCourseAsync(courseCreateDto);

            var courseUpdateDto = new CourseUpdateDto
            {
                Id = createdCourse.Id,
                Title = "Updated Course Title",
                Description = "Original Description", // Same as original
                Status = createdCourse.Status.ToString() // Same as original
            };

            var updatedCourse = await _courseService.UpdateCourseAsync(courseUpdateDto);

            Assert.Equal(createdCourse.Id, updatedCourse.Id);
            Assert.Equal("Updated Course Title", updatedCourse.Title);
            Assert.Equal("Original Description", updatedCourse.Description);
            Assert.Equal(createdCourse.Status, updatedCourse.Status);
            Assert.Equal(createdCourse.CreatedAt, updatedCourse.CreatedAt);
        }

        [Fact]
        public async Task UpdateCourseAsync_UpdateOnlyDescription_ReturnPartiallyUpdatedCourse()
        {
            // First create a course
            var courseCreateDto = new CourseCreateDto
            {
                Title = "Original Course",
                Description = "Original Description"
            };
            var createdCourse = await _courseService.CreateCourseAsync(courseCreateDto);

            var courseUpdateDto = new CourseUpdateDto
            {
                Id = createdCourse.Id,
                Title = "Original Course", // Same as original
                Description = "Updated Description",
                Status = createdCourse.Status.ToString() // Same as original
            };

            var updatedCourse = await _courseService.UpdateCourseAsync(courseUpdateDto);

            Assert.Equal(createdCourse.Id, updatedCourse.Id);
            Assert.Equal("Original Course", updatedCourse.Title);
            Assert.Equal("Updated Description", updatedCourse.Description);
            Assert.Equal(createdCourse.Status, updatedCourse.Status);
            Assert.Equal(createdCourse.CreatedAt, updatedCourse.CreatedAt);
        }

        [Fact]
        public async Task UpdateCourseAsync_UpdateOnlyStatus_ReturnPartiallyUpdatedCourse()
        {
            // First create a course
            var courseCreateDto = new CourseCreateDto
            {
                Title = "Original Course",
                Description = "Original Description"
            };
            var createdCourse = await _courseService.CreateCourseAsync(courseCreateDto);

            var courseUpdateDto = new CourseUpdateDto
            {
                Id = createdCourse.Id,
                Title = "Original Course", // Same as original
                Description = "Original Description", // Same as original
                Status = CourseStatus.Rejected.ToString()
            };

            var updatedCourse = await _courseService.UpdateCourseAsync(courseUpdateDto);

            Assert.Equal(createdCourse.Id, updatedCourse.Id);
            Assert.Equal("Original Course", updatedCourse.Title);
            Assert.Equal("Original Description", updatedCourse.Description);
            Assert.Equal(CourseStatus.Rejected, updatedCourse.Status);
            Assert.Equal(createdCourse.CreatedAt, updatedCourse.CreatedAt);
        }

        [Fact]
        public async Task UpdateCourseAsync_UpdateAllFields_ReturnFullyUpdatedCourse()
        {
            // First create a course
            var courseCreateDto = new CourseCreateDto
            {
                Title = "Original Course",
                Description = "Original Description"
            };
            var createdCourse = await _courseService.CreateCourseAsync(courseCreateDto);

            var courseUpdateDto = new CourseUpdateDto
            {
                Id = createdCourse.Id,
                Title = "Completely Updated Course",
                Description = "Completely Updated Description",
                Status = CourseStatus.Approved.ToString()
            };

            var updatedCourse = await _courseService.UpdateCourseAsync(courseUpdateDto);

            Assert.Equal(createdCourse.Id, updatedCourse.Id);
            Assert.Equal("Completely Updated Course", updatedCourse.Title);
            Assert.Equal("Completely Updated Description", updatedCourse.Description);
            Assert.Equal(CourseStatus.Approved, updatedCourse.Status);
            Assert.Equal(createdCourse.CreatedAt, updatedCourse.CreatedAt);
        }

        [Fact]
        public async Task UpdateCourseAsync_UpdateWithSameValues_ReturnUnchangedCourse()
        {
            // First create a course
            var courseCreateDto = new CourseCreateDto
            {
                Title = "Original Course",
                Description = "Original Description"
            };
            var createdCourse = await _courseService.CreateCourseAsync(courseCreateDto);

            var courseUpdateDto = new CourseUpdateDto
            {
                Id = createdCourse.Id,
                Title = "Original Course", // Same as original
                Description = "Original Description", // Same as original
                Status = createdCourse.Status.ToString() // Same as original
            };

            var updatedCourse = await _courseService.UpdateCourseAsync(courseUpdateDto);

            Assert.Equal(createdCourse.Id, updatedCourse.Id);
            Assert.Equal(createdCourse.Title, updatedCourse.Title);
            Assert.Equal(createdCourse.Description, updatedCourse.Description);
            Assert.Equal(createdCourse.Status, updatedCourse.Status);
            Assert.Equal(createdCourse.CreatedAt, updatedCourse.CreatedAt);
        }
        #endregion

        #region DeleteCourseAsync

        [Fact]
        public async Task DeleteCourseAsync_EmptyGuid_ThrowsArgumentException()
        {
            await Assert.ThrowsAsync<ArgumentException>(async () => await _courseService.DeleteCourseAsync(Guid.Empty));
        }

        [Fact]
        public async Task DeleteCourseAsync_NonExistentCourseId_ReturnsFalse()
        {
            // Arrange - Create some courses
            var course1 = await _courseService.CreateCourseAsync(new CourseCreateDto
            {
                Title = "Test Course 1",
                Description = "Test Description 1"
            });

            var course2 = await _courseService.CreateCourseAsync(new CourseCreateDto
            {
                Title = "Test Course 2",
                Description = "Test Description 2"
            });

            // Act - Try to delete a non-existent course
            bool result = await _courseService.DeleteCourseAsync(Guid.NewGuid());

            // Assert
            Assert.False(result);
        }

        [Fact]
        public async Task DeleteCourseAsync_ExistingCourseId_ReturnsTrue()
        {
            // Arrange - Create a course
            var courseCreateDto = new CourseCreateDto
            {
                Title = "Course to Delete",
                Description = "This course will be deleted"
            };
            var createdCourse = await _courseService.CreateCourseAsync(courseCreateDto);

            // Act - Delete the course
            bool result = await _courseService.DeleteCourseAsync(createdCourse.Id);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public async Task DeleteCourseAsync_ExistingCourseId_CourseIsRemovedFromDataSource()
        {
            // Arrange - Create a course
            var courseCreateDto = new CourseCreateDto
            {
                Title = "Course to Delete",
                Description = "This course will be deleted"
            };
            var createdCourse = await _courseService.CreateCourseAsync(courseCreateDto);

            // Verify course exists before deletion
            var courseBeforeDelete = await _courseService.GetCourseByIdAsync(createdCourse.Id);
            Assert.NotNull(courseBeforeDelete);

            // Act - Delete the course
            bool result = await _courseService.DeleteCourseAsync(createdCourse.Id);

            // Assert
            Assert.True(result);
            
            // Verify course no longer exists
            var courseAfterDelete = await _courseService.GetCourseByIdAsync(createdCourse.Id);
            Assert.Null(courseAfterDelete);
        }

        [Fact]
        public async Task DeleteCourseAsync_ExistingCourseId_OtherCoursesRemainUnchanged()
        {
            // Arrange - Create multiple courses
            var course1 = await _courseService.CreateCourseAsync(new CourseCreateDto
            {
                Title = "Course 1",
                Description = "Description 1"
            });

            var course2 = await _courseService.CreateCourseAsync(new CourseCreateDto
            {
                Title = "Course 2",
                Description = "Description 2"
            });

            var course3 = await _courseService.CreateCourseAsync(new CourseCreateDto
            {
                Title = "Course 3",
                Description = "Description 3"
            });

            // Act - Delete only course2
            bool result = await _courseService.DeleteCourseAsync(course2.Id);

            // Assert
            Assert.True(result);

            // Verify course1 still exists
            var remainingCourse1 = await _courseService.GetCourseByIdAsync(course1.Id);
            Assert.NotNull(remainingCourse1);
            Assert.Equal("Course 1", remainingCourse1.Title);

            // Verify course2 is deleted
            var deletedCourse2 = await _courseService.GetCourseByIdAsync(course2.Id);
            Assert.Null(deletedCourse2);

            // Verify course3 still exists
            var remainingCourse3 = await _courseService.GetCourseByIdAsync(course3.Id);
            Assert.NotNull(remainingCourse3);
            Assert.Equal("Course 3", remainingCourse3.Title);
        }

        [Fact]
        public async Task DeleteCourseAsync_AllCourses_ReturnsTrueForEach()
        {
            // Arrange - Create multiple courses
            var course1 = await _courseService.CreateCourseAsync(new CourseCreateDto
            {
                Title = "Course 1",
                Description = "Description 1"
            });

            var course2 = await _courseService.CreateCourseAsync(new CourseCreateDto
            {
                Title = "Course 2",
                Description = "Description 2"
            });

            var course3 = await _courseService.CreateCourseAsync(new CourseCreateDto
            {
                Title = "Course 3",
                Description = "Description 3"
            });

            // Act - Delete all courses
            bool result1 = await _courseService.DeleteCourseAsync(course1.Id);
            bool result2 = await _courseService.DeleteCourseAsync(course2.Id);
            bool result3 = await _courseService.DeleteCourseAsync(course3.Id);

            // Assert
            Assert.True(result1);
            Assert.True(result2);
            Assert.True(result3);

            // Verify all courses are deleted
            var allCourses = await _courseService.GetAllCoursesAsync();
            Assert.Empty(allCourses);
        }

        [Fact]
        public async Task DeleteCourseAsync_DeleteSameCourseTwice_FirstReturnsTrueSecondReturnsFalse()
        {
            // Arrange - Create a course
            var courseCreateDto = new CourseCreateDto
            {
                Title = "Course to Delete Twice",
                Description = "This course will be deleted twice"
            };
            var createdCourse = await _courseService.CreateCourseAsync(courseCreateDto);

            // Act - Delete the course twice
            bool firstDeleteResult = await _courseService.DeleteCourseAsync(createdCourse.Id);
            bool secondDeleteResult = await _courseService.DeleteCourseAsync(createdCourse.Id);

            // Assert
            Assert.True(firstDeleteResult);
            Assert.False(secondDeleteResult);
        }

        [Fact]
        public async Task DeleteCourseAsync_EmptyDataSource_ReturnsFalse()
        {
            // Act - Try to delete from empty data source
            bool result = await _courseService.DeleteCourseAsync(Guid.NewGuid());

            // Assert
            Assert.False(result);
        }

        [Fact]
        public async Task DeleteCourseAsync_ValidGuid_DoesNotThrowException()
        {
            // Arrange - Create a course
            var courseCreateDto = new CourseCreateDto
            {
                Title = "Course for Exception Test",
                Description = "Testing that no exception is thrown"
            };
            var createdCourse = await _courseService.CreateCourseAsync(courseCreateDto);

            // Act & Assert - Should not throw any exception
            var exception = await Record.ExceptionAsync(async () => await _courseService.DeleteCourseAsync(createdCourse.Id));
            Assert.Null(exception);
        }

        [Fact]
        public async Task DeleteCourseAsync_AfterDelete_GetAllCoursesReturnsCorrectCount()
        {
            // Arrange - Create multiple courses
            var course1 = await _courseService.CreateCourseAsync(new CourseCreateDto
            {
                Title = "Course 1",
                Description = "Description 1"
            });

            var course2 = await _courseService.CreateCourseAsync(new CourseCreateDto
            {
                Title = "Course 2",
                Description = "Description 2"
            });

            var course3 = await _courseService.CreateCourseAsync(new CourseCreateDto
            {
                Title = "Course 3",
                Description = "Description 3"
            });

            // Verify initial count
            var initialCourses = await _courseService.GetAllCoursesAsync();
            Assert.Equal(3, initialCourses.Count());

            // Act - Delete one course
            await _courseService.DeleteCourseAsync(course2.Id);

            // Assert - Verify count is reduced by 1
            var remainingCourses = await _courseService.GetAllCoursesAsync();
            Assert.Equal(2, remainingCourses.Count());
        }

        [Fact]
        public async Task DeleteCourseAsync_DeleteAndRecreate_WorksCorrectly()
        {
            // Arrange - Create a course
            var courseCreateDto = new CourseCreateDto
            {
                Title = "Original Course",
                Description = "Original Description"
            };
            var originalCourse = await _courseService.CreateCourseAsync(courseCreateDto);

            // Act - Delete the course
            bool deleteResult = await _courseService.DeleteCourseAsync(originalCourse.Id);

            // Assert - Verify deletion was successful
            Assert.True(deleteResult);

            // Act - Create a new course with same title
            var newCourseCreateDto = new CourseCreateDto
            {
                Title = "Original Course", // Same title
                Description = "New Description"
            };
            var newCourse = await _courseService.CreateCourseAsync(newCourseCreateDto);

            // Assert - Verify new course was created successfully
            Assert.NotNull(newCourse);
            Assert.NotEqual(originalCourse.Id, newCourse.Id); // Should have different ID
            Assert.Equal("Original Course", newCourse.Title);
            Assert.Equal("New Description", newCourse.Description);
        }

        #endregion
    }
}
