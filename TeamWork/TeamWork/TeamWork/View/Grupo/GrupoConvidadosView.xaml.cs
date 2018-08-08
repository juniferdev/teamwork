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
    public partial class GrupoConvidadosView : ContentPage
    {
        public GrupoConvidadosView()
        {
            InitializeComponent();
            BindingContext = new GrupoConvidadosViewModel();
        }
    }
}