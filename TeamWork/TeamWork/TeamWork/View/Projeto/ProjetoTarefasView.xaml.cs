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
    public partial class ProjetoTarefasView : ContentPage
    {
        private ProjetoTarefasViewModel vm;
        public ProjetoTarefasView()
        {
            InitializeComponent();
            vm = new ProjetoTarefasViewModel();
            BindingContext = vm;
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
    }
}