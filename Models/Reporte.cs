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
        private const String SPReporteGeneral = "SP_Adminitrador_Reporte_General_parametros";
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
        public List<ReporteAdminitradorGeneral> ObtenerReporteGeneral(int trimestre,int anio)
        {
            List<ReporteAdminitradorGeneral> Result = new List<ReporteAdminitradorGeneral>();
            DataMapper<ReporteAdminitradorGeneralBD> BDdatos = new DataMapper<ReporteAdminitradorGeneralBD>(SVCUNIDADGENERO);
            List<ReporteAdminitradorGeneralBD> ResultData = BDdatos.FromStoredProcedure
            (
                SPReporteGeneral,
                new List<DataParam>()
                {
                    new DataParam(){ Id = "@Trimestre", Value = trimestre, Type = System.Data.DbType.Int32 },
                    new DataParam(){ Id = "@Anio", Value = anio, Type = System.Data.DbType.Int32 }

                }
            );
            if (ResultData.Count > 0)
            {
                ResultData.ForEach(r =>
                {
                    var existe = Result.Find(ra => ra.IdAccion == r.IdAccion);
                    if (existe == null)
                    {
                        ReporteAdminitradorGeneral accion = new ReporteAdminitradorGeneral();
                        accion.IdAccion = r.IdAccion;
                        accion.Accion = r.Accion;
                        accion.Areas = new List<ReporteAdminitradorArea>();
                        accion.TotalParticipantes = new List<TotalParticipantes>();
                        ReporteAdminitradorArea area = new ReporteAdminitradorArea();
                        area.IdArea = r.Id;
                        area.Area = r.Nombre;
                        area.Actividades = new List<ReporteAdminitradorActivdad>();
                        accion.Areas.Add(area);
                        ReporteAdminitradorActivdad activdad = new ReporteAdminitradorActivdad();
                        activdad.IdActividad = r.IdActividad;
                        activdad.Actividad = r.Actividad;
                        activdad.Hombres = r.Hombres;
                        activdad.Mujeres = r.Mujeres;
                        activdad.Descripcion = r.Descripcion;
                        activdad.ResultadosCualitativos = r.Cualitativos;
                        activdad.MediosVerificacion = r.IdMedio;
                        activdad.Observaciones = r.Observaciones;
                        area.Actividades.Add(activdad);
                        TotalParticipantes participantes = new TotalParticipantes();
                        participantes.IdActividad = r.IdActividad;
                        participantes.Actividad = r.Actividad;
                        participantes.Hombres = r.Hombres;
                        participantes.Mujeres = r.Mujeres;
                        accion.TotalParticipantes.Add(participantes);
                        Result.Add(accion);
                    }
                    else
                    {
                        var indexgeneral = Result.FindIndex(r => r.IdAccion == r.IdAccion);
                        var existearea = Result[indexgeneral].Areas.Find(a => a.IdArea == r.Id);
                        if (existearea == null)
                        {
                            ReporteAdminitradorArea areanueva = new ReporteAdminitradorArea();
                            areanueva.IdArea = r.Id;
                            areanueva.Area = r.Nombre;
                            areanueva.Actividades = new List<ReporteAdminitradorActivdad>();
                            Result[indexgeneral].Areas.Add(areanueva);
                            var indexarea = Result[indexgeneral].Areas.FindIndex(aa => aa.IdArea == r.Id);
                            var misma = Result[indexgeneral].IdAccion == r.IdAccion ? true : false;
                            if (misma)
                            {
                                ReporteAdminitradorActivdad activdadnueva = new ReporteAdminitradorActivdad();
                                activdadnueva.IdActividad = r.IdActividad;
                                activdadnueva.Actividad = r.Actividad;
                                activdadnueva.Hombres = r.Hombres;
                                activdadnueva.Mujeres = r.Mujeres;
                                activdadnueva.Descripcion = r.Descripcion;
                                activdadnueva.ResultadosCualitativos = r.Cualitativos;
                                activdadnueva.MediosVerificacion = r.IdMedio;
                                activdadnueva.Observaciones = r.Observaciones;
                                Result[indexgeneral].Areas[indexarea].Actividades.Add(activdadnueva);
                                var indextotal = Result[indexgeneral].TotalParticipantes.FindIndex(t => t.IdActividad == r.IdActividad);
                                if (indextotal != -1)
                                {
                                    Result[indexgeneral].TotalParticipantes[indextotal].Hombres = Result[indexgeneral].TotalParticipantes[indextotal].Hombres + r.Hombres;
                                    Result[indexgeneral].TotalParticipantes[indextotal].Mujeres = Result[indexgeneral].TotalParticipantes[indextotal].Mujeres + r.Mujeres;

                                }
                                else
                                {
                                    TotalParticipantes participantes = new TotalParticipantes();
                                    participantes.IdActividad = r.IdActividad;
                                    participantes.Actividad = r.Actividad;
                                    participantes.Hombres = r.Hombres;
                                    participantes.Mujeres = r.Mujeres;
                                    Result[indexgeneral].TotalParticipantes.Add(participantes);
                                }

                            }
                        }
                        else
                        {
                            var indexarea = Result[indexgeneral].Areas.FindIndex(aa => aa.IdArea == r.Id);
                            var misma = Result[indexgeneral].IdAccion == r.IdAccion ? true : false;
                            if (misma)
                            {
                                ReporteAdminitradorActivdad activdadnueva = new ReporteAdminitradorActivdad();
                                activdadnueva.IdActividad = r.IdActividad;
                                activdadnueva.Actividad = r.Actividad;
                                activdadnueva.Hombres = r.Hombres;
                                activdadnueva.Mujeres = r.Mujeres;
                                activdadnueva.Descripcion = r.Descripcion;
                                activdadnueva.ResultadosCualitativos = r.Cualitativos;
                                activdadnueva.MediosVerificacion = r.IdMedio;
                                activdadnueva.Observaciones = r.Observaciones;
                                Result[indexgeneral].Areas[indexarea].Actividades.Add(activdadnueva);
                                var indextotal = Result[indexgeneral].TotalParticipantes.FindIndex(t => t.IdActividad == r.IdActividad);
                                if (indextotal != -1)
                                {
                                    Result[indexgeneral].TotalParticipantes[indextotal].Hombres = Result[indexgeneral].TotalParticipantes[indextotal].Hombres + r.Hombres;
                                    Result[indexgeneral].TotalParticipantes[indextotal].Mujeres = Result[indexgeneral].TotalParticipantes[indextotal].Mujeres + r.Mujeres;

                                }
                                else
                                {
                                    TotalParticipantes participantes = new TotalParticipantes();
                                    participantes.IdActividad = r.IdActividad;
                                    participantes.Actividad = r.Actividad;
                                    participantes.Hombres = r.Hombres;
                                    participantes.Mujeres = r.Mujeres;
                                    Result[indexgeneral].TotalParticipantes.Add(participantes);
                                }
                            }
                        }
                    }

                });
            }

            return Result;
        }
    }

}
