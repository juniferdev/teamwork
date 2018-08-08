using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TeamWork.Model;
using TeamWork.Service;
using TeamWork.ViewModel.Projeto;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TeamWork.View.Projeto
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class ProjetosView : ContentPage
	{
        private ProjetosViewModel vm;
        public Command ExcluirProjetoCommand;
        private int idUsuarioLogado;
        private int idGerenteProjeto;

        public ProjetosView()
        {
            InitializeComponent();
            vm = new ProjetosViewModel();
            BindingContext = vm;
            ExcluirProjetoCommand = new Command(ExcluirProjeto);
        }

        private void TextoAlteradoNaSearchBar(object sender, TextChangedEventArgs e)
        {
            prjList.BeginRefresh();

            if (!string.IsNullOrWhiteSpace(e.NewTextValue))
            {

                prjList.ItemsSource = vm.Projetos.Where((p =>

                p.NomeProjeto.IndexOf(e.NewTextValue, StringComparison.OrdinalIgnoreCase) >= 0)

                );
            }
            else
            {
                prjList.ItemsSource = vm.ListarProjetos();
            }

            prjList.EndRefresh();
        }


        public void SelecionouProjeto(object sender, SelectedItemChangedEventArgs e)
        {
            LimparToolbar();
            var projeto = e.SelectedItem as Model.Projeto;
            Application.Current.Properties["idProjeto"] = projeto.Id;
            vm.servicoProjeto.SalvarIdProjetoSelecionado();
            idUsuarioLogado = (int) Application.Current.Properties["id"];
            idGerenteProjeto = vm.servicoProjeto.ObterGerenteProjeto().Id;
            if (ToolbarItems.Count <= 1)
            {
                ToolbarItems.Add(new ToolbarItem() { Name = "Visualizar", Icon = "prjview.png", Priority = 2, Command = vm.ProjetoDetalhesCommand });

                if (idUsuarioLogado == idGerenteProjeto)
                {
                    ToolbarItems.Add(new ToolbarItem() { Name = "Apagar", Icon = "prjdel.png", Priority = 4, Command = ExcluirProjetoCommand });
                }
            }
        }

        public async void ExcluirProjeto()
        {
            var resposta = await DisplayAlert("Exclusão de Projeto", "Deseja excluir este projeto?", "Sim", "Não");
            if (resposta)
            {
                vm.servicoProjeto.SalvarIdProjetoSelecionado();
                vm.servicoProjeto.ExcluirProjetoSelecionado();
                prjList.ItemsSource = vm.ListarProjetos();
                LimparToolbar();
            }
            
        }

        public void LimparToolbar()
        {
            ToolbarItems.Clear();
            ToolbarItems.Add(new ToolbarItem() { Name = "Adicionar", Icon = "prjadd.png", Command = vm.CriarProjetoCommand });
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            LimparToolbar();
            prjList.BeginRefresh();
            prjList.ItemsSource = vm.ListarProjetos();
            prjList.EndRefresh();
        }

    }
}