using WebApiDemo1.Enums;

namespace WebApiDemo1.DTO.InputDTO
{
    public class MovieDto
    {
        public int Id { get; set; }
        public string Hero { get; set; }   
        public string Heroine { get; set; }
        public string Title { get; set; }
        public MovieTypes MovieType { get; set;}
        public DateTime ReleaseDate { get; set; }
    }
}
