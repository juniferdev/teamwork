using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TeamWork.ViewModel;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TeamWork.View
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class TelaTeste : ContentPage
	{
        private TelaTesteViewModel vm;

        public TelaTeste ()
		{
            InitializeComponent();
            vm = new TelaTesteViewModel();
            BindingContext = vm;
        }

        private void Picker_SelectedIndexChanged(object sender, EventArgs e)
        {
            var picker = (Picker)sender;

            if (picker.SelectedIndex != -1)
            {
                vm.ProjetoSelecionado = vm.Projetos[picker.SelectedIndex].Id.ToString();
            }

            //Application.Current.Properties["idProjeto"] = vm.Projetos[picker.SelectedIndex].Id;
            //vm.servicoProjeto.SalvarIdProjetoSelecionado();
        }
    }
}