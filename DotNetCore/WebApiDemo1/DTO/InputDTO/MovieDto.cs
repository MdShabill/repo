﻿using WebApiDemo1.Enums;

namespace WebApiDemo1.DTO.InputDTO
{
    public class MovieDto
    {
        public int Id { get; set; }
        public string ActorName { get; set; }   
        public string ActressName { get; set; }
        public string Title { get; set; }
        public MovieTypes MovieType { get; set;}
    }
}