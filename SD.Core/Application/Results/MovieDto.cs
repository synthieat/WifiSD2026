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
    }
}
