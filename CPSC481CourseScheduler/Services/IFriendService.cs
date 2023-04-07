using CPSC481CourseScheduler.Data;

namespace CPSC481CourseScheduler.Services
{
    public interface IFriendService
    {
        Task AddFriend(Friend friend);
        Task RemoveFriend(Friend friend);
        Task SelectFriend(Friend selectedFriend);
        List<Friend> GetAllFriends();
        List<Course> GetFriendCourses();

        public event EventHandler<List<Course>> OnSelectedFriendChanged;
    }
}
