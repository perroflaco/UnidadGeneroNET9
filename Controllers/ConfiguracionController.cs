using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnidadGenero.Class;
using Configuracion.Models;
namespace Configuracion.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ConfiguracionController : Controller 
    {
        private readonly ILogger<ConfiguracionController> _logger;

        ConfiguracionModels Configuracion {  get; set; }
        public ConfiguracionController(ILogger<ConfiguracionController> logger)
        {
            _logger = logger;
            Configuracion = new ConfiguracionModels();
        }
        [HttpPut("actualizar/trimestre")]
        public ActionResult<TrimestreResponse> ActualizarUsuario([FromBody] TrimestreRequest datos)
        {
            try
            {
                var trimestreActualizado =Configuracion.ActualizarTrimestre(datos.Trimestre);
                return Ok(trimestreActualizado);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error al actualizar trimestre: {ex.Message}");
            }
        }
    }
}
