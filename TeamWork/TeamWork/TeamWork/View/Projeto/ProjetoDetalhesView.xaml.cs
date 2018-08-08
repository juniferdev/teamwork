using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TeamWork.ViewModel.Projeto;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TeamWork.View.Projeto
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ProjetoDetalhesView : TabbedPage
    {
        private ProjetoDetalhesViewModel vm;
        private int idUsuarioLogado;
        private int idGerenteProjeto;

        public ProjetoDetalhesView ()
        {
            InitializeComponent();
            vm = new ProjetoDetalhesViewModel();
            BindingContext = vm;
        }

        public void SelecionouContato(object sender, SelectedItemChangedEventArgs e)
        {
            var contato = e.SelectedItem as Model.Usuario;
            Application.Current.Properties["idContato"] = contato.Id; //contato é um objeto da classe Usuario
            vm.HabilitarBotaoConvidar = true;
        }

        public void SelecionouColaborador(object sender, SelectedItemChangedEventArgs e)
        {
            var colaborador = e.SelectedItem as Model.Usuario;
            Application.Current.Properties["idColaborador"] = colaborador.Id; //contato é um objeto da classe Usuario
            vm.HabilitarBotaoRemover = true;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            idUsuarioLogado = (int) Application.Current.Properties["id"];
            idGerenteProjeto = vm.servicoProjeto.ObterGerenteProjeto().Id;
            ToolbarItems.Clear();
            if (idUsuarioLogado == idGerenteProjeto)
            {
                ToolbarItems.Add(new ToolbarItem() { Name = "Editar", Icon = "edit.png", Command = vm.EditarPrjetoCommand });
                ToolbarItems.Add(new ToolbarItem() { Name = "Salvar", Icon = "save.png", Command = vm.SalvarCommand });
            }
            ToolbarItems.Add(new ToolbarItem() { Name = "Convidados", Icon = "mailsent.png", Command = vm.ConvidadosCommand });
        }

    }
}