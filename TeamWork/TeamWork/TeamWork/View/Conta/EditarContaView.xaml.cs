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
    public partial class EditarContaView : ContentPage
    {
        public EditarContaView()
        {
            InitializeComponent();
            BindingContext = new EditarContaViewModel();
        }

    }
}