using SQLite.Net.Attributes;
using SQLiteNetExtensions.Attributes;
using System;
using System.Collections.Generic;
using System.Text;
using TeamWork.Model;

namespace TeamWork.Model
{
    public class Grupo
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        [ForeignKey(typeof(Usuario))]
        public int IdCriador { get; set; }

        [ForeignKey(typeof(Grupo))]
        public int IdGrupoPai { get; set; }

        public bool Contatos { get; set; }

        [MaxLength(60)]
        public string NomeGrupo { get; set; }

        [MaxLength(200)]
        public string ObjetivoGrupo { get; set; }

    }
}
