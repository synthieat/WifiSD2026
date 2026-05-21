using Mediator;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace SD.WS.Controllers
{
    public class MediatorBaseController : ControllerBase
    {
        protected const string ID_PARAMETER = "/{Id}";

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

        protected T SetLocationUri<T>(T result, string id)
        {
            if(result == null || string.IsNullOrWhiteSpace(id))
            {
                throw new HttpRequestException("Resource not found");
            }

            /* Aktueller URL ermitteln */
            var baseUrl = Request.HttpContext.Request.GetEncodedUrl();

            /* BASE URL bis zum ersten (QueryString) Parameter, falls vorhanden, kürzen */
            var length = baseUrl.IndexOf('?') > 0 ? baseUrl.IndexOf('?') : baseUrl.Length;

            /* URL auf gültige Länge mit Substring zuschneiden */
            var uri = baseUrl.Substring(0, length);

            /*
               .../Movie/MovieDto
               .../Movie/MovieDto/
               .../Movie/MovieDto/?Parameter1=
               .../MOvie/MovieDto?Parameter1=
            */

            uri = string.Concat(uri, uri.EndsWith('/') ? string.Empty : "/", id);

            /* Location Header im Response setzen */
            HttpContext.Response.Headers.Append("Location", uri);

            /* Http-Status auf 201 - Created setzen */
            HttpContext.Response.StatusCode = StatusCodes.Status201Created;

            return result;
            
        }
    }
}
