using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using System.Text.Json.Serialization;

namespace SD.Core.Entities
{

    [Table(name: nameof(MediumType) + "s")]
    public class MediumType : IEntity
    {
        public MediumType() 
        { 
            this.Movies = new HashSet<MovieBase>(); 
        }

        [Key]
        [MinLength(2), MaxLength(8)]
        public virtual string Code { get; set; }

        [MinLength(2), MaxLength(32)]
        [Required]
        public virtual string Name { get; set; }

        /*Navigation Property zur Movie-Entität */
        [JsonIgnore]
        public virtual ICollection<MovieBase> Movies { get; }   
    }
}
