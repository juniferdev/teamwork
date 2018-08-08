using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using TeamWork.Internal;
using TeamWork.Model;
using TeamWork.Repository;
using TeamWork.Service;
using TeamWork.View.Projeto;
using Xamarin.Forms;

namespace TeamWork.ViewModel.Projeto
{
    public class ProjetoDetalhesViewModel : BaseViewModel
    {
        private string nomeView;
        public string NomeView { get { return nomeView; } set { nomeView = value; OnPropertyChanged(nameof(NomeView)); } }
        private string objetivoView;
        public string ObjetivoView { get { return objetivoView; } set { objetivoView = value; OnPropertyChanged(nameof(ObjetivoView)); } }
        private string descricaoView;
        public string DescricaoView { get { return descricaoView; } set { descricaoView = value; OnPropertyChanged(nameof(DescricaoView)); } }
        private string contratanteView;
        public string ContratanteView { get { return contratanteView; } set { contratanteView = value; OnPropertyChanged(nameof(ContratanteView)); } }
        private string contratadaView;
        public string ContratadaView { get { return contratadaView; } set { contratadaView = value; OnPropertyChanged(nameof(ContratadaView)); } }
        public string GerenteView { get; set; }
        private DateTime dataPrevInicio;
        public DateTime DataPrevInicio { get { return dataPrevInicio; } set { dataPrevInicio = value; OnPropertyChanged(nameof(DataPrevInicio)); } }
        private DateTime dataPrevTermino;
        public DateTime DataPrevTermino { get { return dataPrevTermino; } set { dataPrevTermino = value; OnPropertyChanged(nameof(DataPrevTermino)); } }

        public bool habilitarBotaoConvidar = false;
        public bool HabilitarBotaoConvidar { get { return habilitarBotaoConvidar; } set { habilitarBotaoConvidar = value; OnPropertyChanged(nameof(HabilitarBotaoConvidar)); } }

        public bool habilitarBotaoRemover = false;
        public bool HabilitarBotaoRemover { get { return habilitarBotaoRemover; } set { habilitarBotaoRemover = value; OnPropertyChanged(nameof(HabilitarBotaoRemover)); } }

        public bool camposHabilitados = false;
        public bool CamposHabilitados
        {
            get { return camposHabilitados; }
            set { camposHabilitados = value; OnPropertyChanged(nameof(CamposHabilitados)); }
        }

        public Command SalvarCommand { get; set; }
        public Command EditarPrjetoCommand { get; set; }
        public Command ConvidadosCommand { get; set; }
        public Command ConvidarContatoCommand { get; set; }
        public Command RemoverColaboradorCommand { get; set; }

        public Command ProjetoTarefasCommand { get; set; }

        public List<ConviteProjeto> novosConvites;
        public ObservableCollection<Usuario> Contatos { get; set; }
        public ObservableCollection<ConviteProjeto> Convidados { get; set; }
        public ObservableCollection<Usuario> usuariosProjeto { get; set; }
        public ObservableCollection<Usuario> UsuariosProjeto { get { return usuariosProjeto; } set { usuariosProjeto = value; OnPropertyChanged(nameof(UsuariosProjeto)); } }

        public DateTime dataMaiorQueInicio;
        public DateTime DataMaiorQueInicio { get { return dataMaiorQueInicio; } set { dataMaiorQueInicio = value; } }

        public DateTime resultData()
        {
            if (DataPrevInicio > DataPrevTermino)
            {
                return DataMaiorQueInicio = DataPrevInicio;
            }
            return DataMaiorQueInicio = DataPrevTermino;
        }

        public ProjetoService servicoProjeto;
        private GrupoService servicoGrupo;

        private Model.Projeto projeto;

