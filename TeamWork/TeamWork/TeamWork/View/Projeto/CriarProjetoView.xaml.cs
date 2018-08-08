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
    public partial class CriarProjetoView : TabbedPage
    {
        CriarProjetoViewModel vm;

        public CriarProjetoView ()
        {
            InitializeComponent();
            vm = new CriarProjetoViewModel();
            BindingContext = vm;
        }

        public void SelecionouContato(object sender, SelectedItemChangedEventArgs e)
        {
            var contato = e.SelectedItem as Model.Usuario;
            Application.Current.Properties["idContato"] = contato.Id; //contato é um objeto da classe Usuario
            vm.HabilitarBotaoConvidar = true;
        }
    }
}