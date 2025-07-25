using System;
using System.Collections.Generic;
using UnidadGenero.Class;
using SEV.Library;
using Reporte.Models;
using Microsoft.Extensions.ObjectPool;
using Microsoft.OpenApi.Any;
namespace Configuracion.Models
{
    public class ConfiguracionModels
    {
        ReporteModels Reporte { get; set; }
        private const String SPActualizarTrimestre = "SP_Administrador_Actualizar_Trimestre";
        private string SVCUNIDADGENERO { get; set; } = "sqlprodv21_UnidadadGenero";
        public ConfiguracionModels()
        {
            Reporte = new ReporteModels();
        }
        public TrimestreResponse ActualizarTrimestre(int trimestre)
        {
            List<TrimestreResponse> Result = new List<TrimestreResponse>();
            DataMapper<TrimestreResponse> BDdatos = new DataMapper<TrimestreResponse>(SVCUNIDADGENERO);
            List<TrimestreResponse> ResultData = BDdatos.FromStoredProcedure
            (
                SPActualizarTrimestre,
                new List<DataParam>()
                {
                    new DataParam(){ Id = "@Trimestre", Value = trimestre, Type = System.Data.DbType.Int32 },
                }
            );
            if (ResultData.Count > 0)
            {
                Result = ResultData;
            }
            return Result[0];
        }
    }

}
