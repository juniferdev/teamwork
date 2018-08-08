using SQLite.Net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using TeamWork.Internal;
using TeamWork.Model;
using TeamWork.Repository;
using Xamarin.Forms;

namespace TeamWork.Service
{
    public class ContaService
    {
        private string NomeServ;
        private string EmailServ;
        private string SenhaServ;
        private string ConfirmSenhaServ;

        private Usuario usuario;
        private ConviteLista convites;
        private UsuarioRepository dadosUsuario;
        private UsuarioProjetoRepository dadosUsuarioProjeto;
        private ConviteRepository dadosConvite;

        #region Construtores

        public ContaService()
        {
            // Remover depois. Sendo usado na CriarProjetoViewModel.
        }

        public ContaService(string email)
        {
            // Para VM Redefinir Senha
            EmailServ = email;
        }

        public ContaService(string email, string senha)
        {
            // Para VM Entrar
            EmailServ = email;
            SenhaServ = senha;
        }

        public ContaService(string nome, string email, string senha, string confirmSenha)
        {
            // Para VM Criar Conta
            NomeServ = nome;
            EmailServ = email;
            SenhaServ = senha;
            ConfirmSenhaServ = confirmSenha;
        }

        #endregion Construtores

        #region CriarConta

        private bool ValidarFormCriarConta()
        {
            #region Resumo
            // Verifica se os dados do formulário foram preenchidos e valida suas informações. 
            // Caso tenham sido preenchidos e sejam válidos conforme as regras de negócio, cria a conta do usuário.
            #endregion Resumo

            if (CamposPreenchidosCriarConta() && InformacoesValidasCriarConta())
            {
                return true;
            }
            return false;
        }

        private bool CamposPreenchidosCriarConta()
        {
            #region Resumo
            // Verifica se os campos obrigatórios do formulário foram preenchidos.
            // Retorna Verdadeiro se todos os campos foram preenchidos ou Falso caso pelo um deles não tenha sido preenchido.
            #endregion Resumo

            if (string.IsNullOrWhiteSpace(NomeServ) || string.IsNullOrWhiteSpace(EmailServ) ||
                string.IsNullOrWhiteSpace(SenhaServ) || string.IsNullOrWhiteSpace(ConfirmSenhaServ))
            {
                // Mensagem: Favor preencher todos os campos.
                Toast.ShortMessage(Mensagem.MENS_FORM_01);
                return false;
            }
            return true;
        }

        private bool InformacoesValidasCriarConta()
        {
            #region Resumo
            // Verifica se as informações que foram preenchidas nos campos são válidas conforme as regras de negócio.
            // Retorna Verdadeiro se todas as informações fornecidas são válidas ou Falso caso pelo menos uma seja inválida.
            #endregion Resumo

            if (Regex.IsMatch(EmailServ, @"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$"))
            {
                if (SenhaConformePolitica())
                {
                    if (SenhaServ.Equals(ConfirmSenhaServ))
                    {
                        return true;
                    }
                    // Mensagem: Senhas não correspondem. \n Favor digitar novamente.
                    Toast.ShortMessage(Mensagem.MENS_FORM_02);
                    return false;
                }
                return false;
            }
            else
            {
                // Mensagem: O e-mail informado não parece ser válido.
                Toast.ShortMessage(Mensagem.MENS_FORM_04);
                return false;
            }
        }

        private bool SenhaConformePolitica()
        {
            #region Resumo
            // Verifica se a senha informada obedece os requisitos de segurança da política de senha do sistema.
            // Retorna Verdadeiro se a senha obedece os requisitos ou Falso caso não esteja em conformidade.
            #endregion Resumo

            if (SenhaServ.Length >= 8)
            {
                if (SenhaServ.IndexOfAny("0123456789".ToCharArray()) != -1)
                {
                    if (SenhaServ.IndexOfAny("ABCDEFGHIJKLMNOPQRSTUVWXYZ".ToCharArray()) != -1)
                    {
                        return true;
                    }
                }
            }
            // Mensagem: A senha não cumpre os requisitos mínimos de segurança...
            Toast.LongMessage(Mensagem.MENS_FORM_03);
            return false;
        }

        public bool CriarContaUsuario()
        {
            #region Resumo
            // Verifica se o e-mail informado não está sendo usado por outra conta de usuário.
            // Caso não esteja, cria a conta de usuário no banco de dados.
            #endregion Resumo

            if (ValidarFormCriarConta())
            {
                dadosUsuario = new UsuarioRepository();

                var usuarioTemp = dadosUsuario.ConsultarUsuarioPorEmail(EmailServ);

                if (usuarioTemp == null)
                {
                    usuario = new Usuario()
                    {
                        NomeUsuario = NomeServ,
                        Email = EmailServ.ToLower(),
                        Senha = SenhaServ
                    };
                    try
                    {
                        dadosUsuario.IncluirUsuario(usuario);
                        // Mensagem: Usuário cadastrado com sucesso.
                        Toast.ShortMessage(Mensagem.MENS_FORM_05);
                        return true;
                    }
                    catch (SQLiteException ex)
                    {
                        // Mensagem: Erro ao tentar incluir o novo usuário no banco de dados.
                        Toast.ShortMessage(Mensagem.MENS_FORM_11);
                    }
                }
                else
                {
                    // Mensagem: E-mail já cadastrado no sistema.
                    Toast.ShortMessage(Mensagem.MENS_FORM_06);
                }
            }
            return false;
        }

