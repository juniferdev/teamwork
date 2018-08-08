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
    public class GrupoRepository
    {
        private SQLiteConnection conexao;

        public GrupoRepository()
        {
            var config = DependencyService.Get<ISQLiteConfig>();
            conexao = new SQLiteConnection(config.Plataforma, Path.Combine(config.DiretorioSQLite, "TeamWork.db3"));
            conexao.CreateTable<Usuario>();
            conexao.CreateTable<Grupo>();
            conexao.CreateTable<ConviteGrupo>();
        }

        public void IncluirGrupoContatos(Grupo grupo)
        {
            conexao.Query<Grupo>("INSERT INTO Grupo" +
                    "(IdCriador," +
                    "Contatos)" +
                    " VALUES (?,?)",
                    grupo.IdCriador,
                    grupo.Contatos);
        }

        public void IncluirGrupo(Grupo grupo)
        {
            conexao.Query<Grupo>("INSERT INTO Grupo" +
                    "(IdCriador," +
                    "IdGrupoPai," +
                    "NomeGrupo," +
                    "ObjetivoGrupo," +
                    "Contatos)" +
                    " VALUES (?,?,?,?,?)",
                    grupo.IdCriador,
                    grupo.IdGrupoPai,
                    grupo.NomeGrupo,
                    grupo.ObjetivoGrupo,
                    grupo.Contatos);
        }

        public int UltimoIdInserido()
        {
            return conexao.Table<Grupo>().LastOrDefault().Id;
        }

        public Grupo ConsultarGrupo(int idGrupo)
        {
            return conexao.FindWithQuery<Grupo>("SELECT * FROM Grupo WHERE Id = ?", idGrupo);
        }

        // Remover este método depois:
        public List<Grupo> ConsultarGrupos()
        {
            return conexao.Table<Grupo>().ToList();
        }

        public Grupo ConsultarGrupoContatos(int idUsuario)
        {
            return conexao.Query<Grupo>("SELECT * FROM Grupo").FirstOrDefault(campo => campo.Contatos == true && campo.IdCriador == idUsuario);
        }

        public void AlterarGrupo(Grupo grupo)
        {
            conexao.Execute("UPDATE Grupo SET NomeGrupo = ?, ObjetivoGrupo = ?",
                            grupo.NomeGrupo,
                            grupo.ObjetivoGrupo);
        }

        public void DeletarGrupo(int idGrupo)
        {
            conexao.Query<Grupo>("DELETE FROM Grupo Where Id = ?", idGrupo);
        }

        // Remover este método depois:
        public void LimparTabela()
        {
            conexao.DropTable<Grupo>();
            conexao.CreateTable<Grupo>();
        }
    }
}
