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
	public partial class EntrarView : ContentPage
	{
        public EntrarView()
        {
            InitializeComponent();
            email.Completed += (s, e) => senha.Focus();
            BindingContext = new EntrarViewModel();
        }

        private double width = 0;
        private double height = 0;

    }
}