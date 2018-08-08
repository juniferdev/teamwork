using SQLiteNetExtensions.Attributes;

namespace TeamWork.Model
{
    public class UsuarioGrupo
    {

       [ForeignKey(typeof(Usuario))]
       public int IdUsuario { get; set; }

       [ForeignKey(typeof(Grupo))]
       public int IdGrupo { get; set; }

    }
}
