using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SQLite.Net;
using TeamWork.Internal;
using TeamWork.Model;
using TeamWork.Repository;
using Xamarin.Forms;
using System.Collections.ObjectModel;

namespace TeamWork.Service
{
    public class TarefaService
    {
        private string TarefaNomeServ;
        private string TarefaDescricaoServ;
        private TipoTarefa TipoTarefaServ;
        private List<Projeto> Projeto;
        private int IdCriadorServ;
        private int IdProjetoServ;
        private string TarefaCriadorServ; //nome do gerente que criou a tarefa
        private DateTime TarefaDataPrevInicioServ;
        private DateTime TarefaDataPrevTerminoServ;
        private List<Usuario> TareaResponsavelServ;
        private int IdTarefaSelecionada;
        private int IdResponsavelServ;
        private Estado EstadoServ;
        private int LimpaIdDoResponsavel;

        private Tarefa tarefa;
        private TarefaRepository dadosTarefa;
        private Usuario usuario;
        private UsuarioRepository dadosUsuario;
        private Projeto projeto;
        private ProjetoRepository dadosProjeto;
        private UsuarioProjeto usuarioProjeto;
        private UsuarioProjetoRepository dadosUsuarioProjeto;
        private ConviteRepository dadosUsuarioConvidado;
        private UsuarioTarefaRepository dadosUsuarioTarefa;


        public TarefaService() { }

        public TarefaService(string nome, TipoTarefa tipo, string descricao, DateTime dataInicio, DateTime dataTermino)
        {
            TarefaNomeServ = nome;
            TipoTarefaServ = tipo;
            TarefaDescricaoServ = descricao;
            TarefaDataPrevInicioServ = dataInicio;
            TarefaDataPrevTerminoServ = dataTermino;
        }
                                          
        public TarefaService(string nome, TipoTarefa tipo, string descricao, DateTime dataInicio, DateTime dataTermino, int IdResponsavel)
        {
            TarefaNomeServ = nome;
            TipoTarefaServ = tipo;
            TarefaDescricaoServ = descricao;
            TarefaDataPrevInicioServ = dataInicio;
            TarefaDataPrevTerminoServ = dataTermino;
            IdResponsavelServ = IdResponsavel;
        }

        public TarefaService(string nome, TipoTarefa tipo, string descricao, DateTime dataInicio, DateTime dataTermino, int IdResponsavel, int IdProjeto)
        {
            TarefaNomeServ = nome;
            TipoTarefaServ = tipo;
            TarefaDescricaoServ = descricao;
            TarefaDataPrevInicioServ = dataInicio;
            TarefaDataPrevTerminoServ = dataTermino;
            IdProjetoServ = IdProjeto;
        }

        public List<Usuario> ObterUsuarioTarefa()
        {
            return dadosUsuario.ConsultarUsuarios().ToList();
        }

        public string ObterNomeProjetoDaTarefa()
        {
           // Resumo: Retorna o projeto da tarefa selecionada.

            if(dadosTarefa == null)
            {
                dadosTarefa = new TarefaRepository();
            }

            IdProjetoServ = dadosTarefa.ConsultarTarefa(IdTarefaSelecionada).IdProjeto;

            if (IdProjetoServ == 0)
            {
                return "N/A";
            }

            if (dadosProjeto == null)
            {
                dadosProjeto = new ProjetoRepository();
            }

            return dadosProjeto.ConsultarProjeto(IdProjetoServ).NomeProjeto;
        }

        public List<Tarefa> ObterTarefasDoUsuarioLogado(bool avisarListaVazia = true)
        {
            int IdUsuario = (int) Application.Current.Properties["id"];

            if (dadosUsuarioTarefa == null)
            {
                dadosUsuarioTarefa = new UsuarioTarefaRepository();
            }

            var dados = dadosUsuarioTarefa.ConsultarTarefasDoUsuario(IdUsuario);

            if (avisarListaVazia)
            {
                if (dados.Count == 0)
                {
                    // Mensagem: Você ainda não criou ou é responsável por nenhuma tarefa. 
                    Toast.ShortMessage(Mensagem.MENS_FORM_46);
                }
            }

            return dados;
        }

