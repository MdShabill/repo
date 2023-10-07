namespace MyWebApp.DataModel
{
    public class Movie
    {
        public int Id { get; set; }
        public string MovieName { get; set; }
        public string DirectorName { get; set; }

        public int ActorId { get; set; }
        public string ActorName { get; set; }

        public int ActressId { get; set; }
        public string ActressName { get; set; }
    }
}
