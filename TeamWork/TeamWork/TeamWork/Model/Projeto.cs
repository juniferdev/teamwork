using SQLite.Net.Attributes;
using SQLiteNetExtensions.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace TeamWork.Model
{
    public class Projeto
    {
        #region + Atributos e Propriedades +

        [PrimaryKey,AutoIncrement]
        public int Id { get; set; }

        [MaxLength(40)]
        public string NomeProjeto { get; set; }

        [MaxLength(200)]
        public string ObjetivoProjeto { get; set; }

        [MaxLength(350)]
        public string DescricaoProjeto { get; set; }

        [ForeignKey(typeof(Usuario))]
        public int IdGerente { get; set; }

        [MaxLength(40)]
        public string Contratada { get; set; }

        [MaxLength(40)]
        public string Contratante { get; set; }

        private DateTime dataPrevInicio;
        [MaxLength(50)]
        public DateTime DataPrevInicio { get; set; }

        private DateTime dataPrevTermino;
        [MaxLength(50)]
        public DateTime DataPrevTermino { get; set; }

        [ManyToOne]
        public Usuario GerenteProjeto { get; set; }

        [ManyToMany(typeof(UsuarioProjeto))]
        public List<Usuario> Usuarios { get; set; }

        #endregion - Atributos e Propriedades -

    }
}
