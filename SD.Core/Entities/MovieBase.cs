using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace SD.Core.Entities
{
    public abstract class MovieBase
    {
        public virtual Guid Id { get; set; }

        [MinLength(1), MaxLength(128)]
        public virtual string Title { get; set; }

        public virtual int GenreId { get; set; }

        public virtual string? MediumTypeCode { get; set; }

        public virtual decimal Price { get; set; }

        public virtual DateTime ReleaseDate { get; set; }

    }
}
