using SEV.Library;
namespace UnidadGenero.Class
{
    public class UsuarioResponse : RowMapper
    {
        public long IdUsuario { get; set; }
        public string Nombre { get; set; }
        public string ApellidoPaterno { get; set; }
        public string ApellidoMaterno { get; set; }
        public string Correo { get; set; }
        public string Sexo { get; set; }
        public string Rol { get; set; }
        public DateTime FechaRegistro { get; set; }
        public int Activo { get; set; }
        public int IdAreaEnlace { get; set; }
    }

    public class UsuarioRequest
    {
        public long? IdUsuario { get; set; }
        public string Nombre { get; set; }
        public string ApellidoPaterno { get; set; }
        public string ApellidoMaterno { get; set; }
        public string Correo { get; set; }
        public string Sexo { get; set; }
        public string Rol { get; set; }
        public DateTime FechaRegistro { get; set; }
        public int Activo { get; set; }
        public int IdAreaEnlace { get; set; }
    }
}
