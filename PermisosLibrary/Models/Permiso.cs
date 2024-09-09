namespace PermisosLibrary.Models
{
    public class Permiso
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public int Estado_id { get; set; }
        public int Roles_id { get; set; }
    }
}
