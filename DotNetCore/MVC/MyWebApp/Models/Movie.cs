namespace MyWebApp.Models
{
    public class Movie
    {
        public int Id { get; set; }
        public string MovieName { get; set; }
        public string DirectorName { get; set; }

        public int ActorId { get; set; }
        public string ActorName { get; set; }
    }
}
