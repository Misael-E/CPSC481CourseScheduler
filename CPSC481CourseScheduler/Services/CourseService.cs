

using CPSC481CourseScheduler.Components;
using CPSC481CourseScheduler.Data;
using System;
using System.Collections.Generic;

namespace CPSC481CourseScheduler.Services
{
    public class CourseService : ICourseService
    {

        Dictionary<string, List<Course>> SelectedCourses { get; set; } = new Dictionary<string, List<Course>>
        {
            { "Fall", new List<Course>() },
            { "Spring", new List<Course>() },
            { "Summer", new List<Course>() },
            { "Winter", new List<Course>() }
        };
        Dictionary<string, List<Course>> Bookmarks { get; set; } = new Dictionary<string, List<Course>>
        {
            { "Fall", new List<Course>() },
            { "Spring", new List<Course>() },
            { "Summer", new List<Course>() },
            { "Winter", new List<Course>() }
        };

        Dictionary<string, List<Course>> CoursesToDrop { get; set; } = new Dictionary<string, List<Course>>
        {
            { "Fall", new List<Course>() },
            { "Spring", new List<Course>() },
            { "Summer", new List<Course>() },
            { "Winter", new List<Course>() }
        };




        Semester SelectedSemester { get; set; } = new Semester("Winter", "2023");
        public async Task SelectSemester(Semester semester)
        {
            await Task.Factory.StartNew(() =>
            {
                SelectedSemester = semester;
            });
            OnSelectedSemesterChanged?.Invoke(this, SelectedSemester);
            OnBookmarksChanged?.Invoke(this, GetBookmarks());
            OnSelectedCoursesChanged?.Invoke(this, GetSelectedCourses());
        }

        public async Task KeepCourse(Course course)
        {
            await Task.Factory.StartNew(() =>
            {
                course.DropCourse = false;
                CoursesToDrop[SelectedSemester.Season].Remove(course);
            });
            OnSelectedCoursesChanged?.Invoke(this, SelectedCourses[SelectedSemester.Season]);
        }

        public async Task<int> AddToSelectedCourses(Course course)
        {

            if (course.DropCourse)
            {
                return -1;
            }
            bool duplicate = false;

            foreach (var crs in SelectedCourses[SelectedSemester.Season])
            {
                if (crs.CourseCode == course.CourseCode)
                {
                    duplicate = true;
                    break;
                }
            }

            bool removeCourse = false;
            foreach (var crs in CoursesToDrop[SelectedSemester.Season])
            {
                if (crs.CourseCode == course.CourseCode)
                {
                    removeCourse = true;
                    break;
                }
            }

            if (removeCourse)
            {
                CoursesToDrop[SelectedSemester.Season].Remove(course);
            }

            if (!duplicate && SelectedCourses[SelectedSemester.Season].Count < 6)
            {

                await Task.Factory.StartNew(() => SelectedCourses[SelectedSemester.Season].Add(course));
                OnSelectedCoursesChanged?.Invoke(this, SelectedCourses[SelectedSemester.Season]);
                return 0;
            }
            else if (duplicate)
            {
                return -1;
            }
            else
            {
                return -2;
            }
        }

        public async Task<int> AddToBookmarks(Course course)
        {
            bool duplicate = false;

            foreach (var crs in Bookmarks[SelectedSemester.Season])
            {
                if (crs.CourseCode == course.CourseCode)
                {
                    duplicate = true;
                    break;
                }
            }

            if (!duplicate)
            {
                await Task.Factory.StartNew(() => Bookmarks[SelectedSemester.Season].Add(course));
                OnBookmarksChanged?.Invoke(this, Bookmarks[SelectedSemester.Season]);
                return 0;
            }
            else
            {
                return -1;
            }
        }

        public async Task<int> setSelectedCourses(List<Course> courses) 
        {
            await Task.Factory.StartNew(() => SelectedCourses[SelectedSemester.Season] = courses);
            OnSelectedCoursesChanged?.Invoke(this, SelectedCourses[SelectedSemester.Season]);
            return 0;
        }

        public List<Course> GetSearchedCourses()
        {
            return CoursesBySemester[SelectedSemester.Season];
        }

        public event EventHandler<List<Course>> OnSelectedCoursesChanged;
        public event EventHandler<List<Course>> OnBookmarksChanged;
        public event EventHandler<Semester> OnSelectedSemesterChanged;

        public void FinalizedSchedule()
        {
            CoursesToDrop[SelectedSemester.Season].Clear();
            OnSelectedCoursesChanged?.Invoke(this, SelectedCourses[SelectedSemester.Season]);
        }

        public List<Course> GetSelectedCourses()
        {
            return SelectedCourses[SelectedSemester.Season];
        }

