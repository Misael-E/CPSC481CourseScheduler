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

        public int compare(Semester other)
        {
            return other.SemesterDisplayName.CompareTo(this.SemesterDisplayName);
        }
    }
}
