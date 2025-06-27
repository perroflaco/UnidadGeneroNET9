using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnidadGenero.Class;
using Catalogo.Models;
namespace Catalogo.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CatalogoController : Controller 
    {
        private readonly ILogger<CatalogoController> _logger;
        CatalogoModels Catalogo {  get; set; }
        public CatalogoController(ILogger<CatalogoController> logger)
        {
            _logger = logger;
            Catalogo = new CatalogoModels();
        }
        [HttpGet()]
        public Catalogos Get()
        {
            Catalogos Result = new Catalogos();
            Result = Catalogo.Obtener();
            return Result;
        }
        [HttpGet("acciones")]
        public List<Accion> Get1()
        {
            List<Accion> Result = new List<Accion>();
            Result = Catalogo.Acciones();
            return Result;
        }
        [HttpGet("actividades")]
        public List<Actividad> Get4()
        {
            List<Actividad> Result = new List<Actividad>();
            Result = Catalogo.Actividades();
            return Result;
        }
        [HttpGet("medios")]
        public List<Medio> Get2()
        {
            List<Medio> Result = new List<Medio>();
            Result = Catalogo.Medios();
            return Result;
        }
        [HttpGet("areasenlaces")]
        public List<AreaEnlace> Get3()
        {
            List<AreaEnlace> Result = new List<AreaEnlace>();
            Result = Catalogo.AreasEnlaces();
            return Result;
        }
    }
}
