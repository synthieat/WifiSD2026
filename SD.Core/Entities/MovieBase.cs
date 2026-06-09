using SD.Resources;
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
        [Display(Name = nameof(MovieBase.Title), ResourceType = typeof(BasicRes))]
        public virtual string Title { get; set; }

        [Display(Name = "Genre", ResourceType = typeof(BasicRes))]
        public virtual int GenreId { get; set; }

        [Display(Name = "MediumType", ResourceType = typeof(BasicRes))]
        public virtual string? MediumTypeCode { get; set; }

        [Display(Name = nameof(MovieBase.Price), ResourceType = typeof(BasicRes))]
        public virtual decimal Price { get; set; }

        [Display(Name = nameof(MovieBase.ReleaseDate), ResourceType = typeof(BasicRes))]
        public virtual DateTime ReleaseDate { get; set; }

        [Display(Name = nameof(MovieBase.Rating), ResourceType = typeof(BasicRes))]
        public virtual Ratings Rating { get; set; }

    }
}
