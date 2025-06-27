using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnidadGenero.Class;
using Enlace.Models;
namespace Enlace.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EnlaceController : Controller 
    {
        private readonly ILogger<EnlaceController> _logger;

        EnlaceModels Enlace {  get; set; }
        public EnlaceController(ILogger<EnlaceController> logger)
        {
            _logger = logger;
            Enlace = new EnlaceModels();
        }
        [HttpPost()]
        public EnlaceResponse Get2([FromBody] EnlaceRequest datos)
        {
            EnlaceResponse Result = new EnlaceResponse();
            Result = Enlace.Crear(datos);
            return Result;
        }
    }
}
