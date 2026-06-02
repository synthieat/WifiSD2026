using Mediator;
using Microsoft.AspNetCore.Mvc;

namespace SD.Web.Controllers
{
    public class MediatorBaseController : Controller
    {
        private IMediator mediator;

        /* Konstante für ID - Parameter */
        protected const string ID_PARAMETER = "/{Id}";

        protected IMediator Mediator => mediator ??= HttpContext.RequestServices.GetService<IMediator>();

        /* Scaffolding im SD.Web Ordner in Console ausführen:

           dotnet aspnet-codegenerator controller -name MoviesController -m SD.Core.Entities.Movie -dc SD.Persistence.Repositories.DBContext.MovieDbContext -f

           Dann den Controller in den Controllers Ordner verschieben und dann in den cshtml Dateien von Views/Movies das Layout und den Titel anpassen.
        */
    }
}
