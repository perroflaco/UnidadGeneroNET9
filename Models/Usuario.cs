using System;
using System.Collections.Generic;
using UnidadGenero.Class;
using SEV.Library;
using Reporte.Models;
using Microsoft.Extensions.ObjectPool;
using Microsoft.OpenApi.Any;
namespace Usuario.Models
{
   public class UsuarioModels
    {
        private const string SPCrearUsuario = "SP_Crear_Usuario"; // Nombre del SP para crear usuario
        private string SVCUNIDADGENERO { get; set; } = "sqlprodv21_UnidadadGenero";

        public UsuarioModels() { }

        public UsuarioResponse Crear(UsuarioRequest datos)
        {
            List<UsuarioResponse> result = new List<UsuarioResponse>();
            DataMapper<UsuarioResponse> BDdatos = new DataMapper<UsuarioResponse>(SVCUNIDADGENERO);

            List<UsuarioResponse> resultData = BDdatos.FromStoredProcedure
            (
                SPCrearUsuario,
                new List<DataParam>()
                {
                    new DataParam() { Id = "@nombre", Value = datos.Nombre, Type = System.Data.DbType.String },
                    new DataParam() { Id = "@apellidoPaterno", Value = datos.ApellidoPaterno, Type = System.Data.DbType.String },
                    new DataParam() { Id = "@apellidoMaterno", Value = datos.ApellidoMaterno, Type = System.Data.DbType.String },
                    new DataParam() { Id = "@correo", Value = datos.Correo, Type = System.Data.DbType.String },
                    new DataParam() { Id = "@sexo", Value = datos.Sexo, Type = System.Data.DbType.String },
                    new DataParam() { Id = "@rol", Value = datos.Rol, Type = System.Data.DbType.String },
                    new DataParam() { Id = "@fechaRegistro", Value = datos.FechaRegistro, Type = System.Data.DbType.Date },
                    new DataParam() { Id = "@activo", Value = datos.Activo, Type = System.Data.DbType.Int32 },
                    new DataParam() { Id = "@idAreaEnlace", Value = datos.IdAreaEnlace, Type = System.Data.DbType.Int32 }
                }
            );

            if (resultData == null || resultData.Count == 0)
            {
                throw new Exception("Error al crear el usuario. El procedimiento no devolvió ningún resultado.");
            }

            return resultData[0];

        }

        public List<UsuarioResponse> ObtenerTodos()
        {
            DataMapper<UsuarioResponse> BDdatos = new DataMapper<UsuarioResponse>(SVCUNIDADGENERO);
            List<UsuarioResponse> resultData = BDdatos.FromStoredProcedure("SP_Obtener_Usuarios", new List<DataParam>());

            if (resultData == null)
            {
                Console.WriteLine("resultData es null");
            }
            else if (resultData.Count == 0)
            {
                Console.WriteLine("resultData está vacío");
            }
            else
            {
                Console.WriteLine($"resultData tiene {resultData.Count} registros");
            }

            return resultData ?? new List<UsuarioResponse>();
        }

        public UsuarioResponse BuscarPorCorreo(String correo)
        {
            const string SP_OBTENER_POR_ID = "SP_Obtener_Usuario_Por_Correo";

            DataMapper<UsuarioResponse> BDdatos = new DataMapper<UsuarioResponse>(SVCUNIDADGENERO);

            List<UsuarioResponse> resultData = BDdatos.FromStoredProcedure
            (
                SP_OBTENER_POR_ID,
                new List<DataParam>()
                {
            new DataParam() { Id = "@correo", Value = correo, Type = System.Data.DbType.String }
                }
            );

            if (resultData == null || resultData.Count == 0)
            {
                return null;
            }

            return resultData[0];
        }

        public UsuarioResponse Editar(UsuarioRequest datos)
        {
            const string SP_EDITAR_USUARIO = "SP_Editar_Usuario";

            DataMapper<UsuarioResponse> BDdatos = new DataMapper<UsuarioResponse>(SVCUNIDADGENERO);

            List<UsuarioResponse> resultData = BDdatos.FromStoredProcedure
            (
                SP_EDITAR_USUARIO,
                new List<DataParam>()
                {
            new DataParam() { Id = "@idUsuario", Value = datos.IdUsuario, Type = System.Data.DbType.Int64 },
            new DataParam() { Id = "@nombre", Value = datos.Nombre, Type = System.Data.DbType.String },
            new DataParam() { Id = "@apellidoPaterno", Value = datos.ApellidoPaterno, Type = System.Data.DbType.String },
            new DataParam() { Id = "@apellidoMaterno", Value = datos.ApellidoMaterno, Type = System.Data.DbType.String },
            new DataParam() { Id = "@correo", Value = datos.Correo, Type = System.Data.DbType.String },
            new DataParam() { Id = "@sexo", Value = datos.Sexo, Type = System.Data.DbType.String },
            new DataParam() { Id = "@rol", Value = datos.Rol, Type = System.Data.DbType.String },
            new DataParam() { Id = "@fechaRegistro", Value = datos.FechaRegistro, Type = System.Data.DbType.Date },
            new DataParam() { Id = "@activo", Value = datos.Activo, Type = System.Data.DbType.Int32 },
            new DataParam() { Id = "@idAreaEnlace", Value = datos.IdAreaEnlace, Type = System.Data.DbType.Int32 }
                }
            );

            if (resultData == null || resultData.Count == 0)
            {
                throw new Exception("No se encontró el usuario o no se pudo actualizar.");
            }

            return resultData[0];
        }

        public UsuarioResponse ActualizarActivo(long idUsuario, int activo)
        {
            const string SP_ACTUALIZAR_ACTIVO = "SP_Actualizar_Estatus_Usuario";

            DataMapper<UsuarioResponse> BDdatos = new DataMapper<UsuarioResponse>(SVCUNIDADGENERO);

            List<UsuarioResponse> resultData = BDdatos.FromStoredProcedure
            (
                SP_ACTUALIZAR_ACTIVO,
                new List<DataParam>()
                {
            new DataParam() { Id = "@idUsuario", Value = idUsuario, Type = System.Data.DbType.Int64 },
            new DataParam() { Id = "@activo", Value = activo, Type = System.Data.DbType.Int32 }
                }
            );

            if (resultData == null || resultData.Count == 0)
            {
                throw new Exception("No se pudo actualizar el estado activo del usuario.");
            }

            return resultData[0];
        }

    }
}
