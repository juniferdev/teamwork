using SQLite.Net;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TeamWork.Interface;
using TeamWork.Model;
using Xamarin.Forms;

namespace TeamWork.Repository
{
    public class TarefaProjetoRepository
    {
        private SQLiteConnection conexao;

        public TarefaProjetoRepository()
        {
            var config = DependencyService.Get<ISQLiteConfig>();
            conexao = new SQLiteConnection(config.Plataforma, Path.Combine(config.DiretorioSQLite, "TeamWork.db3"));
            conexao.CreateTable<Projeto>();
            conexao.CreateTable<Tarefa>();
        }

        public List<Tarefa> ConsultarTarefasDoProjeto(int idProjeto)
        {
            return conexao.Query<Tarefa>("SELECT * FROM Tarefa WHERE IdProjeto = ?", idProjeto);
        }
    }
}
