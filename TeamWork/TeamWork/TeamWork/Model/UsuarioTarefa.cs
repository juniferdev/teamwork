using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SQLiteNetExtensions.Attributes;

namespace TeamWork.Model
{
    public class UsuarioTarefa
    {
        [ForeignKey(typeof(Tarefa))]
        public int IdTarefa { get; set; }

        [ForeignKey(typeof(Usuario))]
        public int IdUsuario { get; set; }
    }
}
