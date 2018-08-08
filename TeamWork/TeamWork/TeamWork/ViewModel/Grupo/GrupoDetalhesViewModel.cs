using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TeamWork.Model;
using TeamWork.Service;
using TeamWork.View.Grupo;
using Xamarin.Forms;

namespace TeamWork.ViewModel.Grupo
{
    public class GrupoDetalhesViewModel: BaseViewModel
    {
        private string nomeView;
        public string NomeView { get { return nomeView; } set { nomeView = value; OnPropertyChanged(nameof(NomeView)); } }
        private string objetivoView;
        public string ObjetivoView { get { return objetivoView; } set { objetivoView = value; OnPropertyChanged(nameof(ObjetivoView)); } }

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

        public List<ConviteGrupo> novosConvites;
        public ObservableCollection<Usuario> Contatos { get; set; }
        public ObservableCollection<ConviteGrupo> Convidados { get; set; }
        public ObservableCollection<Usuario> membros { get; set; }
        public ObservableCollection<Usuario> Membros { get { return membros; } set { membros = value; OnPropertyChanged(nameof(Membros)); } }

        public Command SalvarCommand { get; set; }
        public Command EditarGrupoCommand { get; set; }
        public Command ConvidadosCommand { get; set; }
        public Command ConvidarContatoCommand { get; set; }
        public Command RemoverMembroCommand { get; set; }

        public GrupoService servicoGrupo;

        public GrupoDetalhesViewModel()
        {
            SalvarCommand = new Command(SalvarAlteracoes);
            EditarGrupoCommand = new Command(HabilitarCampos);
            ConvidadosCommand = new Command(ChamarConvidadosGrupoView);
            ConvidarContatoCommand = new Command(ConvidarContatoGrupo);
            RemoverMembroCommand = new Command(RemoverMembro);
            DadosAutomaticos();
        }

        public void DadosAutomaticos()
        {
            servicoGrupo = new GrupoService();
            servicoGrupo.SalvarIdGrupoSelecionado();
            NomeView = servicoGrupo.ObterGrupoSelecionado()?.NomeGrupo;
            ObjetivoView = servicoGrupo.ObterGrupoSelecionado()?.ObjetivoGrupo;
            Contatos = new ObservableCollection<Usuario>(servicoGrupo.ObterMembrosDoGrupoContatos());
            Convidados = new ObservableCollection<ConviteGrupo>(servicoGrupo.ObterConvitesEnviadosDoGrupo());
            Membros = new ObservableCollection<Usuario>(servicoGrupo.ObterMembrosDoGrupo());
            novosConvites = new List<ConviteGrupo>();
        }

        private void HabilitarCampos()
        {
            CamposHabilitados = true;
        }

        private void ConvidarContatoGrupo()
        {
            if (servicoGrupo.ContatoNaoFoiConvidado(Convidados))
            {
                if (!servicoGrupo.ContatoJaColaboraNoProjeto())
                {
                    novosConvites.Add(servicoGrupo.CriarConviteGrupo());
                    Convidados.Add(servicoGrupo.CriarConviteGrupo());
                    servicoGrupo.EnviarConvitesGrupo(novosConvites, servicoGrupo.ObterGrupoSelecionado());
                }
            }
        }

        private async void RemoverMembro()
        {
            var resposta = await Application.Current.MainPage.DisplayAlert("Remoção de Usuário", "Remover este usuário do grupo?", "Sim", "Não");
            if (resposta)
            {
                servicoGrupo.RemoverUsuarioDoGrupo();
                Membros = new ObservableCollection<Usuario>(servicoGrupo.ObterMembrosDoGrupo());
            }
        }

        private void SalvarAlteracoes()
        {
            Model.Grupo modelGrupo = new Model.Grupo()
            {
                Id = servicoGrupo.ObterIdGrupoSelecionado(),
                NomeGrupo = NomeView,
                ObjetivoGrupo = ObjetivoView
            };
            servicoGrupo.AlterarGrupo(modelGrupo);
            Application.Current.MainPage.Navigation.PopAsync();
        }

        private void ChamarConvidadosGrupoView()
        {
            Application.Current.MainPage.Navigation.PushModalAsync(new NavigationPage(new GrupoConvidadosView()) { BarBackgroundColor = Color.FromHex("#3b5998") }, true);
        }
    }
}
