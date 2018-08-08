using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TeamWork.Internal;
using TeamWork.Model;
using TeamWork.Service;
using Xamarin.Forms;

namespace TeamWork.ViewModel.Conta
{
    public class ContatosViewModel: BaseViewModel
    {
        public string EmailView { get; set; }
        public ObservableCollection<Usuario> contatos;
        public ObservableCollection<Usuario> Contatos { get { return contatos; } set { contatos = value; OnPropertyChanged(nameof(Contatos)); } }

        public Command PesquisarCommand { get; set; }

        private Usuario usuario;
        private ContaService servicoConta;
        public GrupoService servicoGrupo;

        public ContatosViewModel()
        {
            PesquisarCommand = new Command(PesquisarUsuario);
            servicoConta = new ContaService();
            servicoGrupo = new GrupoService();
            Contatos = new ObservableCollection<Usuario>(servicoGrupo.ObterMembrosDoGrupoContatos());
        }

        public async void PesquisarUsuario()
        {
            usuario = servicoConta.ObterUsuarioPorEmail(EmailView);
            if (usuario == null)
            {
                // Mensagem: Nenhum usuário cadastrado com o e-mail informado.
                Toast.ShortMessage(Mensagem.MENS_FORM_27);
            }
            else
            {
                var resposta = await Application.Current.MainPage.DisplayAlert("Usuário encontrado!", $"{usuario.NomeUsuario} \n\nEnviar solicitação de amizade?", "Sim", "Não");
                if (resposta)
                {
                    servicoGrupo.CriarConviteGrupoContatos(usuario);
                }
            }
        }

    }
}
