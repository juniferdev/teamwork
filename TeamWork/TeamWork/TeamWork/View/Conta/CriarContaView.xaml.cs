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
	public partial class CriarContaView : ContentPage
    {
        public CriarContaView()
        {
            InitializeComponent();
            nome.Completed += (s, e) => email.Focus();
            email.Completed += (s, e) => senha.Focus();
            senha.Completed += (s, e) => confsenha.Focus();
            BindingContext = new CriarContaViewModel();
        }
    }
}