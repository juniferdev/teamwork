using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using TeamWork.Service;
using TeamWork.View;
using TeamWork.View.Conta;
using Xamarin.Forms;

namespace TeamWork.ViewModel.Conta
{
    public class EntrarViewModel : BaseViewModel
    {
        public string EmailView { get; set; }
        public string SenhaView { get; set; }

        public Command PrincipalViewCommand { get; set; }
        public Command CriarContaCommand { get; set; }
        public Command EsqueciMinhaSenhaCommand { get; set; }

        public ContaService servicoConta;

        public EntrarViewModel()
        {
            EmailView = "ff@gmail.com";
            SenhaView = "Fabicha1";
            PrincipalViewCommand = new Command(ChamarPrincipalView);
            CriarContaCommand = new Command(ChamarCriarContaView);
            EsqueciMinhaSenhaCommand = new Command(ChamarRedefinirSenhaView);
        }

        public void ChamarPrincipalView()
        {
            servicoConta = new ContaService(EmailView, SenhaView);
            if (servicoConta.AutenticarUsuario())
            {
                Application.Current.MainPage = new NavigationPage(new PrincipalView()) { BarBackgroundColor = Color.FromHex("#3b5998")};
            }
        }

        private void ChamarCriarContaView()
        {
            Application.Current.MainPage.Navigation.PushModalAsync(new NavigationPage(new CriarContaView()) { BarBackgroundColor = Color.FromHex("#3b5998") }, true);
        }

        private void ChamarRedefinirSenhaView()
        {
            Application.Current.MainPage.Navigation.PushModalAsync(new NavigationPage(new RecuperarContaView()) { BarBackgroundColor = Color.FromHex("#3b5998") }, true);
        }
    }

}