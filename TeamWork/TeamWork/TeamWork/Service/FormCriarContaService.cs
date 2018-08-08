using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using TeamWork.Model;
using TeamWork.Repository;
using Xamarin.Forms;
using TeamWork.Internal;

namespace TeamWork.Service
{
    public class FormCriarContaService
    {
        #region Atributos e Propriedades

        private string NomeBus;
        private string EmailBus;
        private string SenhaBus;
        private string ConfirmSenhaBus;
        private string AlertaForm;

        private Usuario usuario;
        private UsuarioRepository dados;

        #endregion Atributos e Propriedades

        #region Construtores

        public FormCriarContaService(string nome, string email, string senha, string confirmSenha)
        {
            NomeBus = nome;
            EmailBus = email;
            SenhaBus = senha;
            ConfirmSenhaBus = confirmSenha;
        }

        #endregion Construtores

        #region Validações

        public void ValidarFormCriarConta()
        {
            #region Resumo
            // Verifica se os dados do formulário foram preenchidos e valida suas informações. 
            // Caso tenham sido preenchidos e sejam válidos conforme as regras de negócio, cria a conta do usuário.
            #endregion Resumo

            if (CamposForamPreenchidos() && InformacoesSaoValidas())
            {
                CriarContaUsuario();
            }
        }

        private bool CamposForamPreenchidos()
        {
            #region Resumo
            // Verifica se os campos obrigatórios do formulário foram preenchidos.
            // Retorna Verdadeiro se todos os campos foram preenchidos ou Falso caso pelo um deles não tenha sido preenchido.
            #endregion Resumo

            if (string.IsNullOrWhiteSpace(NomeBus) || string.IsNullOrWhiteSpace(EmailBus) ||
                string.IsNullOrWhiteSpace(SenhaBus) || string.IsNullOrWhiteSpace(ConfirmSenhaBus))
            {
                // Mensagem: Favor preencher todos os campos.
                AlertaForm = Mensagem.MENS_FORM_01;
                return false;
            }
            return true;
        }

        private bool InformacoesSaoValidas()
        {
            #region Resumo
            // Verifica se as informações que foram preenchidas nos campos são válidas conforme as regras de negócio.
            // Retorna Verdadeiro se todas as informações fornecidas são válidas ou Falso caso pelo menos uma seja inválida.
            #endregion Resumo

            if (Regex.IsMatch(EmailBus, @"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$"))
            {
                if (SenhaConformePolitica())
                {
                    if (SenhaBus.Equals(ConfirmSenhaBus))
                    {
                        return true;
                    }
                    // Mensagem: Senhas não correspondem. \n Favor digitar novamente.
                    AlertaForm = Mensagem.MENS_FORM_02;
                    return false;
                }
                // Mensagem: A senha não cumpre os requisitos mínimos de segurança...
                AlertaForm = Mensagem.MENS_FORM_03;
                return false;
            }
            else
            {
                // Mensagem: O e-mail informado não parece ser válido.
                AlertaForm = Mensagem.MENS_FORM_04;
                return false;
            }
        }

        private bool SenhaConformePolitica()
        {
            #region Resumo
            // Verifica se a senha informada obedece os requisitos de segurança da política de senha do sistema.
            // Retorna Verdadeiro se a senha obedece os requisitos ou Falso caso não esteja em conformidade.
            #endregion Resumo

            if (SenhaBus.Length >= 8)
            {
                if (SenhaBus.IndexOfAny("0123456789".ToCharArray()) != -1)
                {
                    if (SenhaBus.IndexOfAny("ABCDEFGHIJKLMNOPQRSTUVWXYZ".ToCharArray()) != -1)
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        #endregion Validações

        private void CriarContaUsuario()
        {
            #region Resumo
            // Verifica se o e-mail informado não está sendo usado por outra conta de usuário.
            // Caso não esteja, cria a conta de usuário no banco de dados.
            #endregion Resumo

            dados = new UsuarioRepository();

            Usuario usuario = dados.ConsultarUsuarioPorEmail(EmailBus);

            if (usuario == null)
            {
                usuario = new Usuario()
                {
                    Nome = NomeBus,
                    Email = EmailBus.ToLower(),
                    Senha = SenhaBus
                };
                dados.IncluirUsuario(usuario);
                // Mensagem: Usuário cadastrado com sucesso.
                AlertaForm = Mensagem.MENS_FORM_05;
            }
            else
            {
                // Mensagem: E-mail já cadastrado no sistema.
                AlertaForm = Mensagem.MENS_FORM_06;
            }            
        }

        public string GetAlert()
        {
            #region Resumo
            // Retorna a mensagem mais recente sobre as operações de criação da conta.
            #endregion Resumo

            return AlertaForm;
        }

    }
}
