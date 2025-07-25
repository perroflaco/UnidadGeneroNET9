using SEV.Library;
namespace UnidadGenero.Class
{
    public class TrimestreRequest
    {
        public Int32 Trimestre{ get; set; }
    }
    public class TrimestreResponse : RowMapper

    {
        public Int32 Id { get; set; }
        public String Nombre { get; set; }
        public Int32 Activo { get; set; }
    }
    
}
