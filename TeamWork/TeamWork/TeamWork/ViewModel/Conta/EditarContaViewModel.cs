using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TeamWork.Model;
using TeamWork.Service;
using Xamarin.Forms;

namespace TeamWork.ViewModel.Conta
{
    public class EditarContaViewModel: BaseViewModel
    {
        public string nomeView { get; set; }
        public string NomeView { get { return nomeView; } set { nomeView = value; OnPropertyChanged(nameof(NomeView)); } }
        public string emailView { get; set; }
        public string EmailView { get { return emailView; } set { emailView = value; OnPropertyChanged(nameof(EmailView)); } }
        public string senhaView { get; set; }
        public string SenhaView { get { return senhaView; } set { senhaView = value; OnPropertyChanged(nameof(SenhaView)); } }
        public string confirmSenhaView { get; set; }
        public string ConfirmSenhaView { get { return confirmSenhaView; } set { confirmSenhaView = value; OnPropertyChanged(nameof(ConfirmSenhaView)); } }

        public Command EditarContaCommand { get; set; }
        private ContaService servicoConta;

        private Usuario usuario;

        public EditarContaViewModel()
        {
            EditarContaCommand = new Command(SalvarAlteracoes);
            servicoConta = new ContaService();
            usuario = servicoConta.ObterUsuarioPorIdLogado();
            NomeView = usuario.NomeUsuario;
            EmailView = usuario.Email;
        }

        private void SalvarAlteracoes()
        {
            servicoConta = new ContaService(NomeView, EmailView, SenhaView, ConfirmSenhaView);
            if (servicoConta.EditarContaDeUsuario())
            {
                Application.Current.MainPage.Navigation.PopModalAsync();
            }
        }
    }
}