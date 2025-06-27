using SEV.Library;
namespace UnidadGenero.Class
{
    public class AcuseResponse:RowMapper

    {
        public Int32 Id { get; set; }
        public String IdReporte {  get; set; }
        public DateTime Fecha { get; set; }
    }
    public class AcuseRequets:RowMapper

    {
        public Int32 Id { get; set; }
        public String IdReporte {  get; set; }
    }
    public class AcuseInfoResponse:RowMapper
    {
        public Int32 Id { get; set; }
		public DateTime FechaCreacion { get; set; }
		public Int32 IdReporte { get; set; }
		public Int32 IdAccion { get; set; }
		public String NombreAccion { get; set; }
		public String Actividad { get; set; }
		public Int32 Trimestre { get; set; }
		public Int32 IdArea { get; set; }
		public String Area { get; set; } 
		public Int32 Hombres { get; set; } 
		public Int32 Mujeres { get; set; } 
		public String Descripcion { get; set; }
		public String Cualitativos { get; set; }
		public Int32 Porcentaje { get; set; }
		public String IdMedio { get; set; }
		public String Observaciones { get; set; }
		public DateTime FechaCreacionReporte { get; set; } 
		public DateTime FechaModificacion { get; set; } 
		public String NombreEnlace { get; set; }
		public String Correo { get; set; }
		public String Nombre { get; set; }
		public String Ruta { get; set; }
    }
    public class AcuseReporteDatos
    {
        public Int32 Id { get; set; }
		public DateTime FechaCreacion { get; set; }
    }
    public class AcuseReporte
    {
        public AcuseReporteDatos Acuses { get; set; }
        public List<ReporteAcuse> Reportes { get; set; }
        public EnlaceAcuse Enlace { get; set; }
        public ArchivoAcuse Archivo { get; set; }

    }
}
