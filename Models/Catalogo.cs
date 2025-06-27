using System;
using System.Collections.Generic;
using UnidadGenero.Class;
using SEV.Library;
namespace Catalogo.Models
{
    public class CatalogoModels
    {
        private const String SPObtenerAcciones = "SP_Obtener_Acciones";
        private const String SPObtenerActividades = "SP_Obtener_Actividades";
        private const String SPObtenerMedios = "SP_Obtener_Medios";
        private const String SPListaAreasEnlaces = "SP_Lista_Area_Enlance";
        private string SVCUNIDADGENERO { get; set; } = "sqlprodv21_UnidadadGenero";
        private string LOCALUNIDADGENERO {get; set;} = "local_UnidadGenero";
        public CatalogoModels() { }
        public Catalogos Obtener()
        {
            Catalogos Result = new Catalogos();
            Result.Acciones = Acciones();
            Result.Medios = Medios();
            return Result;
        }
        public List<Accion> Acciones()
        {
            List<Accion> Result = new List<Accion>();
            DataMapper<Accion> datos = new DataMapper<Accion>(SVCUNIDADGENERO);
            List<Accion> ResultData = datos.FromStoredProcedure
            (
                SPObtenerAcciones
            );
            if (ResultData.Count > 0)
            {
                Result = ResultData;
            }
            return Result;
        }
        public List<Actividad> Actividades()
        {
            List<Actividad> Result = new List<Actividad>();
            DataMapper<Actividad> datos = new DataMapper<Actividad>(SVCUNIDADGENERO);
            List<Actividad> ResultData = datos.FromStoredProcedure
            (
                SPObtenerActividades
            );
            if (ResultData.Count > 0)
            {
                Result = ResultData;
            }
            return Result;
        }
        public List<Medio> Medios()
        {
            List<Medio> Result = new List<Medio>();
            DataMapper<Medio> datos = new DataMapper<Medio>(SVCUNIDADGENERO);
            List<Medio> ResultData = datos.FromStoredProcedure
            (
                SPObtenerMedios
            );
            if (ResultData.Count > 0)
            {
                Result = ResultData;
            }
            return Result;
        }
        public List<AreaEnlace> AreasEnlaces()
        {
            List<AreaEnlace> Result = new List<AreaEnlace>();
            DataMapper<AreaEnlace> datos = new DataMapper<AreaEnlace>(SVCUNIDADGENERO);
            List<AreaEnlace> ResultData = datos.FromStoredProcedure
            (
                SPListaAreasEnlaces
            );
            if (ResultData.Count > 0)
            {
                Result = ResultData;
            }
            return Result;
        }
    }
    
}
