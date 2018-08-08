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
    public class TarefaRepository
    {
        private SQLiteConnection conexao;

        public TarefaRepository()
        {
            var config = DependencyService.Get<ISQLiteConfig>();
            conexao = new SQLiteConnection(config.Plataforma, Path.Combine(config.DiretorioSQLite, "TeamWork.db3"));
            conexao.CreateTable<Tarefa>();
        }

        public void IncluirTarefa(Tarefa tarefa)
        {
            conexao.Query<Tarefa>("INSERT INTO Tarefa" +
                          "(NomeTarefa," +
                          "IdCriador," +
                          "IdResponsavel," +
                          "IdProjeto," +
                          "TipoTarefa," +
                          "DescricaoTarefa," +
                          "DataPrevInicio," +
                          "DataPrevTermino," +
                          "Estado," +
                          "Motivo)" +
                          " VALUES (?,?,?,?,?,?,?,?,?,?)",
                tarefa.NomeTarefa,
                tarefa.IdCriador,
                tarefa.IdResponsavel,
                tarefa.IdProjeto,
                tarefa.TipoTarefa,
                tarefa.DescricaoTarefa,
                tarefa.DataPrevInicio,
                tarefa.DataPrevTermino,
                tarefa.Estado,
                tarefa.Motivo);

            /*
                    NomeTarefa = TarefaNomeServ,
                    DescricaoTarefa = TarefaDescricaoServ,
                    DataPrevInicio = TarefaDataPrevInicioServ,
                    DataPrevTermino = TarefaDataPrevTerminoServ,
                    IdCriador = IdCriadorServ,
                    IdProjeto = IdProjetoServ,
                    Estado = EstadoEnum.Aberta,
                    Motivo = MotivoEnum.Criada,
                    IdResponsavel = (int) Application.Current.Properties["idResponsavel"]
             */
        }

        // Este método deverá ser alterado para consultar projetos do usuário, pedindo um parâmetro do email do usuário
        public List<Tarefa> ConsultarTarefas()
        {
            return conexao.Query<Tarefa>("SELECT * FROM Tarefa");
        }

        public Tarefa ConsultarTarefa(int idTarefa)
        {
            return conexao.FindWithQuery<Tarefa>("SELECT * FROM Tarefa WHERE Id = ?", idTarefa);
        }

        public void DeletarTarefaSelecionada(int IdTarefa)
        {
            conexao.Query<Tarefa>("DELETE FROM Tarefa WHERE Id = ?", IdTarefa);
        }

        public void AlterarTarefa(Tarefa tarefa)
        {
            conexao.Query<Tarefa>("UPDATE Tarefa SET " +
                "NomeTarefa = ?," +
                "TipoTarefa = ?," +
                "IdResponsavel = ?," +
                "DescricaoTarefa = ?," +
                "DataPrevInicio = ?," +
                "DataPrevTermino = ?," +
                "Estado = ?," +
                "Motivo = ?," +
                "IdProjeto = ?" +
                "WHERE Id = ?",

                tarefa.NomeTarefa,
                tarefa.TipoTarefa,
                tarefa.IdResponsavel,
                tarefa.DescricaoTarefa,
                tarefa.DataPrevInicio,
                tarefa.DataPrevTermino,
                tarefa.Estado,
                tarefa.Motivo,
                tarefa.IdProjeto,
                tarefa.Id
                );
        }

        public bool UsuarioResponsavelPelaTarefa(int idUsuario)
        {
            Usuario usuario = conexao.FindWithQuery<Usuario>("SELECT * FROM Tarefa WHERE IdResponsavel = ?", idUsuario);
            return usuario == null ? false : true;
        }

    }
}
