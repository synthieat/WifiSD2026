using Mediator;
using Microsoft.AspNetCore.Mvc;
using SD.Core.Application.Commands;
using SD.Core.Application.Queries;
using SD.Core.Application.Results;

namespace SD.WS.Controllers
{
    [ApiController]
    [Route("[Controller]")]
    public class MovieController : MediatorBaseController
    {
              

        [HttpGet(nameof(MovieDto) + ID_PARAMETER)]
        public async Task<MovieDto> GetMovieDto([FromRoute] GetMovieDtoQuery query, CancellationToken cancellationToken)
        {
            return await Mediator.Send(query, cancellationToken);
        }


        [HttpGet(nameof(MovieDto))]
        public async Task<IEnumerable<MovieDto>> GetMovieDtos([FromQuery] GetMovieDtosQuery query, CancellationToken cancellationToken)
        {
            return await Mediator.Send(query, cancellationToken);
        }

        [HttpPost(nameof(MovieDto))]
        [ProducesResponseType(typeof(MovieDto), StatusCodes.Status201Created)]
        public async Task<MovieDto> CreateMovieDto(CancellationToken cancellationToken)
        {
            var result = await base.Mediator.Send(new CreateMovieDtoCommand(), cancellationToken);
            return base.SetLocationUri<MovieDto>(result, result.Id.ToString());
        }
    }
}