        public void SalvarIdProjetoSelecionado()
        {
            // Resumo: Identifica para a aplicação que agora vamos interagir com um projeto específico.

            IdProjetoServ = (int)Application.Current.Properties["idProjeto"];
        }

        public bool ValidarFormCriarTarefa()
        {
            if (CamposPreenchidos())
            {
                return true;
            }
            return false;
        }

        private bool CamposPreenchidos()
        {
            if (!string.IsNullOrWhiteSpace(TarefaNomeServ) && (!TipoTarefaServ.HasFlag(TipoTarefa.Indefinido)))
            {
                return true;
            }
            // Mensagem: Favor preencher todos os campos com *.
            Toast.ShortMessage(Mensagem.MENS_FORM_08);
            return false;
        }

        public bool CriarNovaTarefa()
        {
            if (ValidarFormCriarTarefa())
            {
                IdCriadorServ = (int)Application.Current.Properties["id"];

                tarefa = new Tarefa()
                {
                    NomeTarefa = TarefaNomeServ,
                    TipoTarefa = TipoTarefaServ,
                    DescricaoTarefa = TarefaDescricaoServ,
                    DataPrevInicio = TarefaDataPrevInicioServ,
                    DataPrevTermino = TarefaDataPrevTerminoServ,
                    IdCriador = IdCriadorServ,
                    IdProjeto = IdProjetoServ,
                    Estado = Estado.Aberta,
                    Motivo = Razao.Criada,
                    IdResponsavel = IdResponsavelServ != 0 ? IdResponsavelServ : IdCriadorServ
                };

                if (dadosTarefa == null)
                {
                    dadosTarefa = new TarefaRepository();
                }
                try
                {
                    dadosTarefa.IncluirTarefa(tarefa);
                    // Mensagem: Tarefa criada com sucesso.
                    Toast.ShortMessage(Mensagem.MENS_FORM_10);
                    return true;
                }
                catch (SQLiteException ex)
                {
                    // Mensagem: Erro ao incluir uma tarefa no banco de dados.
                    Toast.ShortMessage(Mensagem.MENS_FORM_20);
                }
            }
            return false;
        }

        public void SalvarIdTarefaSelecionada()
        {
            IdTarefaSelecionada = (int)Application.Current.Properties["idTarefa"];
        }

        public int ObterIdTarefaSelecionada()
        {
            return IdTarefaSelecionada;
        }

        public void SalvarEstadoSelecionado()
        {
            EstadoServ = (Estado) Application.Current.Properties["idEstado"];
        }

        public Estado ObterEstadoSelecionado()
        {
            return EstadoServ;
        }

        public Tarefa ObterTarefaSelecionada()
        {
           // Resumo: Permite consultar os dados da tarefa selecionada.

            if (dadosTarefa == null)
            {
                dadosTarefa = new TarefaRepository();
            }

            tarefa = dadosTarefa.ConsultarTarefa(IdTarefaSelecionada);

            IdCriadorServ = tarefa.IdCriador;

            return tarefa;
        }

        public void ExcluirTarefaSelecionada()
        {
            if(dadosTarefa == null)
            {
                dadosTarefa = new TarefaRepository();
            }
            try
            {
                dadosTarefa.DeletarTarefaSelecionada(IdTarefaSelecionada);
            }
            catch(SQLiteException ex)
            {
                Toast.ShortMessage("DEU RUIM");
            }
            
        }

        public void SalvarResponsavelPorId()
        {
            IdResponsavelServ = (int)Application.Current.Properties["idResponsavel"];
        }

        public void LimparIdResponsavel()
        {
            LimpaIdDoResponsavel = 0;
        }

        public int ObterIdDoResponsavelSelecionado()
        {
            return IdResponsavelServ;
        }

        public int ObterIdDoProjetoSelecionado()
        {
            return IdProjetoServ;
        }

        public Usuario respTemp;
        public Usuario ResponsavelEscolhido()
        {
            if (dadosUsuario == null)
            {
                dadosUsuario = new UsuarioRepository();
            }

            ObterIdDoResponsavelSelecionado();
            respTemp = dadosUsuario.ConsultarUsuarioPorId(IdResponsavelServ);
            return respTemp;

        }

