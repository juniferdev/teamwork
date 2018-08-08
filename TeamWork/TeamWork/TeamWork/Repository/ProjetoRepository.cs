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
    public class ProjetoRepository
    {
        private SQLiteConnection conexao;
        private UsuarioProjetoRepository dadosUsuarioProjeto;

        public ProjetoRepository()
        {
            var config = DependencyService.Get<ISQLiteConfig>();
            conexao = new SQLiteConnection(config.Plataforma, Path.Combine(config.DiretorioSQLite, "TeamWork.db3"));
            conexao.CreateTable<Projeto>();
        }

        public void IncluirProjeto(Projeto projeto)
        {
            conexao.Query<Projeto>("INSERT INTO Projeto" +
                    "(NomeProjeto," +
                    "IdGerente," +
                    "ObjetivoProjeto," + 
                    "DescricaoProjeto," +
                    "Contratante," +
                    "Contratada," +
                    "DataPrevInicio," +
                    "DataPrevTermino)" +
                    " VALUES (?,?,?,?,?,?,?,?)",
                    projeto.NomeProjeto,
                    projeto.IdGerente,
                    projeto.ObjetivoProjeto,
                    projeto.DescricaoProjeto,
                    projeto.Contratante,
                    projeto.Contratada,
                    projeto.DataPrevInicio,
                    projeto.DataPrevTermino);
        }

        public int UltimoIdInserido()
        {
            return conexao.Table<Projeto>().LastOrDefault().Id;
        }

        public List<Projeto> ConsultarProjetos(int idUsuario)
        {
            return conexao.Query<Projeto>("SELECT * FROM Projeto");
        }

        public Projeto ConsultarProjeto(int idProjeto)
        {
            return conexao.FindWithQuery<Projeto>("SELECT * FROM Projeto WHERE Id = ?", idProjeto);
        }

        // Remover depois
        public List<Projeto> ConsultarTodosOsProjetos()
        {
            return conexao.Query<Projeto>("SELECT * FROM Projeto");
        }

        public void DeletarProjeto(int idProjeto)
        {
            conexao.Query<Projeto>("DELETE FROM Projeto Where Id = ?", idProjeto);
        }

        public void AlterarProjeto(Projeto projeto)
        {
            conexao.Execute("UPDATE Projeto SET NomeProjeto = ?, ObjetivoProjeto = ?, DescricaoProjeto = ?, Contratada = ?," +
                            "Contratante = ?, DataPrevInicio = ?, DataPrevTermino = ? Where Id = ?",
                            projeto.NomeProjeto,
                            projeto.ObjetivoProjeto,
                            projeto.DescricaoProjeto,
                            projeto.Contratada,
                            projeto.Contratante,
                            projeto.DataPrevInicio,
                            projeto.DataPrevTermino,
                            projeto.Id);
        }

        public void LimparTabela()
        {
            conexao.DropTable<Projeto>();
            conexao.CreateTable<Projeto>();
        }

        #region + Metodos Obsoletos +
        // Move-los para cá
        #endregion - Metodos Obsoletos -
    }
}
