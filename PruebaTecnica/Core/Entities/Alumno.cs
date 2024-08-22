namespace Core.Entities
{
    public class Alumno
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string Apellido { get; set; }
        public DateTime FechaNacimiento { get; set; }
        public int IdGenero{ get; set; }
        public int IdTipoDocumento { get; set; }
        public string NumeroDocumento { get; set; }
    }
}
