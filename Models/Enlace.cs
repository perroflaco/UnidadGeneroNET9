using System;
using System.Collections.Generic;
using UnidadGenero.Class;
using SEV.Library;
namespace Enlace.Models
{
    public class EnlaceModels
    {
        private const String SPCrearEnlace = "SP_Crear_Enlace";
        private string SVCUNIDADGENERO { get; set; } = "sqlprodv21_UnidadadGenero";

        public EnlaceModels() { }
        public EnlaceResponse Crear(EnlaceRequest datos)
        {
            List<EnlaceResponse> Result = new List<EnlaceResponse>();
            DataMapper<EnlaceResponse> BDdatos = new DataMapper<EnlaceResponse>(SVCUNIDADGENERO);
            List<EnlaceResponse> ResultData = BDdatos.FromStoredProcedure
            (
                SPCrearEnlace,
                new List<DataParam>()
                {
                      new DataParam(){ Id = "@Id", Value = datos.Id, Type = System.Data.DbType.Int32 },
                      new DataParam(){ Id = "@Enlace", Value = datos.Enlace, Type = System.Data.DbType.String },
                      new DataParam(){ Id = "@IdReporte", Value = datos.IdReporte, Type = System.Data.DbType.String },
                      new DataParam(){ Id = "@Correo", Value = datos.Correo, Type = System.Data.DbType.String },
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
