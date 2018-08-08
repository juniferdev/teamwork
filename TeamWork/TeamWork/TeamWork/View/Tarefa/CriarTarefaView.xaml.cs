using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TeamWork.ViewModel.Tarefa;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TeamWork.View.Tarefa
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CriarTarefaView : TabbedPage
    {
        CriarTarefaViewModel vm;

        public CriarTarefaView ()
        {
            InitializeComponent();
            vm = new CriarTarefaViewModel();
            BindingContext = vm;
        }

        public void SelecionouContato(object sender, SelectedItemChangedEventArgs e)
        {
            var contato = e.SelectedItem as Model.Usuario;
            Application.Current.Properties["idResponsavel"] = contato.Id; //contato é um objeto da classe Usuario
            vm.HabilitarBotaoAtribuirTarefa = true;
        }

        private void SelecionouTipoTarefa(object sender, EventArgs e)
        {
            var picker = (Picker)sender;

            if (picker.SelectedIndex != -1)
            {
                vm.TipoTarefaView = (Internal.TipoTarefa) picker.SelectedIndex;
            }
        }

        private void SelecionouProjeto(object sender, EventArgs e)
        {
            var picker = (Picker)sender;

            if (picker.SelectedIndex != -1)
            {
                vm.ProjetoSelecionado = vm.Projetos[picker.SelectedIndex].NomeProjeto;
                Application.Current.Properties["idProjeto"] = vm.Projetos[picker.SelectedIndex].Id;
                vm.IdProjeto = vm.Projetos[picker.SelectedIndex].Id;
                vm.projetoDefinido = true;
            }
        }


        private void TextoAlteradoNaSearchBar(object sender, TextChangedEventArgs e)
        {
            //prjList.BeginRefresh();

            //if (!string.IsNullOrWhiteSpace(e.NewTextValue))
            //{

            //    prjList.ItemsSource = vm.Projetos.Where((p =>

            //    p.NomeProjeto.IndexOf(e.NewTextValue, StringComparison.OrdinalIgnoreCase) >= 0)

            //    );
            //}
            //else
            //{
            //    prjList.ItemsSource = vm.ListarProjetos();
            //}

            //prjList.EndRefresh();
        }

    }
}