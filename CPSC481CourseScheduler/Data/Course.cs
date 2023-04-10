namespace CPSC481CourseScheduler.Data
{
	public class Course
	{
		public string CourseName { get; set; } = "";
		public string CourseCode { get; set; } = "";
		public string CourseColor { get; set; } = string.Empty;
		public string? Description { get; set; }
		public string? Prereq { get; set; }
		public string? LectureLocation { get; set; }
		public string? InstructorName { get; set; }
		public string? CourseTime { get; set; }
		public TimeSpan StartTime { get; set; }
		public TimeSpan EndTime { get; set; }
		public string? Seats { get; set; } // DEPRECATED

		public int? SeatsTaken { get; set; }

		public int? TotalSeats { get; set; }
		public string? Days { get; set; }
		public List<DayOfWeek> DaysOfWeek { get; set; }
		public string? LectureNumber { get; set; }
		public string? Map { get; set; }
		public string? IndividualMap { get; set; }
		public string? Status { get; set; }

		public bool dropCourse { get; set; } = false;
		public void enroll()
		{
			this.Status = "Enrolled";
			if (this.SeatsTaken != null)
			{
				this.SeatsTaken++;
			}
		}

		public void waitlist()
		{
			this.Status = "Waitlisted";
		}

		public void drop()
		{
			this.Status = "Not enrolled";
			if (this.SeatsTaken != null)
			{
				this.SeatsTaken--;
			}
		}

	}



}
