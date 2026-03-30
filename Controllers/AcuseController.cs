using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnidadGenero.Class;
using Acuse.Models;
namespace Acuse.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AcuseController : Controller 
    {
        private readonly ILogger<AcuseController> _logger;

        AcuseModels Acuse {  get; set; }
        public AcuseController(ILogger<AcuseController> logger)
        {
            _logger = logger;
            Acuse = new AcuseModels();
        }
        [HttpGet("reporte/{correo}")]
        public List<AcuseReporte> Get(string correo)
        {
            List<AcuseReporte> Result = new List<AcuseReporte>();
            Result = Acuse.Obtener(correo);
            return Result;
        }
        [HttpGet("administrador/reporte")]
        public List<AcuseReporteAdminitrador> Get1([FromQuery] int idarea=0, [FromQuery] int trimestre=0)
        {
            List<AcuseReporteAdminitrador> Result = new List<AcuseReporteAdminitrador>();
            Result = Acuse.ObtenerAdministrador(idarea,trimestre);
            return Result;
        }
        [HttpPost()]
        public AcuseResponse Post([FromBody] AcuseRequets datos)
        {
            AcuseResponse Result = new AcuseResponse();
            Result = Acuse.Crear(datos);
            return Result;
        }
    }
}
