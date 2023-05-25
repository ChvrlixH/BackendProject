namespace BackEndProject.Models
{
    public class CourseInfo
    {
        public int Id { get; set; }
        public DateTime Starts { get; set; }
        public int Duration { get; set; }
        public int ClassDuration { get; set; }
        public string SkillLevel { get; set; }
        public string Language { get; set; }
        public int Students { get; set; }
        public string Assesments { get; set; }
        public int Fee { get; set; }
    }
}
