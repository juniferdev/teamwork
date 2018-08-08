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
    public partial class CriarGrupoView : TabbedPage
    {
        private CriarGrupoViewModel vm;
        //private Command RemoverUsuarioCommand;

        public CriarGrupoView()
        {
            InitializeComponent();
            vm = new CriarGrupoViewModel();
            BindingContext = vm;
            //RemoverUsuarioCommand = new Command(RemoverUsuario);
        }

        public void SelecionouContato(object sender, SelectedItemChangedEventArgs e)
        {
            var usuario = e.SelectedItem as Model.Usuario;
            Application.Current.Properties["idContato"] = usuario.Id;
            vm.HabilitarBotaoConvidar = true;
        }

        //public async void RemoverUsuario()
        //{
        //    var resposta = await DisplayAlert("Remoção de Usuário", "Deseja remover este usuário do grupo?", "Sim", "Não");
        //    if (resposta)
        //    {
        //        vm.servicoGrupo.SalvarIdContatoSelecionado();
        //        vm.servicoGrupo.RemoverUsuarioSelecionadoDoGrupo();
        //        usuList.ItemsSource = vm.ListarUsuariosDoGrupo();
        //    }

        //}

    }
}