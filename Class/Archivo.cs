using SEV.Library;
namespace UnidadGenero.Class
{
    public class ArchivoResponse : RowMapper

    {
        public Int32 Id { get; set; }
        public String IdReporte { get; set; }
        public String Nombre { get; set; }
        public String Ruta { get; set; }
        public DateTime FechaCreacion { get; set; }
        public DateTime FechaModificacion { get; set; }
    }
    public class ArchivoRequest

    {
        public Int32 Id { get; set; }
        public String IdReporte { get; set; }
        public String Nombre { get; set; }
        public String Ruta { get; set; }
    }
    public class ArchivoAcuse
    {
        public String Nombre { get; set; }
		public String Ruta { get; set; }
    }
}
