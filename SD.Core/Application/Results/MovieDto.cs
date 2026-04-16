using SD.Core.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace SD.Core.Application.Results
{
    public class MovieDto : MovieBase
    {
        public string GenreName { get; set; }
        public string MediumTypeName { get; set; }


        public static MovieDto MapFrom(Movie movie)
        {
            return new MovieDto
            {
                Id = movie.Id,
                Title = movie.Title,
                GenreId = movie.GenreId,
                GenreName = movie.Genre.Name,
                MediumTypeCode = movie.MediumTypeCode,
                MediumTypeName = movie.MediumType.Name,
                ReleaseDate = movie.ReleaseDate,
                Rating = movie.Rating

            };
        }
    }
}
