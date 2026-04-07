using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using System.Text.Json.Serialization;

namespace SD.Core.Entities
{

    [Table(name: nameof(Genre) + "s")]
    public class Genre : IEntity
    {
        public Genre() 
        { 
            this.Movies = new HashSet<Movie>();
        }

        public virtual int Id { get; set; }

        [MinLength(2)]
        [MaxLength(64)]
        [Required]
        public virtual string Name { get; set; } = string.Empty;

        /* Navigation Property zur Movie-Entität */
        [JsonIgnore]
        public virtual ICollection<Movie> Movies { get; }

    }
}
