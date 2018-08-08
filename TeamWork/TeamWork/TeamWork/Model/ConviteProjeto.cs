using SQLite.Net.Attributes;
using SQLiteNetExtensions.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TeamWork.Model
{
    public class ConviteProjeto
    {
        [ForeignKey(typeof(Projeto))]
        public int IdProjeto { get; set; }

        [ForeignKey(typeof(Usuario))]
        public int IdRemetente { get; set; }

        [ForeignKey(typeof(Usuario))]
        public int IdDestinatario { get; set; }

        [MaxLength(40)]
        public string NomeProjeto { get; set; }

        [MaxLength(40)]
        public string NomeRemetente { get; set; }

        [MaxLength(40)]
        public string NomeDestinatario { get; set; }

        [MaxLength(50)]
        public string Email { get; set; }

        [MaxLength(100)]
        public string Convite { get; set; }

    }
}
