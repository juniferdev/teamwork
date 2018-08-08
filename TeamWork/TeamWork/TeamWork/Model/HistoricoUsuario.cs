using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TeamWork.Model
{
    public class HistoricoUsuario
    {
        public Dictionary<Projeto, DateTime> UltimosProjetos { get; set; }
        public Dictionary<Tarefa, DateTime> UltimasTarefas { get; set; }
    }
}
