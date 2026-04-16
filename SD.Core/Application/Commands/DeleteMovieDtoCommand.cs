using Mediator;
using System;
using System.Collections.Generic;
using System.Text;

namespace SD.Core.Application.Commands
{
    public class DeleteMovieDtoCommand : ICommand
    {
        public Guid Id { get; set; }
    }
}
