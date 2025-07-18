using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnidadGenero.Class;
using Reporte.Models;
namespace Reporte.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ReporteController : Controller 
    {
        private readonly ILogger<ReporteController> _logger;

        ReporteModels Reporte {  get; set; }
        public ReporteController(ILogger<ReporteController> logger)
        {
            _logger = logger;
            Reporte = new ReporteModels();
        }
        [HttpGet()]
        public List<ReporteResponse> Get()
        {
            List<ReporteResponse> Result = new List<ReporteResponse>();
            Result = Reporte.Lista();
            return Result;
        }

        [HttpGet("administrador/general")]
        public List<ReporteAdminitradorGeneral> Get1( [FromQuery] int trimestre =0, [FromQuery] int anio=0)
        {
            List<ReporteAdminitradorGeneral> Result = new List<ReporteAdminitradorGeneral>();
            Result = Reporte.ObtenerReporteGeneral(trimestre,anio);
            return Result;
        }
        
        [HttpGet("{trimestre}/{idarea}")]
        public List<ReportesTrimestreArea> Get2(int trimestre =0,int idarea=0)
        {
            List<ReportesTrimestreArea> Result = new List<ReportesTrimestreArea>();
            Result = Reporte.ReportesTrimestreArea(trimestre,idarea);
            return Result;
        }
        [HttpGet("estado/{trimestre}/{idarea}")]
        public EstadoTrimestreReporte Get(int trimestre,int idarea)
        {
            EstadoTrimestreReporte Result = new EstadoTrimestreReporte();
            Result = Reporte.EstadoReporteTrimestre(trimestre,idarea);
            return Result;
        }
        
        [HttpPut("finalizado/{idarea}")]
        public ActualizarResponse Put(int idarea)
        {
            ActualizarResponse Result = new ActualizarResponse();
            Result = Reporte.ReporteFinalizado(idarea);
            return Result;
        }
        [HttpPost()]
        public ReporteResponse Post([FromBody] ReporteRequest datos)
        {
            ReporteResponse Result = new ReporteResponse();
            Result = Reporte.Crear(datos);
            return Result;
        }
        [HttpDelete("{id}")]
        public EliminarReporteResponse Delete(int id)
        {
            EliminarReporteResponse Result = new EliminarReporteResponse();
            Result = Reporte.Eliminar(id);
            return Result;
        }
    }
}
