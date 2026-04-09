using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace SD.Core.Entities
{
    public enum Ratings : byte
    {
        Bad = 10,
        Average = 20,
        Good = 30,
        Excellent = 40
    }

    public abstract class MovieBase
    {
        public virtual Guid Id { get; set; }

        [MinLength(1), MaxLength(128)]
        public virtual string Title { get; set; }

        public virtual int GenreId { get; set; }

        public virtual string? MediumTypeCode { get; set; }

        public virtual decimal Price { get; set; }

        public virtual DateTime ReleaseDate { get; set; }

        public virtual Ratings Rating { get; set; }

    }
}
