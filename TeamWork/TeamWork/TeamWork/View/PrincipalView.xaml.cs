using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TeamWork.View.Grupo;
using TeamWork.View.Projeto;
using TeamWork.View.Tarefa;
using TeamWork.ViewModel;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TeamWork.View
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class PrincipalView : ContentPage
	{
        private PrincipalViewModel vm;
        public PrincipalView()
        {
            InitializeComponent();
            vm = new PrincipalViewModel();
            BindingContext = vm;
        }

        private void VerificarConvitesDoUsuario()
        {
            if(ToolbarItems.Count() > 0)
            {
                ToolbarItems.Clear();
            }
            if (vm.servicoConta.UsuarioTemNovosConvites())
            {
                ToolbarItems.Add(new ToolbarItem() { Name = "Novo convite", Icon = "mailnew.png", Command = vm.NotificacoesCommand });
                AdicionarMenusOcultos();
            }
            else
            {
                ToolbarItems.Add(new ToolbarItem() { Name = "Convites", Icon = "mail.png", Command = vm.NotificacoesCommand });
                AdicionarMenusOcultos();
            }
        }

        public void AdicionarMenusOcultos()
        {
            ToolbarItems.Add(new ToolbarItem() { Text = "Contatos", Order = ToolbarItemOrder.Secondary, Command = vm.ContatosCommand });
            ToolbarItems.Add(new ToolbarItem() { Text = "Editar Conta", Order = ToolbarItemOrder.Secondary, Command = vm.EditarContaCommand });
            ToolbarItems.Add(new ToolbarItem() { Text = "Sair", Order = ToolbarItemOrder.Secondary , Command = vm.SairCommand  });
        }

        public void SelecionouTarefa(object sender, SelectedItemChangedEventArgs e)
        {
            var tarefa = e.SelectedItem as Model.Tarefa;
            Application.Current.Properties["idTarefa"] = tarefa.Id;
            vm.ChamarEditarTarefaView();
        }

        protected override bool OnBackButtonPressed()
        {
            return true;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            vm.NomeUsuario = "Bem vindo, " + vm.servicoConta.ObterUsuarioPorIdLogado().NomeUsuario;
            VerificarConvitesDoUsuario();
            vm.Tarefas = new ObservableCollection<Model.Tarefa>(vm.servicoTarefa.ObterTarefasDoUsuarioLogado(false).Where(c => c.Situacao == "Atrasada"));
        }
    }
}