using Mediator;
using SD.Core.Application.Results;
using System;
using System.Collections.Generic;
using System.Text;

namespace SD.Core.Application.Commands
{
    public class UpdateMovieDtoCommand : ICommand<MovieDto> 
    {
        public Guid Id { get; set; }
        public MovieDto MovieDto { get; set; }

    }
}
