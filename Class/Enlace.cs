using SEV.Library;
namespace UnidadGenero.Class
{
    public class EnlaceResponse : RowMapper

    {
        public Int32 Id { get; set; }
        public String Enlace { get; set; }
        public String IdReporte { get; set; }
        public String Correo { get; set; }
        public DateTime Fecha { get; set; }
    }
    public class EnlaceRequest

    {
        public Int32 Id { get; set; }
        public String Enlace { get; set; }
        public String IdReporte { get; set; }
        public String Correo { get; set; }
    }
    public class EnlaceAcuse
    {
        public String NombreEnlace { get; set; }
		public String Correo { get; set; }
    }
}
