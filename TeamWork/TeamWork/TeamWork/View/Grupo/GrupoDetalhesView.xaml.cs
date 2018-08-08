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
	public partial class GrupoDetalhesView : TabbedPage
	{
        private GrupoDetalhesViewModel vm;
        private int idUsuarioLogado;
        private int idCriadorGrupo;

        public GrupoDetalhesView ()
		{
            InitializeComponent();
            vm = new GrupoDetalhesViewModel();
            BindingContext = vm;
		}

        public void SelecionouContato(object sender, SelectedItemChangedEventArgs e)
        {
            var contato = e.SelectedItem as Model.Usuario;
            Application.Current.Properties["idContato"] = contato.Id; //contato é um objeto da classe Usuario
            vm.HabilitarBotaoConvidar = true;
        }

        public void SelecionouMembro(object sender, SelectedItemChangedEventArgs e)
        {
            var membro = e.SelectedItem as Model.Usuario;
            Application.Current.Properties["idMembro"] = membro.Id; //contato é um objeto da classe Usuario
            vm.HabilitarBotaoRemover = true;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            idUsuarioLogado = (int)Application.Current.Properties["id"];
            idCriadorGrupo = vm.servicoGrupo.ObterCriadorGrupo().Id;
            ToolbarItems.Clear();
            if (idUsuarioLogado == idCriadorGrupo)
            {
                ToolbarItems.Add(new ToolbarItem() { Name = "Editar", Icon = "edit.png", Command = vm.EditarGrupoCommand });
                ToolbarItems.Add(new ToolbarItem() { Name = "Salvar", Icon = "save.png", Command = vm.SalvarCommand });
                ToolbarItems.Add(new ToolbarItem() { Name= "Convidados", Icon = "mailsent.png", Command = vm.ConvidadosCommand});
            }
        }
    }
}