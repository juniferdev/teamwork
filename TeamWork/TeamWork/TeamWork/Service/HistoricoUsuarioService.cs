using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TeamWork.Model;

namespace TeamWork.Service
{
    public class HistoricoUsuarioService
    {
        // Verificar se algum usuário está autenticado. 
        // Se estiver, usar os dados do usuário como filtro de busca no banco de dados.
        // Se não estiver, não é possível listar o histórico pois não há usuário autenticado.

        private HistoricoUsuario Historico;

        public HistoricoUsuarioService(HistoricoUsuario historico)
        {
            Historico = historico;
            ConsultarHistoricoUsuario();
        }

        public void ConsultarHistoricoUsuario()
        {
            
        }
    }
}
