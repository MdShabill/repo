using WebApiDemo1.Enums;

namespace WebApiDemo1.DataModel
{
    public class Movie
    {
        public int Id { get; set; }
        public string ActorName { get; set; }
        public string ActressName { get; set; }
        public string Title { get; set; }
        public MovieTypes MovieType { get; set; }
        public DateTime ReleaseDate { get; set; }
    }
}