        public List<Course> GetCoursesToDrop()
        {
            return CoursesToDrop[SelectedSemester.Season];
        }
        public List<Course> GetBookmarks()
        {
            return Bookmarks[SelectedSemester.Season];
        }
        public List<Course> GetAllCourses() => AllCourses;
        public List<Course> GetCoursesBySemester() => CoursesBySemester[SelectedSemester.Season];

        public List<Course> GetRecommendedCourses()
        {
            return RecommendedCourses[SelectedSemester.Season];
        }
        public Dictionary<string, List<Course>> GetAllFriendsCourses() => FriendCourses;

        public List<Semester> GetAllSemesters() => AllSemesters;

        public Semester GetSelectedSemester() => SelectedSemester;
        public async Task RemoveFromSelectedCourses(Course course)
        {

            await Task.Factory.StartNew(() =>
            {
                if (course.Status == "Enrolled")
                {
                    CoursesToDrop[SelectedSemester.Season].Add(course);
                    course.DropCourse = true;
                }
                else
                {
                    SelectedCourses[SelectedSemester.Season].Remove(course);

                }
            });
            OnSelectedCoursesChanged?.Invoke(this, SelectedCourses[SelectedSemester.Season]);
        }

        public async Task RemoveFromBookmarks(Course course)
        {
            await Task.Factory.StartNew(() => Bookmarks[SelectedSemester.Season].Remove(course));
            OnBookmarksChanged?.Invoke(this, Bookmarks[SelectedSemester.Season]);
        }

        private static string GenerateHexColor()
        {
            Random random = new Random();
            byte red = (byte)random.Next(128, 192);
            byte green = (byte)random.Next(128, 192);
            byte blue = (byte)random.Next(128, 192);
            return "#" + red.ToString("X2") + green.ToString("X2") + blue.ToString("X2");
        }

