using Mediator;
using SD.Core.Application.Results;
using System;
using System.Collections.Generic;
using System.Text;

namespace SD.Core.Application.Queries
{
    public class GetMovieDtoQuery : IQuery<MovieDto>
    {
        public Guid Id { get; set; }
    }
}
