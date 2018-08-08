using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TeamWork.Model;
using TeamWork.Repository;
using TeamWork.Service;
using TeamWork.View;
using Xamarin.Forms;

namespace TeamWork.ViewModel.Conta
{
    public class CriarContaViewModel: BaseViewModel
    {
        public string NomeView { get; set; }
        public string EmailView { get; set; }
        public string SenhaView { get; set; }
        public string ConfirmSenhaView { get; set; }

        public Command CadastrarCommand { get; set; }

        //Remover o command abaixo depois
        public Command UsuariosCommand { get; set; }

        public ContaService servicoConta;
        public GrupoService servicoGrupo;

        public CriarContaViewModel()
        {
            CadastrarCommand = new Command(Cadastrar);
            
            // Remover o command abaixo.
            UsuariosCommand = new Command(ChamarUsuariosView);
        }

        public void Cadastrar()
        {
            servicoConta = new ContaService(NomeView, EmailView, SenhaView, ConfirmSenhaView);

            if (servicoConta.CriarContaUsuario())
            {
                servicoGrupo = new GrupoService();
                servicoGrupo.CriarNovoGrupo(true, false); 
            }
        }

        // Remover este método. Foi criado somente para comprovar a inclusão do usuário no banco
        public void ChamarUsuariosView()
        {
            Application.Current.MainPage.Navigation.PushModalAsync(new NavigationPage(new UsuariosView()) { BarBackgroundColor = Color.FromHex("#3b5998") });
        }

    }
}
