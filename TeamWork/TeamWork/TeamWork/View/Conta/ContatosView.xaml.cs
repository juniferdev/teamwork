using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TeamWork.ViewModel.Conta;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TeamWork.View.Conta
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ContatosView : ContentPage
    {
        private Command ExcluirContatoCommand;
        private ContatosViewModel vm;

        public ContatosView()
        {
            InitializeComponent();
            vm = new ContatosViewModel();
            BindingContext = vm;
            ExcluirContatoCommand = new Command(ExcluirContato);
        }

        public void SelecionouContato(object sender, SelectedItemChangedEventArgs e)
        {
            var contato = e.SelectedItem as Model.Usuario;
            Application.Current.Properties["idContato"] = contato.Id; //contato é um objeto da classe Usuario
            if (ToolbarItems.Count < 1)
            {
                ToolbarItems.Add(new ToolbarItem() { Name = "Apagar", Icon = "usuariodel.png", Priority = 4, Command = ExcluirContatoCommand });
            }
        }

        public async void ExcluirContato()
        {
            var resposta = await DisplayAlert("Exclusão de Contato", "Deseja excluir este contato?", "Sim", "Não");
            if (resposta)
            {
                vm.servicoGrupo.SalvarIdContatoSelecionado();
                vm.servicoGrupo.ExcluirContatoSelecionado();
                contatoList.ItemsSource = vm.servicoGrupo.ObterMembrosDoGrupoContatos();
                LimparToolbar();
            }

        }

        public void LimparToolbar()
        {
            ToolbarItems.Clear();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            LimparToolbar();
            contatoList.BeginRefresh();
            contatoList.ItemsSource = vm.servicoGrupo.ObterMembrosDoGrupoContatos();
            contatoList.EndRefresh();
        }
    }
}