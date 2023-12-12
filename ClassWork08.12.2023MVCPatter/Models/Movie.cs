namespace ClassWork08._12._2023MVCPatter.Models
{
    public class Movie
    {
        public int Id { get; set; } 
        public string? Title { get; set; }
        public string? Description { get; set; }
        public string? Director { get; set; }
        public string? Country { get; set; }
        public string? Ganre { get; set; }
        public int Year { get; set; }
        public string? Cast { get; set; }
        public string? Poster { get; set; }

        public Movie() { }
    }
}
