namespace CPSC481CourseScheduler.Data
{
	public class Semester
	{
		public string Season { get; set; } = "";
		public string Year { get; set; } = "";

		public string SemesterDisplayName { get; set; } = "";

		public Semester(string season, string year)
		{
			this.Season = season;
			this.Year = year;
			this.SemesterDisplayName = season + " " + year;
		}

		public Semester(string semesterDisplayName)
		{
			this.Season = semesterDisplayName.Substring(0, semesterDisplayName.IndexOf(' '));
			this.Year = semesterDisplayName.Substring(semesterDisplayName.IndexOf(" ") + 1);
			this.SemesterDisplayName = semesterDisplayName;
		}

		public int compare(Semester other)
		{
			return other.SemesterDisplayName.CompareTo(this.SemesterDisplayName);
		}

		public override string ToString()
		{
			return SemesterDisplayName;
		}
	}
}
