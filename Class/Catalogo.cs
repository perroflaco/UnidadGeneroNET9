using SEV.Library;
namespace UnidadGenero.Class
{
    public class Catalogos
    {
        public List<Accion> Acciones { get; set; }
        public List<Medio> Medios { get; set; }
    }
    public class Accion : RowMapper
    {
        public Int32 Id { get; set; }
        public string Descripcion { get; set; }
        public Int32 Mostrar { get; set; }
    }
    
    public class Actividad : RowMapper
    {
        public Int32 Id { get; set; }
        public string Descripcion { get; set; }
        public Int32 Activa { get; set; }
    }
    public class Medio : RowMapper
    {
        public Int32 Id { get; set; }
        public string Descripcion { get; set; }
    }
    public class AreaEnlace : RowMapper
    {
        public Int32 Id { get; set; }
        public string Nombre { get; set; }
    }
}
