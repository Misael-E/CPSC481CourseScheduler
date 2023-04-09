using CPSC481CourseScheduler.Data;

namespace CPSC481CourseScheduler.Services
{
    public class FriendService: IFriendService
    {
        public event EventHandler<List<Course>> OnSelectedFriendChanged;

        private List<Friend> friends = new List<Friend> 
        {
            new Friend(1, "Kyle", true),
            new Friend ( 2, "Victor", false),
            new Friend (3, "James",false )
        };

        public async Task AddFriend(Friend friend) 
        {
            await Task.Factory.StartNew(() => friends.Add(friend));
        }

        public async Task RemoveFriend(Friend friend)
        {
            await Task.Factory.StartNew(() => friends.Remove(friend));
        }

        Friend selectedFriendInit = new Friend(1, "Kyle", true);
        public async Task SelectFriend(Friend selectedFriend)
        {
            await Task.Factory.StartNew(() => 
            {
                foreach (var friend in friends)
                {
                    if (friend == selectedFriend)
                    {
                        friend.IsSelected = true;
                    }
                    else
                    {
                        friend.IsSelected = false;
                    }
                }
                selectedFriendInit = selectedFriend;
            });
            OnSelectedFriendChanged?.Invoke(this, FriendCourses[selectedFriendInit.Name]);
        }

        public List<Friend> GetAllFriends() 
        {
            return friends;
        }
        public List<Course> GetFriendCourses()
        {
            return FriendCourses[selectedFriendInit.Name];
        }

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
    }
}