        public void AlterarTarefa(Tarefa tarefa)
        {
            if (dadosTarefa == null)
            {
                dadosTarefa = new TarefaRepository();
            }
            try
            {
                dadosTarefa.AlterarTarefa(tarefa);

                // Mensagem: Tarefa alterada com sucesso.
                Toast.ShortMessage(Mensagem.MENS_FORM_35);
            }
            catch(SQLiteException ex)
            {
                //Tenho que colocar o toast
            }
        }

        public Usuario ObterCriadorTarefa()
        {
            // Resumo: Retorna o nome do criador da tarefa selecionada.

            if (dadosTarefa == null)
            {
                dadosTarefa = new TarefaRepository();
            }

            IdCriadorServ = dadosTarefa.ConsultarTarefa(IdTarefaSelecionada).IdCriador;
            dadosUsuario = new UsuarioRepository();

            return dadosUsuario.ConsultarUsuarioPorId(IdCriadorServ);
        }

        public string ObterNomeCriadorTarefa()
        {
            // Resumo: Retorna o nome do criador da tarefa selecionada.

            if (dadosTarefa == null)
            {
                dadosTarefa = new TarefaRepository();
            }

            IdCriadorServ = dadosTarefa.ConsultarTarefa(IdTarefaSelecionada).IdCriador;
            dadosUsuario = new UsuarioRepository();

            return dadosUsuario.ConsultarUsuarioPorId(IdCriadorServ).NomeUsuario;
        }

        public string ObterNomeResponsavelTarefa()
        {
            // Resumo: Retorna o nome do criador da tarefa selecionada.

            if (dadosTarefa == null)
            {
                dadosTarefa = new TarefaRepository();
            }

            IdResponsavelServ = dadosTarefa.ConsultarTarefa(IdTarefaSelecionada).IdResponsavel;

            if(IdResponsavelServ == 0)
            {
                return "N/A";
            }

            dadosUsuario = new UsuarioRepository();

            return dadosUsuario.ConsultarUsuarioPorId(IdResponsavelServ).NomeUsuario;
        }

        public ObservableCollection<EstadoString> ListarEstadosDisponiveis(Estado estado)
        {

            if (estado == Estado.Aberta)
            {
                return new ObservableCollection<EstadoString>
                {
                    new EstadoString
                    {
                        Estado = Estado.Iniciada,
                        EstadoStr = "Iniciada"
                    }
                };
            }
            else if (estado == Estado.Iniciada)
            {
                return new ObservableCollection<EstadoString>
                {
                    new EstadoString{Estado = Estado.Aberta, EstadoStr = "Aberta"},
                    new EstadoString{Estado = Estado.Feita, EstadoStr = "Feita"},
                    new EstadoString{Estado = Estado.Encerrada, EstadoStr = "Encerrada"},
                };
            }
            else if (estado == Estado.Feita)
            {
                return new ObservableCollection<EstadoString>
                {
                    new EstadoString{Estado = Estado.Encerrada, EstadoStr = "Encerrada"},
                    new EstadoString{Estado = Estado.Aberta, EstadoStr = "Aberta"},
                };
            }
            else
            {
                return new ObservableCollection<EstadoString>
                {
                    new EstadoString{Estado = Estado.Encerrada, EstadoStr = "Encerrada"}
                };
            }
        }

        public Razao ObterMotivoTarefa(Estado estado, Estado estadoAnterior)
        {
            if (estado == Estado.Iniciada && estadoAnterior == Estado.Aberta)
            {
                return Razao.Aceita;
            }
            else if (estado == Estado.Aberta && estadoAnterior == Estado.Iniciada)
            {
                return Razao.Pendente;
            }
            else if (estado == Estado.Aberta && estadoAnterior == Estado.Feita)
            {
                return Razao.NaoConcluida;
            }
            else if (estado == Estado.Feita && estadoAnterior == Estado.Iniciada)
            {
                return Razao.Feita;
            }
            else if (estado == Estado.Encerrada && estadoAnterior == Estado.Feita)
            {
                return Razao.Verificada;
            }
            else
            {
                return Razao.Cancelada;
            }
        }

        public object ObterDataReal()
        {
            if (EstadoServ == Estado.Iniciada || EstadoServ == Estado.Feita)
            {
                return DateTime.Now;
            }
            return null;
        }
    }
}