using System;
using System.Collections.Generic;
using System.Text;

namespace SD.Core.Entities
{
    public class Movie
    {
        public Guid Id { get; set; }

        public string Title { get; set; }

        public int GenreId { get; set; }

        public string? MediumTypeCode { get; set; }

        public decimal Price { get; set; }

        public DateTime ReleaseDate { get; set; }

    }
}