        List<Semester> AllSemesters { get; set; } = new List<Semester>
        {
            new Semester("Winter", "2023"),
            new Semester("Spring", "2023"),
            new Semester("Summer", "2023"),
            new Semester("Fall", "2023"),
        };

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
                Days= "MWF",
                DaysOfWeek = new List<DayOfWeek>{ DayOfWeek.Monday, DayOfWeek.Wednesday, DayOfWeek.Friday },
                CourseTime = "10:00 - 10:50",
                StartTime = TimeSpan.FromHours(10),
                EndTime = TimeSpan.FromHours(10).Add(TimeSpan.FromMinutes(50)),
                Seats = "45/90",
                Map = "/MapImages/ST-Transparent.png",
                IndividualMap = "/MapImages/ST.png",
                Status = "Not enrolled",
                CourseColor = GenerateHexColor()
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
                Days= "TR",
                DaysOfWeek = new List<DayOfWeek>{ DayOfWeek.Tuesday, DayOfWeek.Thursday },
                CourseTime = "14:00 - 15:15",
                StartTime = TimeSpan.FromHours(14).Add(TimeSpan.FromMinutes(0)),
                EndTime = TimeSpan.FromHours(15).Add(TimeSpan.FromMinutes(15)),
                Seats = "40/90",
                Map = "/MapImages/MS-Transparent.png",
                IndividualMap = "/MapImages/MS.png",
                Status = "Not enrolled",
                CourseColor = GenerateHexColor()
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
                Days= "TR",
                DaysOfWeek = new List<DayOfWeek>{ DayOfWeek.Tuesday, DayOfWeek.Thursday },
                CourseTime = "12:30 - 13:45",
                StartTime = TimeSpan.FromHours(12).Add(TimeSpan.FromMinutes(30)),
                EndTime = TimeSpan.FromHours(13).Add(TimeSpan.FromMinutes(45)),
                Seats = "20/50",
                Map = "/MapImages/ICT-Transparent.png",
                IndividualMap = "/MapImages/ICT.png",
                Status = "Not enrolled",
                CourseColor = GenerateHexColor()
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
                Days= "MWF",
                DaysOfWeek = new List<DayOfWeek>{ DayOfWeek.Monday, DayOfWeek.Wednesday, DayOfWeek.Friday },
                CourseTime = "14:00 - 14:50",
                StartTime = TimeSpan.FromHours(14),
                EndTime = TimeSpan.FromHours(14).Add(TimeSpan.FromMinutes(50)),
                Seats = "33/50",
                Map = "/MapImages/SB-Transparent.png",
                IndividualMap = "/MapImages/SB.png",
                Status = "Not enrolled",
                CourseColor = GenerateHexColor()
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
                Days= "MWF",
                DaysOfWeek = new List<DayOfWeek>{ DayOfWeek.Monday, DayOfWeek.Wednesday, DayOfWeek.Friday },
                CourseTime = "12:00 - 12:50",
                StartTime = TimeSpan.FromHours(12),
                EndTime = TimeSpan.FromHours(12).Add(TimeSpan.FromMinutes(50)),
                Seats = "10/100",
                Map = "/MapImages/CHC-Transparent.png",
                IndividualMap = "/MapImages/CHC.png",
                Status = "Waitlisted",
                CourseColor = GenerateHexColor()
            }
        };

        Dictionary<string, List<Course>> FriendCourses { get; set; } = new Dictionary<string, List<Course>>
        {
            {
                "Kyle", new List<Course>
                {
                    new Course {
                        CourseCode = "CPSC 413",
                        StartTime = TimeSpan.FromHours(13).Add(TimeSpan.FromMinutes(0)),
                        EndTime = TimeSpan.FromHours(13).Add(TimeSpan.FromMinutes(50)),
                        DaysOfWeek = new List<DayOfWeek>{ DayOfWeek.Monday, DayOfWeek.Wednesday, DayOfWeek.Friday }
                    },
                    new Course {
                        CourseCode = "CPSC 513",
                        StartTime = TimeSpan.FromHours(12).Add(TimeSpan.FromMinutes(30)),
                        EndTime = TimeSpan.FromHours(13).Add(TimeSpan.FromMinutes(45)),
                        DaysOfWeek = new List<DayOfWeek>{ DayOfWeek.Tuesday, DayOfWeek.Thursday }
                    },
                    new Course {
                        CourseCode = "ASTR 201",
                        StartTime = TimeSpan.FromHours(9),
                        EndTime = TimeSpan.FromHours(9).Add(TimeSpan.FromMinutes(50)),
                        DaysOfWeek = new List<DayOfWeek>{ DayOfWeek.Monday, DayOfWeek.Wednesday, DayOfWeek.Friday }
                    }
                }
            },
            {
                "Victor", new List<Course>
                {
                    new Course {
                        CourseCode = "CPSC 583",
                        StartTime = TimeSpan.FromHours(14),
                        EndTime = TimeSpan.FromHours(14).Add(TimeSpan.FromMinutes(50)),
                        DaysOfWeek = new List<DayOfWeek>{ DayOfWeek.Monday, DayOfWeek.Wednesday, DayOfWeek.Friday }
                    },
                    new Course {
                        CourseCode = "ASTR 201",
                        StartTime = TimeSpan.FromHours(9),
                        EndTime = TimeSpan.FromHours(9).Add(TimeSpan.FromMinutes(50)),
                        DaysOfWeek = new List<DayOfWeek>{ DayOfWeek.Monday, DayOfWeek.Wednesday, DayOfWeek.Friday }
                    },
                    new Course {
                        CourseCode = "CPSC 513",
                        StartTime = TimeSpan.FromHours(12).Add(TimeSpan.FromMinutes(30)),
                        EndTime = TimeSpan.FromHours(13).Add(TimeSpan.FromMinutes(45)),
                        DaysOfWeek = new List<DayOfWeek>{ DayOfWeek.Tuesday, DayOfWeek.Thursday }
                    }
                }
            },
            {
                "James", new List<Course>
                {
                    new Course {
                        CourseCode = "CPSC 571",
                         StartTime = TimeSpan.FromHours(14).Add(TimeSpan.FromMinutes(0)),
                        EndTime = TimeSpan.FromHours(15).Add(TimeSpan.FromMinutes(15)),
                        DaysOfWeek = new List<DayOfWeek>{ DayOfWeek.Tuesday, DayOfWeek.Thursday }
                    },
                    new Course {
                        CourseCode = "ASTR 201",
                        StartTime = TimeSpan.FromHours(9),
                        EndTime = TimeSpan.FromHours(9).Add(TimeSpan.FromMinutes(50)),
                        DaysOfWeek = new List<DayOfWeek>{ DayOfWeek.Monday, DayOfWeek.Wednesday, DayOfWeek.Friday }
                    },
                    new Course {
                        CourseCode = "GLGY 305",
                        StartTime = TimeSpan.FromHours(12),
                        EndTime = TimeSpan.FromHours(12).Add(TimeSpan.FromMinutes(50)),
                        DaysOfWeek = new List<DayOfWeek>{ DayOfWeek.Monday, DayOfWeek.Wednesday, DayOfWeek.Friday },
                    }
                }
            }
        };


        Dictionary<string, List<Course>> CoursesBySemester { get; set; } = new Dictionary<string, List<Course>>
        {
            { "Fall", new List<Course>{
            new Course
                    {
                        CourseName = "Introduction to Sociology",
                        CourseCode = "SOCI 201",
                        Description = "Sociology as a discipline examines how the society in which we live influences our thinking and behaviour. An introduction to sociology through the study of society, social institutions, group behaviour and social change.",
                        Prereq = "None",
                        LectureLocation = "ST 140",
                        LectureNumber = "Lec 01",
                        InstructorName = "Dr Ehud Sharlin",
                        Days= "MWF",
                        DaysOfWeek = new List<DayOfWeek>{ DayOfWeek.Monday, DayOfWeek.Wednesday, DayOfWeek.Friday },
                        CourseTime = "10:00 - 10:50",
                        StartTime = TimeSpan.FromHours(10),
                        EndTime = TimeSpan.FromHours(10).Add(TimeSpan.FromMinutes(50)),
                        Seats = "45/90",
                        SeatsTaken = 45,
                        TotalSeats = 90,
                        Map = "/MapImages/ST-Transparent.png",
                        IndividualMap = "/MapImages/ST.png",
                        Status = "Not enrolled",
                        CourseColor = GenerateHexColor()
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
                        Days= "MWF",
                        DaysOfWeek = new List<DayOfWeek>{ DayOfWeek.Monday, DayOfWeek.Wednesday, DayOfWeek.Friday },
                        CourseTime = "14:00 - 14:50",
                        StartTime = TimeSpan.FromHours(14),
                        EndTime = TimeSpan.FromHours(14).Add(TimeSpan.FromMinutes(50)),
                        Seats = "33/50",
                        SeatsTaken = 33,
                        TotalSeats = 50,
                        Map = "/MapImages/SB-Transparent.png",
                        IndividualMap = "/MapImages/SB.png",
                        Status = "Not enrolled",
                        CourseColor = GenerateHexColor()
                    },
                     new Course
                    {
                        CourseName = "Programming Paradigms",
                        CourseCode = "CPSC 449",
                        Description ="Examination of the basic principles of the major programming language paradigms. Focus on declarative paradigms such as functional and logic programming. Data types, control expressions, loops, types of references, lazy evaluation, different interpretation principles, information hiding.",
                        Prereq = "Computer Science 319 or 331; and Philosophy 279 or 377",
                        LectureLocation = "SB 141",
                        LectureNumber = "Lec 01",
                        InstructorName = "Robin Cockett",
                        Days= "TR",
                        DaysOfWeek = new List<DayOfWeek>{ DayOfWeek.Tuesday, DayOfWeek.Thursday},
                        CourseTime = "09:30 - 10:45",
                        StartTime = TimeSpan.FromHours(9).Add(TimeSpan.FromMinutes(30)),
                        EndTime = TimeSpan.FromHours(10).Add(TimeSpan.FromMinutes(45)),
                        Seats = "37/45",
                        SeatsTaken = 37,
                        TotalSeats = 45,
                        Map = "/MapImages/SB-Transparent.png",
                        IndividualMap = "/MapImages/SB.png",
                        Status = "Not enrolled",
                        CourseColor = GenerateHexColor()
                    },
                     new Course
                    {
                        CourseName = "Computer Networks",
                        CourseCode = "CPSC 441",
                        Description ="Principles and practice in modern telecommunications, computer communications and networks. Layered communication protocols and current physical, data link, network and Internet protocol layers. Circuit switching, packet switching, and an introduction to broadband multimedia networking.",
                        Prereq = "Computer Science 319 or 331; and Computer Science 355 or Computer Engineering 369",
                        LectureLocation = "ICT 148",
                        LectureNumber = "Lec 01",
                        InstructorName = "Majid Ghaderi",
                        Days= "MWF",
                        DaysOfWeek = new List<DayOfWeek>{ DayOfWeek.Monday, DayOfWeek.Wednesday, DayOfWeek.Friday},
                        CourseTime = "12 - 12:50",
                        StartTime = TimeSpan.FromHours(12),
                        EndTime = TimeSpan.FromHours(12).Add(TimeSpan.FromMinutes(50)),
                        Seats = "100/106",
                        SeatsTaken = 100,
                        TotalSeats = 106,
                        Map = "/MapImages/ICT-Transparent.png",
                        IndividualMap = "/MapImages/ICT.png",
                        Status = "Not enrolled",
                        CourseColor = GenerateHexColor()
                    },
                     new Course
                    {
                        CourseName = "Computing Machinery I",
                        CourseCode = "CPSC 355",
                        Description ="An introduction to computing machinery establishing the connection between programs expressed in a compiled language, an assembly language, and machine code, and how such code is executed. Includes the detailed study of a modern CPU architecture, its assembly language and internal data representation, and the relationship between high-level program constructs and machine operations.",
                        Prereq = "3 units from Computer Science 219, 233 or 235",
                        LectureLocation = "MS 322",
                        LectureNumber = "Lec 01",
                        InstructorName = "Tamer Jarada",
                        Days= "MWF",
                        DaysOfWeek = new List<DayOfWeek>{ DayOfWeek.Monday, DayOfWeek.Wednesday, DayOfWeek.Friday},
                        CourseTime = "08 - 08:50",
                        StartTime = TimeSpan.FromHours(8),
                        EndTime = TimeSpan.FromHours(8).Add(TimeSpan.FromMinutes(50)),
                        Seats = "50/100",
                        SeatsTaken = 50,
                        TotalSeats = 100,
                        Map = "/MapImages/MS-Transparent.png",
                        IndividualMap = "/MapImages/MS.png",
                        Status = "Not enrolled",
                        CourseColor = GenerateHexColor()
                    },
                     new Course
                    {
                        CourseName = "Data Structures, Algorithms, and Their Applications",
                        CourseCode = "CPSC 319",
                        Description ="Fundamental data structures, including arrays, lists, stacks, queues, trees, hash tables, and graphs. Algorithms for searching and sorting. Introduction to analysis of algorithms. Applications of these data structures and algorithms. For students wishing to combine studies in computer science with studies in other disciplines.",
                        Prereq = "3 units from Computer Science 219, 233, 235, Computer Engineering 335, 339 or Software Engineering for Engineers 337.",
                        LectureLocation = "CHC 102",
                        LectureNumber = "Lec 01",
                        InstructorName = "Jorg Denzinger",
                        Days= "MWF",
                        DaysOfWeek = new List<DayOfWeek>{ DayOfWeek.Monday, DayOfWeek.Wednesday, DayOfWeek.Friday},
                        CourseTime = "16 - 16:50",
                        StartTime = TimeSpan.FromHours(16),
                        EndTime = TimeSpan.FromHours(16).Add(TimeSpan.FromMinutes(50)),
                        Seats = "20/80",
                        SeatsTaken = 20,
                        TotalSeats = 80,
                        Map = "/MapImages/CHC-Transparent.png",
                        IndividualMap = "/MapImages/CHC.png",
                        Status = "Not enrolled",
                        CourseColor = GenerateHexColor()
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
                        Days= "TR",
                        DaysOfWeek = new List<DayOfWeek>{ DayOfWeek.Tuesday, DayOfWeek.Thursday},
                        CourseTime = "15 - 16:45",
                        StartTime = TimeSpan.FromHours(15),
                        EndTime = TimeSpan.FromHours(16).Add(TimeSpan.FromMinutes(45)),
                        Seats = "100/130",
                        SeatsTaken = 100,
                        TotalSeats = 130,
                        Map = "/MapImages/ICT-Transparent.png",
                        IndividualMap = "/MapImages/ICT.png",
                        Status = "Not enrolled",
                        CourseColor = GenerateHexColor()
                    },
                      new Course
                    {
                        CourseName = "Principles of Macroeconomics",
                        CourseCode = "ECON 203",
                        Description ="Measurement of economic activity, economic growth, money and financial systems, international economy and exchange rates, business cycles, unemployment and inflation, and macroeconomic policies.",
                        Prereq = "Economics 201",
                        LectureLocation = "ICT 121",
                        LectureNumber = "Lec 01",
                        InstructorName = "Aamir Hashmi",
                        Days= "TR",
                        DaysOfWeek = new List<DayOfWeek>{ DayOfWeek.Tuesday, DayOfWeek.Thursday},
                        CourseTime = "18 - 19:45",
                        StartTime = TimeSpan.FromHours(18),
                        EndTime = TimeSpan.FromHours(19).Add(TimeSpan.FromMinutes(45)),
                        Seats = "140/160",
                        SeatsTaken = 140,
                        TotalSeats = 160,
                        Map = "/MapImages/ICT-Transparent.png",
                        IndividualMap = "/MapImages/ICT.png",
                        Status = "Not enrolled",
                        CourseColor = GenerateHexColor()
                    }
            } },
            { "Spring", new List<Course>() },
            { "Summer", new List<Course>() },
            { "Winter", new List<Course>{
                new Course
            {
                CourseName = "Design and Implementation of Database Systems",
                CourseCode = "CPSC 571",
                Description = "Implementation and design of modern database systems including query modification/optimization, recovery, concurrency, integrity, and distribution.",
                Prereq = "Computer Science 471",
                LectureLocation = "MS 217",
                LectureNumber = "Lec 01",
                InstructorName = "Tamer Jarada",
                Days= "TR",
                DaysOfWeek = new List<DayOfWeek>{ DayOfWeek.Tuesday, DayOfWeek.Thursday },
                CourseTime = "14:00 - 15:15",
                StartTime = TimeSpan.FromHours(14).Add(TimeSpan.FromMinutes(0)),
                EndTime = TimeSpan.FromHours(15).Add(TimeSpan.FromMinutes(15)),
                Seats = "40/90",
                SeatsTaken = 40,
                TotalSeats = 90,
                Map = "/MapImages/MS-Transparent.png",
                IndividualMap = "/MapImages/MS.png",
                Status = "Not enrolled",
                CourseColor = GenerateHexColor()
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
                Days= "TR",
                DaysOfWeek = new List<DayOfWeek>{ DayOfWeek.Tuesday, DayOfWeek.Thursday },
                CourseTime = "12:30 - 13:45",
                StartTime = TimeSpan.FromHours(12).Add(TimeSpan.FromMinutes(30)),
                EndTime = TimeSpan.FromHours(13).Add(TimeSpan.FromMinutes(45)),
                Seats = "20/50",
                SeatsTaken = 20,
                TotalSeats = 50,
                Map = "/MapImages/ICT-Transparent.png",
                IndividualMap = "/MapImages/ICT.png",
                Status = "Not enrolled",
                CourseColor = GenerateHexColor()
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
                Days= "MWF",
                DaysOfWeek = new List<DayOfWeek>{ DayOfWeek.Monday, DayOfWeek.Wednesday, DayOfWeek.Friday },
                CourseTime = "12:00 - 12:50",
                StartTime = TimeSpan.FromHours(12),
                EndTime = TimeSpan.FromHours(12).Add(TimeSpan.FromMinutes(50)),
                Seats = "10/100",
                SeatsTaken = 10,
                TotalSeats = 100,
                Map = "/MapImages/CHC-Transparent.png",
                IndividualMap = "/MapImages/CHC.png",
                Status = "Not enrolled",
                CourseColor = GenerateHexColor()
            }, new Course
            {
                CourseName = "Compiler Construction",
                CourseCode = "CPSC 411",
                Description ="Introduction to compilers, interpreters, and the tools for parsing and translation. Lexical analysis, context free grammars and software tools for their recognition. Attribute grammars and their applications in translation and compiling.",
                Prereq = "Computer Science 319 or 331; and Computer Science 355 or Computer Engineering 369",
                LectureLocation = "CHC 101",
                LectureNumber = "Lec 01",
                InstructorName = "John Aycock",
                Days= "TR",
                DaysOfWeek = new List<DayOfWeek>{ DayOfWeek.Tuesday, DayOfWeek.Thursday},
                CourseTime = "09:30 - 10:45",
                StartTime = TimeSpan.FromHours(9).Add(TimeSpan.FromMinutes(30)),
                EndTime = TimeSpan.FromHours(10).Add(TimeSpan.FromMinutes(45)),
                Seats = "35/72",
                SeatsTaken = 35,
                TotalSeats = 72,
                Map = "/MapImages/CHC-Transparent.png",
                IndividualMap = "/MapImages/CHC.png",
                Status = "Not enrolled",
                CourseColor = GenerateHexColor()
            },
             new Course
            {
                CourseName = "Computing Machinery I",
                CourseCode = "CPSC 355",
                Description ="An introduction to computing machinery establishing the connection between programs expressed in a compiled language, an assembly language, and machine code, and how such code is executed. Includes the detailed study of a modern CPU architecture, its assembly language and internal data representation, and the relationship between high-level program constructs and machine operations.",
                Prereq = "3 units from Computer Science 219, 233 or 235",
                LectureLocation = "ICT 212",
                LectureNumber = "Lec 01",
                InstructorName = "Tamer Jarada",
                Days= "MWF",
                DaysOfWeek = new List<DayOfWeek>{ DayOfWeek.Monday, DayOfWeek.Wednesday, DayOfWeek.Friday },
                CourseTime = "10:00 - 10:50",
                StartTime = TimeSpan.FromHours(10),
                EndTime = TimeSpan.FromHours(10).Add(TimeSpan.FromMinutes(50)),
                Seats = "10/100",
                SeatsTaken = 10,
                TotalSeats = 100,
                Map = "/MapImages/ICT-Transparent.png",
                IndividualMap = "/MapImages/ICT.png",
                Status = "Not enrolled",
                CourseColor = GenerateHexColor()
            },
               new Course
            {
                CourseName = "The Classical Period",
                CourseCode = "PHIL 301",
                Description ="The study of a selection of philosophers from the Pre-Socratics, through Plato and Aristotle, to the Hellenistic and Roman philosophers.",
                Prereq = "None",
                LectureLocation = "CHC-112",
                LectureNumber = "Lec 01",
                InstructorName = "Yoshiki Kobasigawa",
                Days= "T",
                DaysOfWeek = new List<DayOfWeek>{ DayOfWeek.Tuesday},
                CourseTime = "17:00 - 19:50",
                StartTime = TimeSpan.FromHours(17),
                EndTime = TimeSpan.FromHours(19).Add(TimeSpan.FromMinutes(50)),
                Seats = "20/35",
                SeatsTaken = 20,
                TotalSeats = 35,
                Map = "/MapImages/CHC-Transparent.png",
                IndividualMap = "/MapImages/CHC.png",
                Status = "Not enrolled",
                CourseColor = GenerateHexColor()
            },
               new Course
            {
                CourseName = "Combinatorics and Graph Theory",
                CourseCode = "MATH 371",
                Description ="Counting techniques, generating functions, inclusion-exclusion, introduction to graph theory.",
                Prereq = "Mathematics 271 or 273; and one of Mathematics 249, 265 or 275. Also known as: (formerly Pure Mathematics 471)",
                LectureLocation = "SB-215",
                LectureNumber = "Lec 01",
                InstructorName = "Karoly Bezdek",
                Days= "MWF",
                DaysOfWeek = new List<DayOfWeek>{  DayOfWeek.Monday, DayOfWeek.Wednesday, DayOfWeek.Friday},
                CourseTime = "09:00 - 09:50",
                StartTime = TimeSpan.FromHours(9),
                EndTime = TimeSpan.FromHours(9).Add(TimeSpan.FromMinutes(50)),
                Seats = "27/42",
                SeatsTaken = 27,
                TotalSeats = 42,
                Map = "/MapImages/SB-Transparent.png",
                IndividualMap = "/MapImages/SB.png",
                Status = "Not enrolled",
                CourseColor = GenerateHexColor()
            }

            } }
        };

        Dictionary<string, List<Course>> RecommendedCourses { get; set; } = new Dictionary<string, List<Course>>
        {
            { "Fall", new List<Course>{
                 new Course
                    {
                        CourseName = "Introduction to Sociology",
                        CourseCode = "SOCI 201",
                        Description = "Sociology as a discipline examines how the society in which we live influences our thinking and behaviour. An introduction to sociology through the study of society, social institutions, group behaviour and social change.",
                        Prereq = "None",
                        LectureLocation = "ST 140",
                        LectureNumber = "Lec 01",
                        InstructorName = "Dr Ehud Sharlin",
                        Days= "MWF",
                        DaysOfWeek = new List<DayOfWeek>{ DayOfWeek.Monday, DayOfWeek.Wednesday, DayOfWeek.Friday },
                        CourseTime = "10:00 - 10:50",
                        StartTime = TimeSpan.FromHours(10),
                        EndTime = TimeSpan.FromHours(10).Add(TimeSpan.FromMinutes(50)),
                        Seats = "45/90",
                        SeatsTaken = 45,
                        TotalSeats = 90,
                        Map = "/MapImages/ST-Transparent.png",
                        IndividualMap = "/MapImages/ST.png",
                        Status = "Not enrolled",
                        CourseColor = GenerateHexColor()
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
                        Days= "MWF",
                        DaysOfWeek = new List<DayOfWeek>{ DayOfWeek.Monday, DayOfWeek.Wednesday, DayOfWeek.Friday },
                        CourseTime = "14:00 - 14:50",
                        StartTime = TimeSpan.FromHours(14),
                        EndTime = TimeSpan.FromHours(14).Add(TimeSpan.FromMinutes(50)),
                        Seats = "33/50",
                        SeatsTaken = 33,
                        TotalSeats = 50,
                        Map = "/MapImages/SB-Transparent.png",
                        IndividualMap = "/MapImages/SB.png",
                        Status = "Not enrolled",
                        CourseColor = GenerateHexColor()
                    },
                     new Course
                    {
                        CourseName = "Programming Paradigms",
                        CourseCode = "CPSC 449",
                        Description ="Examination of the basic principles of the major programming language paradigms. Focus on declarative paradigms such as functional and logic programming. Data types, control expressions, loops, types of references, lazy evaluation, different interpretation principles, information hiding.",
                        Prereq = "Computer Science 319 or 331; and Philosophy 279 or 377",
                        LectureLocation = "SB 141",
                        LectureNumber = "Lec 01",
                        InstructorName = "Robin Cockett",
                        Days= "TR",
                        DaysOfWeek = new List<DayOfWeek>{ DayOfWeek.Tuesday, DayOfWeek.Thursday},
                        CourseTime = "09:30 - 10:45",
                        StartTime = TimeSpan.FromHours(9).Add(TimeSpan.FromMinutes(30)),
                        EndTime = TimeSpan.FromHours(10).Add(TimeSpan.FromMinutes(45)),
                        Seats = "37/45",
                        SeatsTaken = 37,
                        TotalSeats = 45,
                        Map = "/MapImages/SB-Transparent.png",
                        IndividualMap = "/MapImages/SB.png",
                        Status = "Not enrolled",
                        CourseColor = GenerateHexColor()
                    },
                     new Course
                    {
                        CourseName = "Computer Networks",
                        CourseCode = "CPSC 441",
                        Description ="Principles and practice in modern telecommunications, computer communications and networks. Layered communication protocols and current physical, data link, network and Internet protocol layers. Circuit switching, packet switching, and an introduction to broadband multimedia networking.",
                        Prereq = "Computer Science 319 or 331; and Computer Science 355 or Computer Engineering 369",
                        LectureLocation = "ICT 148",
                        LectureNumber = "Lec 01",
                        InstructorName = "Majid Ghaderi",
                        Days= "MWF",
                        DaysOfWeek = new List<DayOfWeek>{ DayOfWeek.Monday, DayOfWeek.Wednesday, DayOfWeek.Friday},
                        CourseTime = "12 - 12:50",
                        StartTime = TimeSpan.FromHours(12),
                        EndTime = TimeSpan.FromHours(12).Add(TimeSpan.FromMinutes(50)),
                        Seats = "100/106",
                        SeatsTaken = 100,
                        TotalSeats = 106,
                        Map = "/MapImages/ICT-Transparent.png",
                        IndividualMap = "/MapImages/ICT.png",
                        Status = "Not enrolled",
                        CourseColor = GenerateHexColor()
                    }
                }
            },
            { "Spring", new List<Course>() },
            { "Summer", new List<Course>() },
            { "Winter", new List<Course>{
                new Course
            {
                CourseName = "Design and Implementation of Database Systems",
                CourseCode = "CPSC 571",
                Description = "Implementation and design of modern database systems including query modification/optimization, recovery, concurrency, integrity, and distribution.",
                Prereq = "Computer Science 471",
                LectureLocation = "MS 217",
                LectureNumber = "Lec 01",
                InstructorName = "Tamer Jarada",
                Days= "TR",
                DaysOfWeek = new List<DayOfWeek>{ DayOfWeek.Tuesday, DayOfWeek.Thursday },
                CourseTime = "14:00 - 15:15",
                StartTime = TimeSpan.FromHours(14).Add(TimeSpan.FromMinutes(0)),
                EndTime = TimeSpan.FromHours(15).Add(TimeSpan.FromMinutes(15)),
                Seats = "40/90",
                SeatsTaken = 40,
                TotalSeats = 90,
                Map = "/MapImages/MS-Transparent.png",
                IndividualMap = "/MapImages/MS.png",
                Status = "Not enrolled",
                CourseColor = GenerateHexColor()
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
                Days= "TR",
                DaysOfWeek = new List<DayOfWeek>{ DayOfWeek.Tuesday, DayOfWeek.Thursday },
                CourseTime = "12:30 - 13:45",
                StartTime = TimeSpan.FromHours(12).Add(TimeSpan.FromMinutes(30)),
                EndTime = TimeSpan.FromHours(13).Add(TimeSpan.FromMinutes(45)),
                Seats = "20/50",
                SeatsTaken = 20,
                TotalSeats = 50,
                Map = "/MapImages/ICT-Transparent.png",
                IndividualMap = "/MapImages/ICT.png",
                Status = "Not enrolled",
                CourseColor = GenerateHexColor()
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
                Days= "MWF",
                DaysOfWeek = new List<DayOfWeek>{ DayOfWeek.Monday, DayOfWeek.Wednesday, DayOfWeek.Friday },
                CourseTime = "12:00 - 12:50",
                StartTime = TimeSpan.FromHours(12),
                EndTime = TimeSpan.FromHours(12).Add(TimeSpan.FromMinutes(50)),
                Seats = "10/100",
                SeatsTaken = 10,
                TotalSeats = 100,
                Map = "/MapImages/CHC-Transparent.png",
                IndividualMap = "/MapImages/CHC.png",
                Status = "Not enrolled",
                CourseColor = GenerateHexColor()
            }, new Course
            {
                CourseName = "Compiler Construction",
                CourseCode = "CPSC 411",
                Description ="Introduction to compilers, interpreters, and the tools for parsing and translation. Lexical analysis, context free grammars and software tools for their recognition. Attribute grammars and their applications in translation and compiling.",
                Prereq = "Computer Science 319 or 331; and Computer Science 355 or Computer Engineering 369",
                LectureLocation = "CHC 101",
                LectureNumber = "Lec 01",
                InstructorName = "John Aycock",
                Days= "TR",
                DaysOfWeek = new List<DayOfWeek>{ DayOfWeek.Tuesday, DayOfWeek.Thursday},
                CourseTime = "09:30 - 10:45",
                StartTime = TimeSpan.FromHours(9).Add(TimeSpan.FromMinutes(30)),
                EndTime = TimeSpan.FromHours(10).Add(TimeSpan.FromMinutes(45)),
                Seats = "35/72",
                SeatsTaken = 35,
                TotalSeats = 72,
                Map = "/MapImages/CHC-Transparent.png",
                IndividualMap = "/MapImages/CHC.png",
                Status = "Not enrolled",
                CourseColor = GenerateHexColor()
            }
                }
            }
        };
    }
}
