using System;
using System.Collections.Generic;
using UnidadGenero.Class;
using SEV.Library;
using Reporte.Models;
using Microsoft.Extensions.ObjectPool;
using Microsoft.OpenApi.Any;
namespace Acuse.Models
{
    public class AcuseModels
    {
        ReporteModels Reporte { get; set; } 
        private const String SPCrearAcuse = "SP_Crear_Acuse";
        public const String SPObtenerAcuses = "SP_Obtener_Acuses";
        private string SVCUNIDADGENERO { get; set; } = "sqlprodv21_UnidadadGenero";
        public AcuseModels() {
            Reporte = new ReporteModels();
         }
        public AcuseResponse Crear(AcuseRequets datos)
        {
            List<AcuseResponse> Result = new List<AcuseResponse>();
            DataMapper<AcuseResponse> BDdatos = new DataMapper<AcuseResponse>(SVCUNIDADGENERO);
            List<AcuseResponse> ResultData = BDdatos.FromStoredProcedure
            (
                SPCrearAcuse,
                new List<DataParam>()
                {
                      new DataParam(){ Id = "@Id", Value = datos.Id, Type = System.Data.DbType.Int32 },
                      new DataParam(){ Id = "@IdReporte", Value = datos.IdReporte, Type = System.Data.DbType.String },
                }
            );
            if(ResultData.Count > 0)
            {
                Result = ResultData;
            }
            return Result[0];
        }
        public List<AcuseReporte> Obtener(string correo)
        {
            List<AcuseReporte> Result = new List<AcuseReporte>();
            List<AcuseInfoResponse> DatosDB = new List<AcuseInfoResponse>();
            List<ReporteAcuse> Estructura = new List<ReporteAcuse>();
            DatosDB = ObtenerAcuseReporte(correo);                 
            DatosDB.ForEach(x =>
            {
                AcuseReporte? acuse = Result.Find(a => a.Acuses.Id == x.Id);
                if (acuse == null)
                {
                    AcuseReporte dato = new AcuseReporte();
                    dato.Acuses = new AcuseReporteDatos();
                    dato.Reportes = new List<ReporteAcuse>();
                    dato.Enlace = new EnlaceAcuse();
                    dato.Archivo = new ArchivoAcuse();
                    dato.Acuses.Id = x.Id;
                    dato.Acuses.FechaCreacion = x.FechaCreacion;
                    ReporteAcuse reporte = new ReporteAcuse();
                    reporte.Reportes = new List<ReporteResponse>();
                    reporte.Accion = x.NombreAccion;
                    ReporteResponse response = new ReporteResponse();
                    response.Id = x.IdReporte;
                    response.IdAccion = x.IdAccion;
                    response.NombreAccion = x.NombreAccion;
                    response.Actividad = x.Actividad;
                    response.Trimestre = x.Trimestre;
                    response.IdArea = x.IdArea;
                    response.Area = x.Area;
                    response.Hombres = x.Hombres;
                    response.Mujeres = x.Mujeres;
                    response.Descripcion = x.Descripcion;
                    response.Cualitativos = x.Cualitativos;
                    response.Porcentaje = x.Porcentaje;
                    response.IdMedio = x.IdMedio;
                    response.Observaciones = x.Observaciones;
                    response.FechaCreacion = x.FechaCreacionReporte;
                    response.FechaModificacion = x.FechaModificacion;
                    ReporteAcuse? estaaccion = dato.Reportes.Find(a => a.Accion == x.NombreAccion);
                    if (estaaccion == null)
                    {
                        reporte.Reportes.Add(response);
                        dato.Reportes.Add(reporte);
                    }
                    else
                    {
                        int index = dato.Reportes.FindIndex(a => a.Accion == x.NombreAccion);
                        dato.Reportes[index].Reportes.Add(response);

                    }
                    dato.Enlace.NombreEnlace = x.NombreEnlace;
                    dato.Enlace.Correo = x.Correo;
                    dato.Archivo.Nombre = x.Nombre;
                    dato.Archivo.Ruta = x.Ruta;
                    Result.Add(dato);

                }
                else
                {
                    int indexgeneral = Result.FindIndex(a => a.Acuses.Id == x.Id);
                    int indexreporte = Result[indexgeneral].Reportes.FindIndex(r => r.Accion == x.NombreAccion);
                    if (indexreporte == -1)
                    {
                        ReporteAcuse reporte = new ReporteAcuse();
                        reporte.Reportes = new List<ReporteResponse>();
                        reporte.Accion = x.NombreAccion;
                        ReporteResponse response = new ReporteResponse();
                        response.Id = x.IdReporte;
                        response.IdAccion = x.IdAccion;
                        response.NombreAccion = x.NombreAccion;
                        response.Actividad = x.Actividad;
                        response.Trimestre = x.Trimestre;
                        response.IdArea = x.IdArea;
                        response.Area = x.Area;
                        response.Hombres = x.Hombres;
                        response.Mujeres = x.Mujeres;
                        response.Descripcion = x.Descripcion;
                        response.Cualitativos = x.Cualitativos;
                        response.Porcentaje = x.Porcentaje;
                        response.IdMedio = x.IdMedio;
                        response.Observaciones = x.Observaciones;
                        response.FechaCreacion = x.FechaCreacionReporte;
                        response.FechaModificacion = x.FechaModificacion;
                        reporte.Reportes.Add(response);
                        Result[indexgeneral].Reportes.Add(reporte);
                    }
                    else
                    {
                        ReporteResponse response = new ReporteResponse();
                        response.Id = x.IdReporte;
                        response.IdAccion = x.IdAccion;
                        response.NombreAccion = x.NombreAccion;
                        response.Actividad = x.Actividad;
                        response.Trimestre = x.Trimestre;
                        response.IdArea = x.IdArea;
                        response.Area = x.Area;
                        response.Hombres = x.Hombres;
                        response.Mujeres = x.Mujeres;
                        response.Descripcion = x.Descripcion;
                        response.Cualitativos = x.Cualitativos;
                        response.Porcentaje = x.Porcentaje;
                        response.IdMedio = x.IdMedio;
                        response.Observaciones = x.Observaciones;
                        response.FechaCreacion = x.FechaCreacionReporte;
                        response.FechaModificacion = x.FechaModificacion;
                        Result[indexgeneral].Reportes[indexreporte].Reportes.Add(response);
                    }

                     
                    
                }
                

            /*if (dato.Acuses.Id != x.Id)
                    {
                        dato.Acuses.Id = x.Id;
                        dato.Acuses.FechaCreacion = x.FechaCreacion;
                    }
                    ReporteAcuse reporte = new ReporteAcuse();
                    reporte.Reportes = new List<ReporteResponse>();
                    reporte.Accion = x.NombreAccion;
                    ReporteResponse response = new ReporteResponse();
                    response.Id = x.IdReporte;
                    response.IdAccion = x.IdAccion;
                    response.NombreAccion = x.NombreAccion;
                    response.Actividad = x.Actividad;
                    response.Trimestre = x.Trimestre;
                    response.IdArea = x.IdArea;
                    response.Area = x.Area;
                    response.Mujeres = x.Mujeres;
                    response.Descripcion = x.Descripcion;
                    response.Cualitativos = x.Cualitativos;
                    response.Porcentaje = x.Porcentaje;
                    response.IdMedio = x.IdMedio;
                    response.Observaciones = x.Observaciones;
                    response.FechaCreacion = x.FechaCreacionReporte;
                    response.FechaModificacion = x.FechaModificacion;
                    ReporteAcuse? estaaccion = dato.Reportes.Find(a => a.Accion == x.NombreAccion);
                    if (estaaccion == null)
                    {
                        reporte.Reportes.Add(response);
                        dato.Reportes.Add(reporte);
                    }
                    else
                    {
                        int index = dato.Reportes.FindIndex(a => a.Accion == x.NombreAccion);
                        dato.Reportes[index].Reportes.Add(response);   

                    }
                    {
                        dato.Enlace.NombreEnlace = x.NombreEnlace;
                        dato.Enlace.Correo = x.Correo;
                    }
                    if (dato.Archivo.Nombre == null)
                    {
                        dato.Archivo.Nombre = x.Nombre;
                        dato.Archivo.Ruta = x.Ruta;
                    }*/



            });
            return Result;
        }
        public List<AcuseInfoResponse> ObtenerAcuseReporte(string correo)
        {
            List<AcuseInfoResponse> Result = new List<AcuseInfoResponse>();
            DataMapper<AcuseInfoResponse> BDdatos = new DataMapper<AcuseInfoResponse>(SVCUNIDADGENERO);
            List<AcuseInfoResponse> ResultData = BDdatos.FromStoredProcedure
            (
                SPObtenerAcuses,
                new List<DataParam>()
                {
                    new DataParam(){ Id = "@Correo", Value = correo, Type = System.Data.DbType.String },
                }
            );
            if(ResultData.Count > 0)
            {
                Result = ResultData;
            }
            return Result;
        }
    }

}
