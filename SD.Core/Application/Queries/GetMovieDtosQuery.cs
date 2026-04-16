using Mediator;
using SD.Core.Application.Results;
using System;
using System.Collections.Generic;
using System.Text;

namespace SD.Core.Application.Queries
{
    public class GetMovieDtosQuery : IQuery<IEnumerable<MovieDto>>
    {
        public int? GenreId { get; set; }
        public string? MediumTypeCode { get; set; }

        public string? SearchText { get; set; }

        public int Take { get; set; } = 10;
        public int Skip { get; set; } = 0; 

    }
}
