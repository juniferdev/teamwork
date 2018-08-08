using SQLite.Net.Attributes;
using SQLiteNetExtensions.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TeamWork.Internal;

namespace TeamWork.Model
{
    public class Usuario
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        [MaxLength(40)]
        public string NomeUsuario { get; set; }
        [MaxLength(50)]
        public string Email { get; set; }
        [MaxLength(30)]
        public string Senha { get; set; }
        
        private string contato;
        public string Contato
        {
            get { return contato; }
            set { contato = NomeUsuario + " - " + Email; }
        }

        [ManyToMany(typeof(UsuarioProjeto))]
        public List<Projeto> Projetos { get; set; }

        public override string ToString()
        {
            return string.Format("Nome: {0}, E-mail: {1}",NomeUsuario, Email);
        }
    }
}
