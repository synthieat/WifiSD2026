using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace SD.Core.Entities
{
    [Table(name: nameof(MovieBase) + "s")]
    public class Movie : MovieBase, IEntity
    {        
        /* Navigation Properties zu Genre und MediumType */
        public virtual Genre Genre { get; set; }

        [ForeignKey(nameof(Movie.MediumTypeCode))]
        public virtual MediumType MediumType { get; set; }  
    }
}
