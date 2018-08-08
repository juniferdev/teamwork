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
    public partial class ProjetoConvidadosView : ContentPage
    {
        public ProjetoConvidadosView()
        {
            InitializeComponent();
            BindingContext = new ProjetoConvidadosViewModel();
        }
    }
}