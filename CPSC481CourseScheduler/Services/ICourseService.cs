using System.Threading.Tasks;
using System.Collections.Generic;
using System;
using CPSC481CourseScheduler.Data;

namespace CPSC481CourseScheduler.Services
{
    public interface ICourseService
    {
        Task AddToSelectedCourses(Course course);
        Task AddToBookmarks(Course course);
        Task RemoveFromSelectedCourses(Course course);
        Task RemoveFromBookmarks(Course course);
        List<Course> GetSelectedCourses();
        List<Course> GetBookmarks();
        List<Course> GetAllCourses();
        List<Course> GetAllFriendCourses();

        public event EventHandler<List<Course>> OnSelectedCoursesChanged;
        public event EventHandler<List<Course>> OnBookmarksChanged;
    }
}
