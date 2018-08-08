using SQLiteNetExtensions.Attributes;
using System;
using System.Collections.Generic;
using System.Text;
using TeamWork.Internal;

namespace TeamWork.Model
{
    public class UsuarioProjeto
    {

        [ForeignKey(typeof(Projeto))]
        public int IdProjeto { get; set; }

        [ForeignKey(typeof(Usuario))]
        public int IdUsuario { get; set; }

    }
}