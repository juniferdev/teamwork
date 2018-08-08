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
    public partial class TarefaDetalhesView : TabbedPage
    {
        private TarefaDetalhesViewModel vm;

        public TarefaDetalhesView ()
        {
            InitializeComponent();
            vm = new TarefaDetalhesViewModel();
            BindingContext = vm;
        }

        private void SelecionouTipoTarefa(object sender, EventArgs e)
        {
            var picker = (Picker)sender;

            if (picker.SelectedIndex != -1)
            {
                vm.TipoTarefaView = (Internal.TipoTarefa)picker.SelectedIndex;
            }
        }

        private void SelecionouEstado(object sender, EventArgs e)
        {
            var picker = (Picker)sender;

            if (picker.SelectedIndex != -1)
            {
                vm.EstadoView = (Internal.Estado)picker.SelectedIndex;
                Application.Current.Properties["idEstado"] = vm.Estados[picker.SelectedIndex].Estado;
                vm.servicoTarefa.SalvarEstadoSelecionado();
                vm.RazaoView = vm.servicoTarefa.ObterMotivoTarefa(vm.servicoTarefa.ObterEstadoSelecionado(),vm.EstadoAnterior);
            }
        }

        private void SelecionouProjeto(object sender, EventArgs e)
        {
            var picker = (Picker)sender;

            if (picker.SelectedIndex != -1)
            {
                vm.ProjetoSelecionado = vm.Projetos[picker.SelectedIndex].NomeProjeto;
                Application.Current.Properties["idProjeto"] = vm.Projetos[picker.SelectedIndex].Id;
                vm.servicoTarefa.SalvarIdProjetoSelecionado();
                //vm.IdProjetoView = vm.Projetos[picker.SelectedIndex].Id;
            }
        }

        public void SelecionouContato(object sender, SelectedItemChangedEventArgs e)
        {
            var contato = e.SelectedItem as Model.Usuario;
            Application.Current.Properties["idResponsavel"] = contato.Id;
            vm.HabilitarBotaoAtribuirTarefa = true;
        }

        //private void TextoAlteradoNaSearchBar(object sender, TextChangedEventArgs e)
        //{
        //    trfList.BeginRefresh();

        //    if (!string.IsNullOrWhiteSpace(e.NewTextValue))
        //    {

        //        trfList.ItemsSource = vm.Projetos.Where((p =>

        //        p.NomeProjeto.IndexOf(e.NewTextValue, StringComparison.OrdinalIgnoreCase) >= 0)

        //        );
        //    }
        //    else
        //    {
        //        trfList.ItemsSource = vm.ListarProjetos();
        //    }

        //    trfList.EndRefresh();
        //}

        //protected override void OnAppearing()
        //{
        //    base.OnAppearing();
        //    LimparToolbar();
        //    trfList.BeginRefresh();
        //    trfList.ItemsSource = vm.();
        //    trfList.EndRefresh();
        //}
    }
}