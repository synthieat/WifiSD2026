using Mediator;
using SD.Core.Application.Commands;
using SD.Core.Application.Results;
using SD.Core.Attributes;
using SD.Core.Entities;
using SD.Core.Repositories.Movies;

namespace SD.Application.Movies
{
    [MapServiceDependency(nameof(MovieCommandHandler))]
    public class MovieCommandHandler : BaseHandler, ICommandHandler<CreateMovieDtoCommand, MovieDto>,
                                                    ICommandHandler<UpdateMovieDtoCommand, MovieDto>,
                                                    ICommandHandler<DeleteMovieDtoCommand>
    {
        protected readonly IMovieRepository movieRepository;

        public MovieCommandHandler(IMovieRepository movieRepository)
        {
            this.movieRepository = movieRepository;
        }

        public async ValueTask<MovieDto> Handle(CreateMovieDtoCommand command, 
                                                CancellationToken cancellationToken)
        {
            var movie = new Movie
            {
                Id = Guid.NewGuid(),
                Title = "n/a",
                GenreId = 1,
                MediumTypeCode = "BR"
            };

            await movieRepository.AddAsync(movie, true, cancellationToken);           

            return MovieDto.MapFrom(movie); 

        }

        public async ValueTask<MovieDto> Handle(UpdateMovieDtoCommand command, CancellationToken cancellationToken)
        {
            command.MovieDto.Id = command.Id;
            var movie = new Movie();

            base.MapEntityProperties(command.MovieDto, movie); 
            var updateMovie = await this.movieRepository.UpdateAsync(movie, command.Id, true, cancellationToken);    

            return MovieDto.MapFrom(movie);


        }

        public async ValueTask<Unit> Handle(DeleteMovieDtoCommand command, CancellationToken cancellationToken)
        {
            await this.movieRepository.RemoveByKeyAsync<Movie>(command.Id, true, cancellationToken); 
            return Unit.Value;
        }
    }

}
