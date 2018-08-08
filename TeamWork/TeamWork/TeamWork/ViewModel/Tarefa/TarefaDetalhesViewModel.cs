using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TeamWork.Internal;
using TeamWork.Model;
using TeamWork.Service;
using Xamarin.Forms;

namespace TeamWork.ViewModel.Tarefa
{
    public class TarefaDetalhesViewModel : BaseViewModel
    {
        #region Propriedades

        public string nomeTarefaView { get; set; }
        public string NomeTarefaView { get { return nomeTarefaView; } set { nomeTarefaView = value; OnPropertyChanged(nameof(NomeTarefaView)); } }

        public TipoTarefa tipoTarefaView { get; set; }
        public TipoTarefa TipoTarefaView { get { return tipoTarefaView; } set { tipoTarefaView = value; OnPropertyChanged(nameof(TipoTarefaView)); } }

        public Estado estadoView { get; set; }
        public Estado EstadoView { get { return estadoView; } set { estadoView = value; OnPropertyChanged(nameof(EstadoView)); } }

        public Razao razaoView { get; set; }
        public Razao RazaoView { get { return razaoView; } set { razaoView = value; OnPropertyChanged(nameof(RazaoView)); } }

        public string descricaoView;
        public string DescricaoView { get { return descricaoView; } set { descricaoView = value; OnPropertyChanged(nameof(DescricaoView)); } }

        public DateTime dataPrevInicioView;
        public DateTime DataPrevInicioView { get { return dataPrevInicioView; } set { dataPrevInicioView = value; OnPropertyChanged(nameof(DataPrevInicioView)); } }

        public DateTime dataPrevTerminoView;
        public DateTime DataPrevTerminoView { get { return dataPrevTerminoView; } set { dataPrevTerminoView = value; OnPropertyChanged(nameof(DataPrevTerminoView)); } }

        public string criadorView;
        public string CriadorView { get { return criadorView; } set { criadorView = value; OnPropertyChanged(nameof(CriadorView)); } }

        public string responsavelView;
        public string ResponsavelView { get { return responsavelView; } set { responsavelView = value; OnPropertyChanged(nameof(ResponsavelView)); } }
 
        public string projetoSelecionado { get; set; }
        public string ProjetoSelecionado { get { return projetoSelecionado; } set { projetoSelecionado = value; OnPropertyChanged(nameof(ProjetoSelecionado)); } }

        public DateTime dataMaiorQueInicio;
        public DateTime DataMaiorQueInicio { get { return dataMaiorQueInicio; } set { dataMaiorQueInicio = value; } }

        private Model.Tarefa tarefa;

        public int IdCriador;

        public int IdResponsavelView;
        public int IdProjetoView;
        public Estado EstadoAnterior;

        public bool camposHabilitados = false;
        public bool CamposHabilitados
        {
            get { return camposHabilitados; }
            set
            {
                camposHabilitados = value;
                OnPropertyChanged(nameof(CamposHabilitados));
            }
        }

        public bool habilitarEstado = false;
        public bool HabilitarEstado
        {
            get { return habilitarEstado; }
            set
            {
                habilitarEstado = value;
                OnPropertyChanged(nameof(HabilitarEstado));
            }
        }

        public bool habilitarBotaoAtribuirTarefa = false;
        public bool HabilitarBotaoAtribuirTarefa
        {
            get { return habilitarBotaoAtribuirTarefa; }
            set { habilitarBotaoAtribuirTarefa = value; OnPropertyChanged(nameof(HabilitarBotaoAtribuirTarefa)); }
        }

        #endregion Propriedades

        public ObservableCollection<Usuario> contatos { get; set; }
        public ObservableCollection<Usuario> Contatos { get { return contatos; } set { contatos = value; OnPropertyChanged(nameof(Contatos)); } }

        public ObservableCollection<Model.Projeto> projetos { get; set; }
        public ObservableCollection<Model.Projeto> Projetos { get { return projetos; } set { projetos = value; OnPropertyChanged(nameof(Projetos)); } }

        public ObservableCollection<EstadoString> estados { get; set; }
        public ObservableCollection<EstadoString> Estados { get { return estados; } set { estados = value; OnPropertyChanged(nameof(Estados)); } }

