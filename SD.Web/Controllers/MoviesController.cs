using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SD.Core.Application.Commands;
using SD.Core.Application.Queries;
using SD.Core.Application.Results;
using SD.Core.Entities;
using SD.Core.EnumDescriptors;
using SD.Persistence.Repositories.DBContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SD.Web.Controllers
{
    public class MoviesController : MediatorBaseController
    {
        private readonly MovieDbContext _context;

        public MoviesController(MovieDbContext context)
        {
            _context = context;
        }

        // GET: Movies
        public async Task<IActionResult> Index([FromQuery] GetMovieDtosQuery query, CancellationToken cancellationToken)
        {
            var movieDtos = await base.Mediator.Send(query, cancellationToken);            
            return View(movieDtos);
        }

        // GET: Movies/Details/5
        public async Task<IActionResult> Details([FromRoute] GetMovieDtoQuery query, CancellationToken cancellationToken)
        {
            var movieDto = await base.Mediator.Send(query, cancellationToken);
            return View(movieDto);
        }


        /* Nicht notwendig, weil mit POST immer eine neue Movie Entität angelegt wird
        // GET: Movies/Create
        public IActionResult Create()
        {
            ViewData["GenreId"] = new SelectList(_context.Genres, "Id", "Name");
            ViewData["MediumTypeCode"] = new SelectList(_context.MediumTypes, "Code", "Code");
            return View();
        }
        */

        // POST: Movies/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CancellationToken cancellationToken)
        {
            var movieDto = await base.Mediator.Send(new CreateMovieDtoCommand(), cancellationToken);

            /* ViewDate = ViewBag */
            await this.InitMovieDtoNavigationProperties(movieDto.GenreId, movieDto.MediumTypeCode, movieDto.Rating, cancellationToken);          
            return View(movieDto);
        }

        // GET: Movies/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var movie = await _context.Movies.FindAsync(id);
            if (movie == null)
            {
                return NotFound();
            }

            ViewData["GenreId"] = new SelectList(_context.Genres, "Id", "Name", movie.GenreId);
            ViewData["MediumTypeCode"] = new SelectList(_context.MediumTypes, "Code", "Code", movie.MediumTypeCode);
            return View(movie);
        }

        // POST: Movies/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit([FromRoute]Guid id, [FromForm]MovieDto movieDto, CancellationToken cancellationToken)
        {
            if (id != movieDto.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var command = new UpdateMovieDtoCommand { Id = id, MovieDto = movieDto };
                    await this.Mediator.Send(command, cancellationToken);
                }
                catch(Exception ex)
                {
                    throw;
                }
                //catch (DbUpdateConcurrencyException)
                //{
                    //if (!MovieExists(movie.Id))
                    //{
                    //    return NotFound();
                    //}
                    //else
                    //{
                    //    throw;
                    //}
                //}
                return RedirectToAction(nameof(Index));
            }

            await this.InitMovieDtoNavigationProperties(movieDto.GenreId, movieDto.MediumTypeCode, movieDto.Rating, cancellationToken);
            return View(movieDto);
        }

        // GET: Movies/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var movie = await _context.Movies
                .Include(m => m.Genre)
                .Include(m => m.MediumType)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (movie == null)
            {
                return NotFound();
            }

            return View(movie);
        }

        // POST: Movies/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var movie = await _context.Movies.FindAsync(id);
            if (movie != null)
            {
                _context.Movies.Remove(movie);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private async Task InitMovieDtoNavigationProperties(int? genreId, string mediumTypeCode = default, Ratings? ratings = default, CancellationToken cancellationToken = default)
        {

            var genres = await base.Mediator.Send(new GetGenresQuery(), cancellationToken);
            var genreSelectList = new SelectList(genres, nameof(Genre.Id), nameof(Genre.Name), genreId);

            var mediumTypeCodes = await base.Mediator.Send(new GetMediumTypesQuery(), cancellationToken);
            var mediumTypeCodeList = new SelectList(mediumTypeCodes, nameof(MediumType.Code), nameof(MediumType.Name), mediumTypeCode);

            var ratingDesciptors = RatingsDescriptor.All.Select(s => new { Rating = (int)s.Enum, RatingName = s.ToString() }).ToList();
            var ratingsList = new SelectList(ratingDesciptors, "Rating", "RatingName", (int)ratings);

            ViewBag.GenreId = genreSelectList;
            ViewData[nameof(MovieDto.MediumTypeCode)] = mediumTypeCodeList;
            ViewBag.Ratings = ratingsList;
        }


        private bool MovieExists(Guid id)
        {
            return _context.Movies.Any(e => e.Id == id);
        }
    }
}