        #endregion CriarConta

        #region Entrar

        private bool ValidarFormEntrar()
        {
            #region Resumo
            // Verifica se campos obrigatórios do formulário foram preenchidos e 
            // se as informações contidas neles são válidas conforme as regras de negócio. 

            // Retorna Verdadeiro se os campos foram preenchidos e
            // se as informações são válidas conforme as regras de negócio. 
            // Caso um campo não tenham sido preenchido ou contenha qualquer informação inválida retorna Falso.
            #endregion Resumo

            if (CamposPreenchidosEntrar() && InformacoesValidasEntrar())
            {
                return true;
            }
            return false;
        }

        private bool CamposPreenchidosEntrar()
        {
            #region Resumo
            // Verifica se campos obrigatórios do formulário foram preenchidos.

            // Retorna Verdadeiro caso os campos obrigatórios tenham sido preenchidos
            // ou Falso caso pelo menos um deles não tenha sido.
            #endregion Resumo

            if (string.IsNullOrWhiteSpace(EmailServ) || string.IsNullOrWhiteSpace(SenhaServ))
            {
                // Mensagem: Favor preencher todos os campos.
                Toast.ShortMessage(Mensagem.MENS_FORM_01);
                return false;
            }
            return true;
        }

        private bool InformacoesValidasEntrar()
        {
            #region Resumo
            // Verifica se as informações preenchidas nos campos do formulário
            // são válidas conforme as regras de negócio.

            //Retorna Verdadeiro se todas as informações são válidas
            // ou Falso caso pelo menos uma seja inválida.
            #endregion Resumo

            if (Regex.IsMatch(EmailServ, @"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$"))
            {
                return true;
            }
            else
            {
                // Mensagem: O e-mail informado não parece ser válido.
                Toast.ShortMessage(Mensagem.MENS_FORM_04);
                return false;
            }
        }

        public bool AutenticarUsuario()
        {
            #region Resumo
            // Consulta no banco de dados se os dados informados correspondem aos de alguma conta do usuário.
            // Retorna Verdadeiro se os dados equivalem aos dados de alguma conta. Caso contrário retorna Falso.
            #endregion Resumo

            if (ValidarFormEntrar())
            {
                dadosUsuario = new UsuarioRepository();

                try
                {
                    usuario = dadosUsuario.AutenticarContaDeUsuario(EmailServ, SenhaServ);
                    if (usuario != null)
                    {
                        Application.Current.Properties["id"] = usuario.Id;
                        return true;
                    }
                    else
                    {
                        // Mensagem: Usuário e/ou senha inválidos.
                        Toast.ShortMessage(Mensagem.MENS_FORM_07);
                    }
                }
                catch(SQLiteException ex)
                {
                    // Mensagem: Erro ao consultar informações da conta no banco de dados.
                    Toast.ShortMessage(Mensagem.MENS_FORM_12);
                }
            }
            return false;
        }

        #endregion Entrar

        public Usuario ObterUsuarioPorIdLogado()
        {
            int idUsuario = (int)Application.Current.Properties["id"];
            dadosUsuario = new UsuarioRepository();
            Usuario usuario = dadosUsuario.ConsultarUsuarioPorId(idUsuario);
            return usuario;
        }

        public Usuario ObterUsuarioPorEmail(string email)
        {
            dadosUsuario = new UsuarioRepository();
            Usuario usuario = dadosUsuario.ConsultarUsuarioPorEmail(email);
            return usuario;
        }

        public bool UsuarioTemNovosConvites()
        {
            if(dadosConvite == null)
            {
                dadosConvite = new ConviteRepository();
            }

            int idUsuario = (int)Application.Current.Properties["id"];

            var convites = dadosConvite.ConsultarConvitesDoUsuario(idUsuario);

            return convites.ConvitesParaGrupos.Count() > 0 ||
                   convites.ConvitesParaProjetos.Count() > 0;
        }

        public bool EditarContaDeUsuario()
        {
            // Resumo: Permite alterar dados da conta do usuário.

            if (ValidarFormCriarConta())
            {
                usuario = new Usuario
                {
                    NomeUsuario = NomeServ,
                    Email = EmailServ,
                    Senha = SenhaServ
                };

                if (dadosUsuario == null)
                {
                    dadosUsuario = new UsuarioRepository();
                }
                try
                {
                    int idUsuario = (int)Application.Current.Properties["id"];

                    dadosUsuario.AlterarUsuario(idUsuario, usuario);

                    // Mensagem: Seus dados foram alterados com sucesso.
                    Toast.ShortMessage(Mensagem.MENS_FORM_44);

                    return true;
                }
                catch (SQLiteException ex)
                {
                    // Mensagem: Erro ao tentar alterar usuário no banco de dados.
                    Toast.ShortMessage(Mensagem.MENS_FORM_45);
                }
            }
            return false;
        }
    }
}
