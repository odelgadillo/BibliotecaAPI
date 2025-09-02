using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BibliotecaAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class Configuraciones : ControllerBase
    {
        private readonly IConfiguration configuration;
        private readonly IConfigurationSection seccion_01;
        private readonly IConfigurationSection seccion_02;

        public Configuraciones(IConfiguration configuration)
        {
            this.configuration = configuration;
            seccion_01 = configuration.GetSection("seccion1");
            seccion_02 = configuration.GetSection("seccion2");
        }

        [HttpGet("seccion_01")]
        public ActionResult GetSeccion01()
        {
            var nombre = seccion_01.GetValue<string>("nombre");
            var edad = seccion_01.GetValue<int>("edad");

            return Ok(new { nombre, edad });
        }

        [HttpGet("seccion_02")]
        public ActionResult GetSeccion02()
        {
            var nombre = seccion_02.GetValue<string>("nombre");
            var edad = seccion_02.GetValue<int>("edad");

            return Ok(new { nombre, edad });
        }

        [HttpGet("obtenertodos")]
        public ActionResult GetObtenerTodos()
        {
            var hijos = configuration.GetChildren().Select(x => $"{x.Key}: {x.Value}");
            return Ok(new { hijos });
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
