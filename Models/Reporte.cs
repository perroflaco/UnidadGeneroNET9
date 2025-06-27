using System;
using System.Collections.Generic;
using UnidadGenero.Class;
using SEV.Library;
namespace Reporte.Models
{
    public class ReporteModels
    {
        private const String SPCrearReporte = "SP_Crear_Reporte";
        private const String SPListaReportes = "SP_Lista_Reportes";
        private const String SPObtenerReportes = "SP_Obtener_Reportes";
        private const String SPEliminarReporte = "SP_Eliminar_Reporte";
        private const String SPReporteGeneral = "SP_Adminitrador_Reporte_General";
        private string SVCUNIDADGENERO { get; set; } = "sqlprodv21_UnidadadGenero";
        public ReporteModels() { }
        public ReporteResponse Crear(ReporteRequest datos)
        {
            List<ReporteResponse> Result = new List<ReporteResponse>();
            DataMapper<ReporteResponse> BDdatos = new DataMapper<ReporteResponse>(SVCUNIDADGENERO);
            List<ReporteResponse> ResultData = BDdatos.FromStoredProcedure
            (
                SPCrearReporte,
                new List<DataParam>()
                {
                    new DataParam(){ Id = "@Id", Value = datos.Id, Type = System.Data.DbType.Int32 },
                    new DataParam(){ Id = "@IdAccion", Value = datos.IdAccion, Type = System.Data.DbType.Int32 },
                    new DataParam(){ Id = "@Actividad", Value = datos.Actividad, Type = System.Data.DbType.String },
                    new DataParam(){ Id = "@Trimestre", Value = datos.Trimestre, Type = System.Data.DbType.Int32 },
                    new DataParam(){ Id = "@IdArea", Value = datos.IdArea, Type = System.Data.DbType.Int32 },
                    new DataParam(){ Id = "@Area", Value = datos.Area, Type = System.Data.DbType.String },
                    new DataParam(){ Id = "@Hombres", Value = datos.Hombres, Type = System.Data.DbType.Int32 },
                    new DataParam(){ Id = "@Mujeres", Value = datos.Mujeres, Type = System.Data.DbType.Int32 },
                    new DataParam(){ Id = "@Descripcion", Value = datos.Descripcion, Type = System.Data.DbType.String},
                    new DataParam(){ Id = "@Cualitativos", Value = datos.Cualitativos, Type = System.Data.DbType.String },
                    new DataParam(){ Id = "@Porcentaje", Value = datos.Porcentaje, Type = System.Data.DbType.Int32 },
                    new DataParam(){ Id = "@IdMedio", Value = datos.IdMedio, Type = System.Data.DbType.String},
                    new DataParam(){ Id = "@Observaciones", Value = datos.Observaciones, Type = System.Data.DbType.String }
                }
            );
            if (ResultData.Count > 0)
            {
                Result = ResultData;
            }
            return Result[0];
        }
        public List<ReporteResponse> Lista()
        {
            List<ReporteResponse> Result = new List<ReporteResponse>();
            DataMapper<ReporteResponse> BDdatos = new DataMapper<ReporteResponse>(SVCUNIDADGENERO);
            List<ReporteResponse> ResultData = BDdatos.FromStoredProcedure
            (
                SPObtenerReportes
            );
            if (ResultData.Count > 0)
            {
                Result = ResultData;
            }
            return Result;
        }
        public List<ReporteResponse> Obtener(string correo)
        {
            List<ReporteResponse> Result = new List<ReporteResponse>();
            DataMapper<ReporteResponse> BDdatos = new DataMapper<ReporteResponse>(SVCUNIDADGENERO);
            List<ReporteResponse> ResultData = BDdatos.FromStoredProcedure
            (
                SPObtenerReportes,
                new List<DataParam>()
                {
                    new DataParam(){ Id = "@Correo", Value = correo, Type = System.Data.DbType.String},
                }
            );
            if (ResultData.Count > 0)
            {
                Result = ResultData;
            }
            return Result;
        }
        public EliminarReporteResponse Eliminar(int id)
        {
            DataMapper<EliminarReporteResponse> BDdatos = new DataMapper<EliminarReporteResponse>(SVCUNIDADGENERO);
            List<EliminarReporteResponse> ResultData = BDdatos.FromStoredProcedure
            (
                SPEliminarReporte,
                new List<DataParam>()
                {
                    new DataParam(){ Id = "@Id", Value = id, Type = System.Data.DbType.Int32 },

                }
            );
            return ResultData[0];
        }
        public List<ReporteAdminitradorGeneral> ObtenerReporteGeneral()
        {
            List<ReporteAdminitradorGeneral> Result = new List<ReporteAdminitradorGeneral>();
            ReporteAdminitradorGeneral Estructura = new ReporteAdminitradorGeneral();
            Estructura.Areas = new List<ReporteAdminitradorArea>();
            DataMapper<ReporteAdminitradorGeneralBD> BDdatos = new DataMapper<ReporteAdminitradorGeneralBD>(SVCUNIDADGENERO);
            List<ReporteAdminitradorGeneralBD> ResultData = BDdatos.FromStoredProcedure
            (
                SPReporteGeneral
            );
            if (ResultData.Count > 0)
            {
                ResultData.ForEach(r =>
                {
                    Estructura.IdAccion = r.IdAccion;
                    Estructura.Accion = r.Accion;
                    Result.Add(Estructura);

                });
            }

            return Result;
        }
    }

}
