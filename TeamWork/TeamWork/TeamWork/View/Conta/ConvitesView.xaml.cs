using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TeamWork.Model;
using TeamWork.ViewModel.Conta;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TeamWork.View.Conta
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ConvitesView : TabbedPage
    {
        private ConvitesViewModel vm;
        private int idUsuarioLogado;
        //private int idRemetente;
        //private int idGrupoRemetente;
        //private int idGrupoDestinatario;
        private ConviteGrupo conviteGrupo;
        private ConviteProjeto conviteProjetoo;
        private int idProjeto;
        private Command AceitarGrupoContatosCommand { get; set; }
        private Command RejeitarGrupoContatosCommand { get; set; }
        private Command AceitarProjetoCommand { get; set; }
        private Command RejeitarProjetoCommand { get; set; }
        
        public ConvitesView()
        {
            InitializeComponent();
            vm = new ConvitesViewModel();
            BindingContext = vm;
            AceitarGrupoContatosCommand = new Command(AceitarConviteGrupo);
            RejeitarGrupoContatosCommand = new Command(RecusarConviteGrupo);
            AceitarProjetoCommand = new Command(AceitarConviteProjeto);
            RejeitarProjetoCommand = new Command(RecusarConviteProjeto);
            idUsuarioLogado = (int)Application.Current.Properties["id"];
        }

        public void SelecionouConviteGrupo(object sender, SelectedItemChangedEventArgs e)
        {
            conviteGrupo = e.SelectedItem as Model.ConviteGrupo;
            if (ToolbarItems.Count <= 1)
            {
                ToolbarItems.Add(new ToolbarItem() { Name = "Aceitar", Icon = "accept.png", Priority = 2, Command = AceitarGrupoContatosCommand });
                ToolbarItems.Add(new ToolbarItem() { Name = "Rejeitar", Icon = "reject.png", Priority = 4, Command = RejeitarGrupoContatosCommand });
            }
        }

        public void SelecionouConviteProjeto(object sender, SelectedItemChangedEventArgs e)
        {
            var convite = e.SelectedItem as Model.ConviteProjeto;
            idProjeto = convite.IdProjeto; ;
            if (ToolbarItems.Count <= 1)
            {
                ToolbarItems.Add(new ToolbarItem() { Name = "Aceitar", Icon = "accept.png", Priority = 2, Command = AceitarProjetoCommand });
                ToolbarItems.Add(new ToolbarItem() { Name = "Rejeitar", Icon = "reject.png", Priority = 4, Command = RejeitarProjetoCommand });
            }
        }

        public void AceitarConviteGrupo()
        {
            vm.servicoGrupo.AdicionarUsuarioAoGrupo(conviteGrupo);
            vm.servicoGrupo.RemoverConvite(conviteGrupo);
            conviteGrupos.ItemsSource = vm.servicoGrupo.ObterConvitesDoUsuarioLogado().ConvitesParaGrupos;
            LimparToolbar();
        }

        public void RecusarConviteGrupo()
        {
            vm.servicoGrupo.RemoverConvite(conviteGrupo);
            conviteGrupos.ItemsSource = vm.servicoGrupo.ObterConvitesDoUsuarioLogado().ConvitesParaGrupos;
            LimparToolbar();
        }

        public void AceitarConviteProjeto()
        {
            vm.servicoProjeto.AdicionarUsuarioAoProjeto(idUsuarioLogado, idProjeto);
            vm.servicoProjeto.RemoverConvite(idProjeto, idUsuarioLogado);
            conviteProjetos.ItemsSource = vm.servicoProjeto.ObterConvitesDoUsuarioLogado().ConvitesParaProjetos;
            LimparToolbar();
        }

        public void RecusarConviteProjeto()
        {
            vm.servicoProjeto.RemoverConvite(idProjeto, idUsuarioLogado);
            conviteProjetos.ItemsSource = vm.servicoProjeto.ObterConvitesDoUsuarioLogado().ConvitesParaProjetos;
            LimparToolbar();
        }

        public void LimparToolbar()
        {
            ToolbarItems.Clear();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            LimparToolbar();
            conviteGrupos.BeginRefresh();
            conviteGrupos.ItemsSource = vm.servicoGrupo.ObterConvitesDoUsuarioLogado().ConvitesParaGrupos;
            conviteGrupos.EndRefresh();

            conviteProjetos.BeginRefresh();
            conviteProjetos.ItemsSource = vm.servicoGrupo.ObterConvitesDoUsuarioLogado().ConvitesParaProjetos;
            conviteProjetos.EndRefresh();
        }
    }
}