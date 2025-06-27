using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnidadGenero.Class;
using Archivo.Models;
namespace Archivo.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ArchivoController : Controller 
    {
        private readonly ILogger<ArchivoController> _logger;

        ArchivoModels Archivo {  get; set; }
        public ArchivoController(ILogger<ArchivoController> logger)
        {
            _logger = logger;
            Archivo = new ArchivoModels();
        }
        [HttpPost()]
        public ArchivoResponse Get2([FromBody] ArchivoRequest datos)
        {
            ArchivoResponse Result = new ArchivoResponse();
            Result = Archivo.Crear(datos);
            return Result;
        }
    }
}
