using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TeamWork.ViewModel.Grupo;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TeamWork.View.Grupo
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class GruposView : ContentPage
	{
        private GruposViewModel vm;
        public Command ExcluirGrupoCommand;

        public int IdUsuarioLogado { get; private set; }
        public int IdCriador { get; private set; }

        public GruposView()
        {
            InitializeComponent();
            vm = new GruposViewModel();
            BindingContext = vm;
            ExcluirGrupoCommand = new Command(ExcluirGrupo);
        }

        private void SelecionouGrupo(object sender, SelectedItemChangedEventArgs e)
        {
            LimparToolbar();
            var grupo = e.SelectedItem as Model.Grupo;
            Application.Current.Properties["idGrupo"] = grupo.Id;
            vm.servicoGrupo.SalvarIdGrupoSelecionado();
            IdUsuarioLogado = (int) Application.Current.Properties["id"];
            IdCriador = vm.servicoGrupo.ObterCriadorGrupo().Id;
            if (ToolbarItems.Count <= 1)
            {
                ToolbarItems.Add(new ToolbarItem() { Name = "Visualizar", Icon = "teamview.png", Priority = 2, Command = vm.GrupoDetalhesCommand });
                if(IdUsuarioLogado == IdCriador)
                {
                    ToolbarItems.Add(new ToolbarItem() { Name = "Apagar", Icon = "teamdel.png", Priority = 4, Command = ExcluirGrupoCommand });
                }
            }
        }

        public async void ExcluirGrupo()
        {
            var resposta = await DisplayAlert("Exclusão de Grupo", "Deseja excluir este grupo?", "Sim", "Não");
            if (resposta)
            {
                vm.servicoGrupo.SalvarIdGrupoSelecionado();
                vm.servicoGrupo.ExcluirGrupoSelecionado();
                grpList.ItemsSource = vm.ListarGrupos();
                LimparToolbar();
            }

        }

        public void LimparToolbar()
        {
            ToolbarItems.Clear();
            ToolbarItems.Add(new ToolbarItem() { Name = "Adicionar", Icon = "teamadd.png", Command = vm.CriarGrupoCommand });
        }

        private void TextoAlteradoNaSearchBar(object sender, TextChangedEventArgs e)
        {
            grpList.BeginRefresh();

            if (!string.IsNullOrWhiteSpace(e.NewTextValue))
            {

                grpList.ItemsSource = vm.Grupos.Where((p =>

                p.NomeGrupo.IndexOf(e.NewTextValue, StringComparison.OrdinalIgnoreCase) >= 0)

                );
            }
            else
            {
                grpList.ItemsSource = vm.ListarGrupos();
            }

            grpList.EndRefresh();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            LimparToolbar();
            grpList.BeginRefresh();
            grpList.ItemsSource = vm.ListarGrupos();
            grpList.EndRefresh();
        }
    }
}