using Mediator;
using Microsoft.EntityFrameworkCore;
using SD.Application.Movies;
using SD.Core.Application.Queries;
using SD.Core.Application.Results;
using SD.Core.Attributes;
using SD.Core.Entities;
using SD.Core.Repositories.Movies;
using System;
using System.Collections.Generic;
using System.Text;

namespace SD.Application.Movies
{
    [MapServiceDependency(nameof(MovieQueryHandler))]
    public class MovieQueryHandler : IQueryHandler<GetMovieDtoQuery, MovieDto>,
                                     IQueryHandler<GetMovieDtosQuery, IEnumerable<MovieDto>>,
                                     IQueryHandler<GetGenresQuery, IEnumerable<Genre>>,
                                     IQueryHandler<GetMediumTypesQuery,IEnumerable<MediumType>>
    {
        protected readonly IMovieRepository movieRepository;

        public MovieQueryHandler(IMovieRepository movieRepository)
        {
            this.movieRepository = movieRepository;
        }

        public async ValueTask<MovieDto> Handle(GetMovieDtoQuery query,
                                                CancellationToken cancellationToken)
        {
            var result = await this.GetMovieQuery()
                                   .Where(w => w.Id == query.Id)
                                   .Select(s => MovieDto.MapFrom(s))
                                   .FirstOrDefaultAsync(cancellationToken);

            if (result != null)
            {
                return result;
            }

            return null;

        }

        public async ValueTask<IEnumerable<MovieDto>> Handle(GetMovieDtosQuery query, CancellationToken cancellationToken)
        {
            var movieDtoQuery = this.GetMovieQuery()
                                    .Where(w => (!query.GenreId.HasValue || w.GenreId == query.GenreId.Value) 
                                          && (string.IsNullOrWhiteSpace(query.MediumTypeCode) || w.MediumTypeCode == query.MediumTypeCode)
                                          && (string.IsNullOrWhiteSpace(query.SearchText) || w.Title.Contains(query.SearchText)))
                                    .Select(s => MovieDto.MapFrom(s))
                                    .Skip(query.Skip) /* Pagination: Skip / Take
                                                         Achtung: Immer zuerst Skip, dann Take, da sonst die falschen Datensätze übersprungen werden! */
                                    .Take(query.Take);

            /* Variante mit separaten Where-Statements 
            if(!string.IsNullOrWhiteSpace(query.SearchText))
            {
                movieDtoQuery = movieDtoQuery.Where(w => w.Title.Contains(query.SearchText));
            }
           
            if(query.GenreId.HasValue)
            {
                movieDtoQuery = movieDtoQuery.Where(w => w.GenreId == query.GenreId.Value);
            }

            if(!string.IsNullOrWhiteSpace(query.MediumTypeCode))
            {
                movieDtoQuery = movieDtoQuery.Where(w => w.MediumTypeCode == query.MediumTypeCode);
            }*/

            return await movieDtoQuery.ToListAsync(cancellationToken);

         }

        public async ValueTask<IEnumerable<Genre>> Handle(GetGenresQuery query, CancellationToken cancellationToken)
        {
           return await this.movieRepository.QueryFrom<Genre>()
                                            .ToListAsync(cancellationToken);
        }

      

        public async ValueTask<IEnumerable<MediumType>> Handle(GetMediumTypesQuery query, CancellationToken cancellationToken)
        {
            return await this.movieRepository.QueryFrom<MediumType>()
                                             .ToListAsync(cancellationToken);
        }

        private IQueryable<Movie> GetMovieQuery()
        {
            return this.movieRepository.QueryFrom<Movie>()
                                       .Include(i => i.Genre)
                                       .Include(i => i.MediumType);
                                       
                                       
        }
    }
}
