using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TeamWork.Internal;
using TeamWork.Model;
using TeamWork.Repository;
using TeamWork.Service;
using TeamWork.ViewModel.Conta;
using Xamarin.Forms;

namespace TeamWork.ViewModel.Tarefa
{
    public class CriarTarefaViewModel : BaseViewModel
    {
        #region ' Propriedades '
        public string NomeTarefa { get; set; }
        public TipoTarefa TipoTarefaView { get; set; }
        public string Descricao { get; set; }
        //public EstadoEnum Estado { get; set; }
        //public MotivoEnum Motivo { get; set; }
        public DateTime DataPrevInicio { get; set; }
        public DateTime DataPrevTermino { get; set; }
        public DateTime dataMaiorQueInicio;
        public DateTime DataMaiorQueInicio { get { return dataMaiorQueInicio; } set { dataMaiorQueInicio = value; } }
        //public List<Tarefa> Predecessoras { get; set; }
        #endregion


        public DateTime resultData()
        {
            if (DataPrevInicio > DataPrevTermino)
            {
                return DataMaiorQueInicio = DataPrevInicio;
            }
            return DataMaiorQueInicio = DataPrevTermino;
        }

        private string criadorView;
        public string CriadorView
        {
            get { return criadorView; }
            set
            {
                criadorView = value;
                OnPropertyChanged(nameof(CriadorView));
            }
        }

        private int IdResponsavel = 0;

        public int IdProjeto;
        public bool projetoDefinido = false;

        private string responsavelView;
        public string ResponsavelView
        {
            get { return responsavelView; }
            set { responsavelView = value; OnPropertyChanged(nameof(ResponsavelView)); }
        }

        public bool habilitarBotaoAtribuirTarefa = false;
        public bool HabilitarBotaoAtribuirTarefa
        {
            get { return habilitarBotaoAtribuirTarefa; }
            set { habilitarBotaoAtribuirTarefa = value; OnPropertyChanged(nameof(HabilitarBotaoAtribuirTarefa)); }
        }
                
            
        #region ' Command '
        public Command SalvarTarefaCommand { get; set; }
        public Command AtribuirResponsavelCommand { get; set; }

        #endregion

        public TarefaService servicoTarefa;
        public ContaService servicoConta;
        public ContaService servicoGrupo;
        public GrupoService servicoGrupo2;
        public ProjetoService servicoProjeto;

        public ObservableCollection<Usuario> UsuariosTarefa { get; set; }

        public string EmailView { get; set; }
        public ObservableCollection<Usuario> contatos;
        public ObservableCollection<Usuario> Contatos { get { return contatos; } set { contatos = value; OnPropertyChanged(nameof(Contatos)); } }

        public ObservableCollection<Model.Projeto> projetos;
        public ObservableCollection<Model.Projeto> Projetos { get { return projetos; } set { projetos = value; OnPropertyChanged(nameof(Projetos)); } }

        public string projetoSelecionado { get; set; }
        public string ProjetoSelecionado { get { return projetoSelecionado; } set { projetoSelecionado = value; OnPropertyChanged(nameof(ProjetoSelecionado)); } }

        public CriarTarefaViewModel()
        {
            SalvarTarefaCommand = new Command(CriarTarefa);
            AtribuirResponsavelCommand = new Command(AtribuirAoResponsavel);
            DadosAutomaticos();
            servicoConta = new ContaService();
            servicoGrupo2 = new GrupoService();
            servicoProjeto = new ProjetoService();
            Contatos = new ObservableCollection<Usuario>(servicoGrupo2.ObterMembrosDoGrupoContatos());
            Projetos = new ObservableCollection<Model.Projeto>(servicoProjeto.ObterProjetosDoUsuarioLogado(false));
            servicoTarefa = new TarefaService();
        }

        public void CriarTarefa()
        {
                if (DataPrevInicio > DataPrevTermino)
                {
                    DataMaiorQueInicio = DataPrevInicio;
                Toast.LongMessage(Mensagem.MENS_FORM_47);
                return;
                }

            servicoTarefa = (projetoDefinido) ?  new TarefaService(NomeTarefa, TipoTarefaView, Descricao, DataPrevInicio, DataPrevTermino, IdResponsavel, IdProjeto) :
                                                         new TarefaService(NomeTarefa, TipoTarefaView, Descricao, DataPrevInicio, DataPrevTermino, IdResponsavel);

            if (servicoTarefa.CriarNovaTarefa())
            {
                Application.Current.MainPage.Navigation.PopAsync();
            }
        }


        private void DadosAutomaticos()
        {
            //DateTime DiaMaisUm = DateTime.Now;
            //DataPrevTermino = DiaMaisUm.AddDays(1);
            DataPrevInicio = DateTime.Now;
            DataPrevTermino = DateTime.Now;
            servicoConta = new ContaService();
            TipoTarefaView = TipoTarefa.Indefinido;
            CriadorView = servicoConta.ObterUsuarioPorIdLogado().NomeUsuario;
        }

        public void AtribuirAoResponsavel()
        {
            servicoTarefa.SalvarResponsavelPorId();
            IdResponsavel = servicoTarefa.ObterIdDoResponsavelSelecionado();
            ResponsavelView = servicoTarefa.ResponsavelEscolhido().NomeUsuario;
            HabilitarBotaoAtribuirTarefa = false;
        }
        
    }
}
