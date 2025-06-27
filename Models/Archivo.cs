using System;
using System.Collections.Generic;
using UnidadGenero.Class;
using SEV.Library;
namespace Archivo.Models
{
    public class ArchivoModels
    {
        private const String SPCrearArchivo = "SP_Crear_Archivo";
        private string SVCUNIDADGENERO { get; set; } = "local_UnidadGenero";
        public ArchivoModels() { }
        public ArchivoResponse Crear(ArchivoRequest datos)
        {
            List<ArchivoResponse> Result = new List<ArchivoResponse>();
            DataMapper<ArchivoResponse> BDdatos = new DataMapper<ArchivoResponse>(SVCUNIDADGENERO);
            List<ArchivoResponse> ResultData = BDdatos.FromStoredProcedure
            (
                SPCrearArchivo,
                new List<DataParam>()
                {
                      new DataParam(){ Id = "@Id", Value = datos.Id, Type = System.Data.DbType.Int32 },
                      new DataParam(){ Id = "@IdReporte", Value = datos.IdReporte, Type = System.Data.DbType.String },
                      new DataParam(){ Id = "@Nombre", Value = datos.Nombre, Type = System.Data.DbType.String },
                      new DataParam(){ Id = "@Ruta", Value = datos.Ruta, Type = System.Data.DbType.String },
                }
            );
            if(ResultData.Count > 0)
            {
                Result = ResultData;
            }
            return Result[0];
        }
    }

}