        public ProjetoDetalhesViewModel()
        {
            SalvarCommand = new Command(SalvarAlteracoes);
            EditarPrjetoCommand = new Command(HabilitarCampos);
            ConvidadosCommand = new Command(ChamarConvidadosProjetoView);
            ConvidarContatoCommand = new Command(ConvidarContatoProjeto);
            RemoverColaboradorCommand = new Command(RemoverColaborador);
            ProjetoTarefasCommand = new Command(ChamarProjetoTarefasView);
            DadosAutomaticos();
        }

        private void DadosAutomaticos()
        {
            servicoProjeto = new ProjetoService();
            servicoGrupo = new GrupoService();
            servicoProjeto.SalvarIdProjetoSelecionado();
            projeto = servicoProjeto.ObterProjetoSelecionado();

            NomeView = projeto.NomeProjeto;
            ObjetivoView = projeto.ObjetivoProjeto;
            DescricaoView = projeto.DescricaoProjeto;
            ContratanteView = projeto.Contratante;
            ContratadaView = projeto.Contratada;
            DataPrevInicio = projeto.DataPrevInicio;
            DataPrevTermino = projeto.DataPrevTermino;

            GerenteView = servicoProjeto.ObterNomeGerenteProjeto();

            Contatos = new ObservableCollection<Usuario>(servicoGrupo.ObterMembrosDoGrupoContatos());
            Convidados = new ObservableCollection<ConviteProjeto>(servicoProjeto.ObterConvitesEnviadosDoProjeto());
            UsuariosProjeto = new ObservableCollection<Usuario>(servicoProjeto.ObterUsuariosDoProjeto());
            novosConvites = new List<ConviteProjeto>();
        }

        private void HabilitarCampos()
        {
            CamposHabilitados = true;
        }

        private void ConvidarContatoProjeto()
        {
            if (servicoProjeto.ContatoNaoFoiConvidado(Convidados))
            {
                if (!servicoProjeto.ContatoJaColaboraNoProjeto())
                {
                    novosConvites.Add(servicoProjeto.CriarConviteProjeto());
                    Convidados.Add(servicoProjeto.CriarConviteProjeto());
                    servicoProjeto.EnviarConvitesProjeto(novosConvites, servicoProjeto.ObterProjetoSelecionado());
                }
            }
        }

        private async void RemoverColaborador()
        {
            var resposta = await Application.Current.MainPage.DisplayAlert("Remoção de Usuário", "Remover este usuário do projeto?", "Sim", "Não");
            if (resposta)
            {
                servicoProjeto.RemoverColaboradorDoProjeto();
                UsuariosProjeto = new ObservableCollection<Usuario>(servicoProjeto.ObterUsuariosDoProjeto());
            }
        }

        private void SalvarAlteracoes()
        {
            if (DataPrevInicio > DataPrevTermino)
            {
                DataMaiorQueInicio = DataPrevInicio;
                Toast.LongMessage(Mensagem.MENS_FORM_47);
                return;
            }

            Model.Projeto modelProjeto = new Model.Projeto()
            {
                Id = servicoProjeto.ObterIdProjetoSelecionado(),
                NomeProjeto = NomeView,
                ObjetivoProjeto = ObjetivoView,
                DescricaoProjeto = DescricaoView,
                Contratante = ContratanteView,
                Contratada = ContratadaView,
                DataPrevInicio = DataPrevInicio,
                DataPrevTermino = DataPrevTermino
            };
            servicoProjeto.AlterarProjeto(modelProjeto);
            Application.Current.MainPage.Navigation.PopAsync();
        }

        private void ChamarConvidadosProjetoView()
        {
            Application.Current.MainPage.Navigation.PushModalAsync(new NavigationPage(new ProjetoConvidadosView()) { BarBackgroundColor = Color.FromHex("#3b5998") }, true);
        }

        private void ChamarProjetoTarefasView()
        {
            Application.Current.MainPage.Navigation.PushModalAsync(new NavigationPage(new ProjetoTarefasView()) { BarBackgroundColor = Color.FromHex("#3b5998") }, true);
        }
    }
}
