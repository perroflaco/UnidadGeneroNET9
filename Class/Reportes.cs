using SEV.Library;
namespace UnidadGenero.Class
{
    public class ReporteResponse : RowMapper

    {
        public Int32 Id { get; set; }
        public Int32 IdAccion { get; set; }
        public String? NombreAccion { get; set; }
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
        public DateTime FechaCreacion { get; set; }
        public DateTime FechaModificacion { get; set; }
    }
    public class ReporteRequest

    {
        public Int32 Id { get; set; }
        public Int32 IdAccion { get; set; }
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
    }
    public class EliminarReporteResponse : RowMapper
    {
        public String Eliminado { get; set; }
    }
    public class ReporteAcuse
    {
        public String Accion { get; set; }
        public List<ReporteResponse> Reportes { get; set; }
    }
    public class ReporteAdminitradorGeneralBD : RowMapper
    {
        public Int32 Id { get; set; }
        public String Nombre { get; set; }
        public Int32 IdAccion { get; set; }
        public String Accion { get; set; }
        public String IdActividad { get; set; }
        public String Actividad { get; set; }
        public Int32 Hombres { get; set; }
        public Int32 Mujeres { get; set; }
        public String Descripcion { get; set; }
        public String Cualitativos { get; set; }
        public String IdMedio { get; set; }
        public String Observaciones { get; set; }

    }
    public class ReporteAdminitradorGeneral
    {
        public Int32 IdAccion { get; set; }
        public String Accion { get; set; }
        public List<ReporteAdminitradorArea> Areas { get; set; }
        public List<TotalParticipantes> TotalParticipantes { get; set; }

    }
    public class ReporteAdminitradorArea
    {
        public Int32 IdArea { get; set; }
        public String Area { get; set; }
        public List<ReporteAdminitradorActivdad> Actividades { get; set; }
    }
    public class ReporteAdminitradorActivdad
    {
        public String IdActividad { get; set; }
        public String Actividad { get; set; }
        public Int32 Hombres { get; set; }
        public Int32 Mujeres { get; set; }
        public String Descripcion { get; set; }
        public String ResultadosCualitativos { get; set; }
        public String MediosVerificacion { get; set; }
        public String Observaciones { get; set; }
    }
    public class TotalParticipantes
    {
        public String IdActividad { get; set; }
        public String Actividad { get; set; }
        public Int32 Hombres { get; set; }
        public Int32 Mujeres { get; set; }
        
    }
    
}
