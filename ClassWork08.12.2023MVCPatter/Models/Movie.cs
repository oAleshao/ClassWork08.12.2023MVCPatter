using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace ClassWork08._12._2023MVCPatter.Models
{
    public class Movie:IComparable<Movie>
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Поле должно быть установлено")]
        public string? Title { get; set; }
        [Required(ErrorMessage = "Поле должно быть установлено")]
        public string? Description { get; set; }
        [Required(ErrorMessage = "Поле должно быть установлено")]
        public string? Director { get; set; }
        [Required(ErrorMessage = "Поле должно быть установлено")]
        public string? Country { get; set; }
        [Required(ErrorMessage = "Поле должно быть установлено")]
        public string? Ganre { get; set; }
        [Required(ErrorMessage = "Поле должно быть установлено")]
        [Range(1900, 2023, ErrorMessage = "Недопустимый год")]
        public int Year { get; set; }
        [Required(ErrorMessage = "Поле должно быть установлено")]
        public string? Cast { get; set; }
        [Remote(action: "CheckPoster", controller: "Movies", ErrorMessage = "Постер уже используется")]
        public string? Poster { get; set; }

        public Movie() { }

        public int CompareTo(Movie? other)
        {
            if (other == null)
                return -1;
            if (this.Title == other.Title && this.Year == other.Year && this.Ganre == other.Ganre
                && this.Cast == other.Cast && this.Description == other.Description && this.Director == other.Director
                && this.Country == other.Country && this.Poster == other.Poster)
            { return 1; }
            else if (this.Id == other.Id && this.Title == other.Title && this.Year == other.Year && this.Ganre == other.Ganre
               && this.Cast == other.Cast && this.Description == other.Description && this.Director == other.Director
               && this.Country == other.Country && this.Poster == other.Poster)
            { return 1; }

            return -1;
        }

        public int CompareTo(List<Movie>? other)
        {
            if (other == null && other?.Count == 0)
                return -1;

            foreach(var movie in other)
            {
                if (this.Title == movie.Title && this.Year == movie.Year && this.Ganre == movie.Ganre
               && this.Cast == movie.Cast && this.Description == movie.Description && this.Director == movie.Director
               && this.Country == movie.Country && this.Poster == movie.Poster)
                { return 1; }
            }


            return -1;
        }
    }
}
