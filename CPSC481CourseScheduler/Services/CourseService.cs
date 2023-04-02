

using CPSC481CourseScheduler.Data;

namespace CPSC481CourseScheduler.Services
{
	public class CourseService : ICourseService
	{

		List<Course> SelectedCourses { get; set; } = new List<Course>();
		List<Course> Bookmarks { get; set; } = new List<Course>();
		

		public async Task AddToSelectedCourses(Course course)
		{
			bool duplicate = false;

			foreach (var crs in SelectedCourses)
			{
				if (crs.CourseCode == course.CourseCode)
				{
					duplicate = true;
					break;
				}
			}

			if (!duplicate && SelectedCourses.Count < 7)
			{
				await Task.Factory.StartNew(() => SelectedCourses.Add(course));
				OnSelectedCoursesChanged?.Invoke(this, SelectedCourses);
			}
		}

		public async Task AddToBookmarks(Course course)
		{
			bool duplicate = false;

			foreach (var crs in Bookmarks)
			{
				if (crs.CourseCode == course.CourseCode)
				{
					duplicate = true;
					break;
				}
			}

			if (!duplicate)
			{
				await Task.Factory.StartNew(() => Bookmarks.Add(course));
				OnBookmarksChanged?.Invoke(this, Bookmarks);
			}
		}


		public event EventHandler<List<Course>> OnSelectedCoursesChanged;
		public event EventHandler<List<Course>> OnBookmarksChanged;

		public List<Course> GetSelectedCourses() => SelectedCourses;

		public List<Course> GetBookmarks() => Bookmarks;
		public List<Course> GetAllCourses() => AllCourses;

		public async Task RemoveFromSelectedCourses(Course course)
		{

				await Task.Factory.StartNew(() => SelectedCourses.Remove(course));
				OnSelectedCoursesChanged?.Invoke(this, SelectedCourses);
		}

		public async Task RemoveFromBookmarks(Course course)
		{
			await Task.Factory.StartNew(() => Bookmarks.Remove(course));
			OnBookmarksChanged?.Invoke(this, Bookmarks);
		}

		List<Course> AllCourses { get; set; } = new List<Course>
		{
			new Course
			{
				CourseName = "Introduction to Sociology",
				CourseCode = "SOCI 201",
				Description = "Sociology as a discipline examines how the society in which we live influences our thinking and behaviour. An introduction to sociology through the study of society, social institutions, group behaviour and social change.",
				Prereq = "None",
				LectureLocation = "ST 140",
				LectureNumber = "Lec 01",
				InstructorName = "Dr Ehud Sharlin",
				Days = "MWF",
				CourseTime = "10:00 - 10:50",
				Seats = "45/90",
				Map = "/MapImages/ST.png",
				Status = "Enrolled"
			},
			new Course
			{
				CourseName = "Design and Implementation of Database Systems",
				CourseCode = "CPSC 571",
				Description = "Implementation and design of modern database systems including query modification/optimization, recovery, concurrency, integrity, and distribution.",
				Prereq = "Computer Science 471",
				LectureLocation = "MS 217",
				LectureNumber = "Lec 01",
				InstructorName = "Tamer Jarada",
				Days = "TR",
				CourseTime = "14:00 - 15:15",
				Seats = "40/90",
				Map = "/MapImages/MS.png",
				Status = "Enrolled"
			},
			new Course
			{
				CourseName = "Web-Based Systems",
				CourseCode = "SENG 513",
				Description ="An overview of software engineering methods and technologies for developing web-based software systems.",
				Prereq = "3 units from Software Engineering 300, 301 or Software Engineering for Engineers 480",
				LectureLocation = "ICT 122",
				LectureNumber = "Lec 01",
				InstructorName = "Ahmad Nasri",
				Days = "TR",
				CourseTime = "12:30 - 13:45",
				Seats = "20/50",
				Map = "/MapImages/ICT.png",
				Status = "Not-Enrolled"
			},
			new Course
			{
				CourseName = "Introduction to Information Visualization",
				CourseCode = "CPSC 583",
				Description ="Principles of information representation, presentation and interaction. Development of mappings from data to visual structures and exploration, navigation, cues, distortion and emphasis techniques.",
				Prereq = "3 units from Computer Science 319, 331 or Data Science 311.",
				LectureLocation = "SB 148",
				LectureNumber = "Lec 01",
				InstructorName = "Emma Towlson",
				Days = "MWF",
				CourseTime = "14:00 - 14:50",
				Seats = "33/50",
				Map = "/MapImages/SB.png",
				Status = "Not-Enrolled"
			},
			new Course
			{
				CourseName = "Introduction to Dinosaurs",
				CourseCode = "GLGY 305",
				Description ="Biology, evolution, and extinction of dinosaurs; geographic and temporal distribution, habitats, and ecology of the various dinosaur groups; preservation, exploration, collection, preparation, and identification of dinosaur fossils.",
				Prereq = "None",
				LectureLocation = "CHC 119",
				LectureNumber = "Lec 01",
				InstructorName = "Darla Zelenitsky",
				Days = "MF",
				CourseTime = "12:00 - 12:50",
				Seats = "10/100",
				Map = "/MapImages/CHC.png",
				Status = "Waitlisted"
			}
		};
	}


}
