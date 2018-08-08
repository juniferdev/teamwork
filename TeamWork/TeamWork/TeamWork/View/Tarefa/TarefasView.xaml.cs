using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TeamWork.Model;
using TeamWork.ViewModel.Tarefa;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TeamWork.View.Tarefa
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class TarefasView : ContentPage
    {
        public Command ExcluirTarefaCommand;
        private TarefasViewModel vm;
        private int IdCriador, IdUsuarioLogado;

        public TarefasView()
        {
            InitializeComponent();
            vm = new TarefasViewModel();
            BindingContext = vm;
            ExcluirTarefaCommand = new Command(ExcluirTarefaView);
        }

        private void TextoAlteradoNaSearchBar(object sender, TextChangedEventArgs e)
        {
            tskList.BeginRefresh();

            if (!string.IsNullOrWhiteSpace(e.NewTextValue))
            {

                tskList.ItemsSource = vm.Tarefas.Where((p =>

                p.NomeTarefa.IndexOf(e.NewTextValue, StringComparison.OrdinalIgnoreCase) >= 0)

                );
            }
            else
            {
                tskList.ItemsSource = vm.ListarTarefas();
            }

            tskList.EndRefresh();
        }

        public void SelecionouTarefa(object sender, SelectedItemChangedEventArgs e)
        {
            LimparToolbar();
            var tarefa = e.SelectedItem as Model.Tarefa;
            Application.Current.Properties["idTarefa"] = tarefa.Id;
            vm.servicoTarefa.SalvarIdTarefaSelecionada();
            IdUsuarioLogado = (int) Application.Current.Properties["id"];
            IdCriador = vm.servicoTarefa.ObterCriadorTarefa().Id;
            if (ToolbarItems.Count <= 1)
            {
                ToolbarItems.Add(new ToolbarItem { Name="Visualizar", Icon = "taskview.png", Command = vm.EditarTarefaCommand });

                if (IdUsuarioLogado == IdCriador)
                { 
                    ToolbarItems.Add(new ToolbarItem { Name = "Apagar", Icon = "taskdel.png", Command = ExcluirTarefaCommand });
                }
            }
        }

            public async void ExcluirTarefaView()
        {
            var resposta = await DisplayAlert("Exclusão de Tarefa", "Tem certeza que deseja" + Environment.NewLine + "excluir esta tarefa? ", "Sim", "Não");
            if (resposta)
            {
                vm.servicoTarefa.SalvarIdTarefaSelecionada();
                vm.servicoTarefa.ExcluirTarefaSelecionada();
                tskList.ItemsSource = vm.ListarTarefas();
            }
            LimparToolbar();
        }

        public void SelecionouOResponsavel(object sender, SelectedItemChangedEventArgs ev)
        {
            var responsavelTarefa = ev.SelectedItem as Model.Usuario;
            Application.Current.Properties["idResponsavel"] = responsavelTarefa.Id;

        }

        public void LimparToolbar()
        {
            ToolbarItems.Clear();
            ToolbarItems.Add(new ToolbarItem() { Name = "Adicionar", Icon = "taskadd.png", Command = vm.CriarTarefaCommand });
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            LimparToolbar();
            tskList.BeginRefresh();
            tskList.ItemsSource = vm.ListarTarefas();
            tskList.EndRefresh();
        }

    }

}