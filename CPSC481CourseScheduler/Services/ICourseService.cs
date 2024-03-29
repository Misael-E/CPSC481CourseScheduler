﻿using System.Threading.Tasks;
using System.Collections.Generic;
using System;
using CPSC481CourseScheduler.Data;

namespace CPSC481CourseScheduler.Services
{

    public interface ICourseService
    {
        Task<int> AddToSelectedCourses(Course course);
        Task<int> setSelectedCourses(List<Course> courses);
        Task<int> AddToBookmarks(Course course);
        Task SelectSemester(Semester semester);
        Task RemoveFromSelectedCourses(Course course);
        Task RemoveFromBookmarks(Course course);
        List<Course> GetSelectedCourses();
        List<Course> GetBookmarks();
        List<Course> GetAllCourses();
        Task KeepCourse(Course course);

        List<Course> GetCoursesToDrop();

        List<Semester> GetAllSemesters();

        List<Course> GetCoursesBySemester();

        Semester GetSelectedSemester();

        List<Course> GetRecommendedCourses();

        List<Course> GetSearchedCourses();
        void FinalizedSchedule();

        public event EventHandler<List<Course>> OnSelectedCoursesChanged;
        public event EventHandler<List<Course>> OnBookmarksChanged;
        public event EventHandler<Semester> OnSelectedSemesterChanged;
    }
}
