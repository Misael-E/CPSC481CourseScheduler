﻿namespace CPSC481CourseScheduler.Data
{
	public class Course
	{
		public string CourseName { get; set; } = "";
		public string CourseCode { get; set; } = "";
		public string? Description { get; set; }
		public string? Prereq { get; set; }
		public string? LectureLocation { get; set; }
		public string? InstructorName { get; set; }
		public string? CourseTime { get; set; }
		public string? Seats { get; set; }
		public string? Days { get; set; }
		public string? LectureNumber { get; set; }
		public string? Map { get; set; }
		public string? Status { get; set; }
	}
}