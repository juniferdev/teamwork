using SQLite.Net.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TeamWork.Model
{
    public class Momento
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        [MaxLength(60)]
        public string NomeMomento { get; set; }
    }
}
