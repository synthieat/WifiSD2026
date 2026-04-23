using Mediator;
using Microsoft.AspNetCore.Mvc;
using SD.Core.Application.Queries;
using SD.Core.Application.Results;

namespace SD.WS.Controllers
{
    [ApiController]
    [Route("[Controller]")]
    public class MovieController : Controller
    {
        private IMediator mediator;

        protected IMediator Mediator => mediator ??= HttpContext.RequestServices.GetService<IMediator>();

        //protected IMediator Mediator
        //{
        //    get
        //    {
        //        if(this.mediator == null)
        //        {
        //            this.mediator = HttpContext.RequestServices.GetService<IMediator>();
        //        }
        //        return this.mediator;
        //    }
        //}


        [HttpGet(nameof(MovieDto))]
        public async Task<IEnumerable<MovieDto>> GetMovieDtos([FromQuery] GetMovieDtosQuery query, CancellationToken cancellationToken)
        {
            return await Mediator.Send(query, cancellationToken);
        }
    }
}
