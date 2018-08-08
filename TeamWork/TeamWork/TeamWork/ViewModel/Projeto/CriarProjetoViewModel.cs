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

namespace TeamWork.ViewModel.Projeto
{
    public class CriarProjetoViewModel: BaseViewModel
    {
        public string NomeView{ get; set; }
        public string DescricaoView{ get; set; }
        private string gerenteView;
        public string GerenteView  { get { return gerenteView; } set { gerenteView = value; OnPropertyChanged(nameof(GerenteView)); } }
        public string ContratanteView { get; set; }
        public string ContratadaView { get; set; }
        public DateTime DataPrevInicioView { get; set; }
        public DateTime DataPrevTerminoView { get; set; }
        public bool habilitarBotaoConvidar = false;
        public bool HabilitarBotaoConvidar { get { return habilitarBotaoConvidar; } set { habilitarBotaoConvidar = value; OnPropertyChanged(nameof(HabilitarBotaoConvidar)); } }
        private Model.Projeto ultimoProjeto;

        public DateTime dataMaiorQueInicio;
        public DateTime DataMaiorQueInicio { get { return dataMaiorQueInicio; } set { dataMaiorQueInicio = value; } }
        
        public DateTime resultData()
        {
            if (DataPrevInicioView > DataPrevTerminoView)
            {
                return DataMaiorQueInicio = DataPrevInicioView;
            }
            return DataMaiorQueInicio = DataPrevTerminoView;
        }

        public ObservableCollection<ConviteProjeto> Convidados { get; set; }
        public ObservableCollection<Usuario> Contatos { get; set; }

        public Command CriarProjetoCommand { get; set; }
        public Command ConvidarContatoProjetoCommand { get; set; }

        private ProjetoService servicoProjeto;
        private ContaService servicoConta;
        private GrupoService servicoGrupo;

        public CriarProjetoViewModel()
        {
            CriarProjetoCommand = new Command(CriarProjeto);
            ConvidarContatoProjetoCommand = new Command(ConvidarContatoProjeto);
            DadosAutomaticos();
        }

        private void DadosAutomaticos()
        {
            servicoConta = new ContaService();
            servicoProjeto = new ProjetoService();
            servicoGrupo = new GrupoService();
            DataPrevInicioView = DateTime.Now;
            DataPrevTerminoView = DateTime.Now;
            GerenteView = servicoConta.ObterUsuarioPorIdLogado().NomeUsuario;
            Contatos = new ObservableCollection<Usuario>(servicoGrupo.ObterMembrosDoGrupoContatos());
            Convidados = new ObservableCollection<ConviteProjeto>();
        }

        private void ConvidarContatoProjeto()
        {
            if (servicoProjeto.ContatoNaoFoiConvidado(Convidados))
            {
                Convidados.Add(servicoProjeto.CriarConviteProjeto());
            }
        }

        private void CriarProjeto()
        {
            if (DataPrevInicioView > DataPrevTerminoView)
            {
                DataMaiorQueInicio = DataPrevInicioView;
                Toast.LongMessage(Mensagem.MENS_FORM_47);
                return;
            }

            servicoProjeto = new ProjetoService(NomeView, DescricaoView, ContratanteView, ContratadaView, DataPrevInicioView, DataPrevTerminoView);
            if (servicoProjeto.CriarNovoProjeto())
            {
                if (Convidados.Count > 0)
                {
                    servicoProjeto.EnviarConvitesProjeto(Convidados.ToList(), servicoProjeto.ObterUltimoProjetoInserido());
                }
                Application.Current.MainPage.Navigation.PopAsync();
            }
            
        }

    }
}
