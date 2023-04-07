namespace CPSC481CourseScheduler.Data
{
    public class Friend
    {
        public int Id { get; set; } = 0;
        public string Name { get; set; } = "";
        public bool IsSelected { get; set; } = false;

        public Friend(int id, string name, bool isSelected)
        {
            this.Id = id;
            this.Name = name;
            this.IsSelected = isSelected;
        }
    }
}
