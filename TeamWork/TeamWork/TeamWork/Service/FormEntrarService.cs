using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using TeamWork.Repository;
using TeamWork.Model;
using TeamWork.Internal;
using Xamarin.Forms;

namespace TeamWork.Service
{
    public class FormEntrarService
    {
        #region Atributos e Propriedades

        private int IdUsuario;
        private string EmailBus;
        private string SenhaBus;
        private string AlertaForm;

        #endregion Atributos e Propriedades

        #region Construtores

        public FormEntrarService() { }

        #endregion Construtores

        #region Validações

        public bool ValidarFormEntrar()
        {
            #region Resumo
            // Verifica se campos obrigatórios do formulário foram preenchidos e 
            // se as informações contidas neles são válidas conforme as regras de negócio. 
            
            // Retorna Verdadeiro se os campos foram preenchidos e
            // se as informações são válidas conforme as regras de negócio. 
            // Caso um campo não tenham sido preenchido ou contenha qualquer informação inválida retorna Falso.
            #endregion Resumo

            if (CamposForamPreenchidos() && InformacoesSaoValidas())
            {
                if (AutenticarUsuario())
                {
                    return true;
                }
            }
            return false;
        }

        public bool CamposForamPreenchidos()
        {
            #region Resumo
            // Verifica se campos obrigatórios do formulário foram preenchidos.

            // Retorna Verdadeiro caso os campos obrigatórios tenham sido preenchidos
            // ou Falso caso pelo menos um deles não tenha sido.
            #endregion Resumo

            if (string.IsNullOrWhiteSpace(EmailBus) || string.IsNullOrWhiteSpace(SenhaBus))
            {
                // Mensagem: Favor preencher todos os campos.
                AlertaForm = Mensagem.MENS_FORM_01;
                return false;
            }
            return true;
        }

        public bool InformacoesSaoValidas()
        {
            #region Resumo
            // Verifica se as informações preenchidas nos campos do formulário
            // são válidas conforme as regras de negócio.

            //Retorna Verdadeiro se todas as informações são válidas
            // ou Falso caso pelo menos uma seja inválida.
            #endregion Resumo

            if (Regex.IsMatch(EmailBus, @"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$"))
            {
                return true;
            }
            else
            {
                // Mensagem: O e-mail informado não parece ser válido.
                AlertaForm = Mensagem.MENS_FORM_04;
                return false;
            }
        }

        #endregion Validações

        public void PassarPropriedades(string email, string senha)
        {
            EmailBus = email;
            SenhaBus = senha;
        }

        public void PassarPropriedades(int idUsuario)
        {
            IdUsuario = idUsuario;
        }

        public bool AutenticarUsuario()
        {
            #region Resumo
            // Consulta no banco de dados se os dados informados correspondem aos de alguma conta do usuário.
            // Retorna Verdadeiro se os dados equivalem aos dados de alguma conta. Caso contrário retorna Falso.
            #endregion Resumo

            UsuarioRepository dados = new UsuarioRepository();

                Usuario usuario = dados.AutenticarContaDeUsuario(EmailBus, SenhaBus);

                if (usuario == null)
                {
                    // Mensagem: Usuário e/ou senha inválidos.
                    AlertaForm = Mensagem.MENS_FORM_07;
                    return false;
                }
                Application.Current.Properties["id"] = usuario.Id;
                return true;
        }

        public string ConsultarUsuarioPorId()
        {
            UsuarioRepository dados = new UsuarioRepository();
            Usuario usuario = dados.ConsultarUsuarioPorId(IdUsuario);
            return usuario.Nome;
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
