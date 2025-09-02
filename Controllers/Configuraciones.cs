using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BibliotecaAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class Configuraciones : ControllerBase
    {
        private readonly IConfiguration configuration;

        public Configuraciones(IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        [HttpGet]
        public ActionResult<string> Get()
        {
            var opcion1 = configuration["apellido"];
            var opcion2 = configuration.GetValue<string>("apellido")!;

            return opcion2;
        }

        [HttpGet("secciones")]
        public ActionResult<string> GetSecction()
        {
            var opcion1 = configuration["ConnectionStrings:DefaultConnection"];
            var opcion2 = configuration.GetValue<string>("ConnectionStrings:DefaultConnection");
            var seccion = configuration.GetSection("ConnectionStrings");
            var opcion3 = seccion["DefaultConnection"];
            var opcion4 = seccion.GetValue<string>("DefaultConnection")!;

            return opcion4;
        }
    }
}