        public Command EditarTarefaCommand { get; set; }
        public Command SalvarTarefaCommand { get; set; }
        public Command AtribuirResponsavelCommand { get; set; }

        public TarefaService servicoTarefa;
        public GrupoService servicoGrupo;
        public ProjetoService servicoProjeto;

        public TarefaDetalhesViewModel()
        {
            EditarTarefaCommand = new Command(HabilitarCampos);
            SalvarTarefaCommand = new Command(SalvarTarefa);
            AtribuirResponsavelCommand = new Command(AtribuirAoResponsavel);
            servicoGrupo = new GrupoService();
            servicoProjeto = new ProjetoService();
            Contatos = new ObservableCollection<Usuario>(servicoGrupo.ObterMembrosDoGrupoContatos());
            Projetos = new ObservableCollection<Model.Projeto>(servicoProjeto.ObterProjetosDoUsuarioLogado(false));
            DadosAutomaticos();
        }

        private void SalvarTarefa()
        {
            if (DataPrevInicioView > DataPrevTerminoView)
            {
                DataMaiorQueInicio = DataPrevInicioView;
                Toast.LongMessage(Mensagem.MENS_FORM_47);
                return;
            }
            Model.Tarefa modelTarefa = new Model.Tarefa()
            {
                Id = servicoTarefa.ObterIdTarefaSelecionada(),
                NomeTarefa = NomeTarefaView,
                TipoTarefa = TipoTarefaView,
                IdResponsavel = servicoTarefa.ObterIdDoResponsavelSelecionado(),
                DescricaoTarefa = DescricaoView,
                DataPrevInicio = DataPrevInicioView,
                DataPrevTermino = DataPrevTerminoView,
                IdProjeto = servicoTarefa.ObterIdDoProjetoSelecionado(),
                Estado = servicoTarefa.ObterEstadoSelecionado(),
                Motivo = RazaoView
            };

            if (modelTarefa.Estado == Estado.Iniciada)
            {
                modelTarefa.DataInicio = DateTime.Now;
            }

            if(modelTarefa.Estado == Estado.Feita)
            {
                modelTarefa.DataTermino = DateTime.Now;
            }

            servicoTarefa.AlterarTarefa(modelTarefa);
            Application.Current.MainPage.Navigation.PopAsync();
        }

        public DateTime resultData()
        {
            if (DataPrevInicioView > DataPrevTerminoView)
            {
                return DataMaiorQueInicio = DataPrevInicioView;
            }
            return DataMaiorQueInicio = DataPrevTerminoView;
        }

        public void HabilitarCampos()
        {
            IdCriador = (int) Application.Current.Properties["id"];
            if(IdCriador == tarefa.IdCriador)
            {
                HabilitarCamposCriador();
            }
            HabilitarCampoEstado();
        }

        public void HabilitarCamposCriador()
        {
            CamposHabilitados = true;
        }

        public void HabilitarCampoEstado()
        {
            HabilitarEstado = true;
        }

        private void DadosAutomaticos()
        {
            servicoTarefa = new TarefaService();
            servicoTarefa.SalvarIdTarefaSelecionada();
            tarefa = servicoTarefa.ObterTarefaSelecionada();

            NomeTarefaView = tarefa.NomeTarefa;
            TipoTarefaView = tarefa.TipoTarefa;
            DescricaoView = tarefa.DescricaoTarefa;
            DataPrevInicioView = tarefa.DataPrevInicio;
            DataPrevTerminoView = tarefa.DataPrevTermino;
            CriadorView = servicoTarefa.ObterNomeCriadorTarefa();
            ResponsavelView = servicoTarefa.ObterNomeResponsavelTarefa();
            ProjetoSelecionado = servicoTarefa.ObterNomeProjetoDaTarefa();
            EstadoView = tarefa.Estado;
            EstadoAnterior = tarefa.Estado;
            Estados = servicoTarefa.ListarEstadosDisponiveis(tarefa.Estado);
            RazaoView = tarefa.Motivo;
        }

        public void AtribuirAoResponsavel()
        {
            servicoTarefa.SalvarResponsavelPorId();
            IdResponsavelView = servicoTarefa.ObterIdDoResponsavelSelecionado();
            ResponsavelView = servicoTarefa.ResponsavelEscolhido().NomeUsuario;
            HabilitarBotaoAtribuirTarefa = false;
        }
    }
}
