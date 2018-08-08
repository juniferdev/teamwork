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
    public partial class UsuariosView : ContentPage
    {
        public UsuariosView()
        {
            InitializeComponent();
            BindingContext = new UsuariosViewModel();
        }
    }
}