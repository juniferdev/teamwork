using SQLite.Net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TeamWork.Internal;
using TeamWork.Model;
using TeamWork.Repository;
using Xamarin.Forms;
using System.Collections.ObjectModel;

namespace TeamWork.Service
{
    public class ProjetoService
    {
        private string NomeServ;
        private string DescricaoServ;
        private string ContratanteServ;
        private string ContratadaServ;
        private int IdGerenteServ;
        private DateTime DataPrevInicioServ;
        private DateTime DataPrevTerminoServ;
        private int IdProjetoSelecionado;
        private int IdContatoSelecionado;
        private int IdColaboradorSelecionado;

        private Usuario usuario;
        private Projeto projeto;
        private UsuarioProjeto usuarioProjeto;
        private ConviteProjeto conviteProjeto;

        private UsuarioRepository dadosUsuario;
        private ProjetoRepository dadosProjeto;
        private UsuarioProjetoRepository dadosUsuarioProjeto;
        private ConviteRepository dadosConvite;
        private TarefaProjetoRepository dadosTarefaProjeto;

        public ProjetoService() { }

        public ProjetoService(string nome, string descricao, string contratante,
                                       string contratada, DateTime dataPrevInicio, DateTime dataPrevTermino)
        {
            NomeServ = nome;
            DescricaoServ = descricao;
            ContratanteServ = contratante;
            ContratadaServ = contratada;
            DataPrevInicioServ = dataPrevInicio;
            DataPrevTerminoServ = dataPrevTermino;
        }

        public bool ValidarFormCriarProjeto()
        {
            /* Resumo: Valida o preenchimento e valor
             dos campos do formulário de criação de projeto
             conforme as regras de negócio. */

            if (CamposPreenchidos())
            {
                return true;
            }
            return false;
        }

        private bool CamposPreenchidos()
        {
            // Resumo: Verifica se os campos obrigatórios foram preenchidos.

            if (!string.IsNullOrWhiteSpace(NomeServ))
            {
                return true;
            }
            // Mensagem: Favor preencher todos os campos com *.
            Toast.ShortMessage(Mensagem.MENS_FORM_08);
            return false;
        }
        
        public bool CriarNovoProjeto()
        {
            // Resumo: Permite criar um novo projeto.

            if (ValidarFormCriarProjeto())
            {
                IdGerenteServ = (int)Application.Current.Properties["id"];

                projeto = new Projeto
                {
                    NomeProjeto = NomeServ,
                    IdGerente = IdGerenteServ,
                    DescricaoProjeto = DescricaoServ,
                    Contratante = ContratanteServ,
                    Contratada = ContratadaServ,
                    DataPrevInicio = DataPrevInicioServ,
                    DataPrevTermino = DataPrevTerminoServ
                };

                dadosProjeto = new ProjetoRepository();

                try
                {
                    // Mensagem: Erro ao incluir o novo projeto no banco de dados.
                    dadosProjeto.IncluirProjeto(projeto);
                }
                catch(SQLiteException ex)
                {
                    Toast.ShortMessage(Mensagem.MENS_FORM_13);
                }

                int id = dadosProjeto.UltimoIdInserido();

                usuarioProjeto = new UsuarioProjeto()
                {
                    IdUsuario = IdGerenteServ,
                    IdProjeto = id++
                };

                dadosUsuarioProjeto = new UsuarioProjetoRepository();

                try
                {
                    dadosUsuarioProjeto.IncluirUsuarioProjeto(usuarioProjeto);
                    
                    // Mensagem: Projeto criado com sucesso.
                    Toast.ShortMessage(Mensagem.MENS_FORM_09);
                    return true;
                }
                catch (SQLiteException ex)
                {
                    // Mensagem: Erro ao incluir associação de usuário e projeto no banco de dados.
                    Toast.ShortMessage(Mensagem.MENS_FORM_14);
                }
            }
            return false;
        }

        public Projeto ObterUltimoProjetoInserido()
        {
            if (dadosProjeto == null)
            {
                dadosProjeto = new ProjetoRepository();
            }
            return dadosProjeto.ConsultarProjeto(dadosProjeto.UltimoIdInserido());
        }

        public void SalvarIdProjetoSelecionado()
        {
            // Resumo: Identifica para a aplicação que agora vamos interagir com um projeto específico.

            IdProjetoSelecionado = (int)Application.Current.Properties["idProjeto"];
        }

        public int ObterIdProjetoSelecionado()
        {
            // Resumo: Devolve o identificador do projeto que estamos interagindo.

            return IdProjetoSelecionado;
        }

        public Projeto ObterProjetoSelecionado()
        {
            // Resumo: Permite consultar os dados do projeto selecionado.

            if (dadosProjeto == null)
            {
                dadosProjeto = new ProjetoRepository();
            }
            projeto = dadosProjeto.ConsultarProjeto(IdProjetoSelecionado);
            IdGerenteServ = projeto.IdGerente;
            return projeto;
        }

        public void AlterarProjeto(Projeto projeto)
        {
            // Resumo: Permite alterar dados de um projeto.

            if (dadosProjeto == null)
            {
                dadosProjeto = new ProjetoRepository();
            }
            try
            {
                dadosProjeto.AlterarProjeto(projeto);
                // Mensagem: Projeto alterado com sucesso.
                Toast.ShortMessage(Mensagem.MENS_FORM_16);
            }
            catch (SQLiteException ex)
            {
                // Mensagem: Erro ao tentar alterar projeto no banco de dados.
                Toast.ShortMessage(Mensagem.MENS_FORM_15);
            }
        }

        public void ExcluirProjetoSelecionado()
        {
            // Resumo: Permite excluir o projeto selecionado.

            if (dadosProjeto == null)
            {
                dadosProjeto = new ProjetoRepository();
            }
            dadosProjeto.DeletarProjeto(IdProjetoSelecionado);

            if(dadosConvite == null)
            {
                dadosConvite = new ConviteRepository();
            }
            dadosConvite.DeletarConvitesProjeto(IdProjetoSelecionado);
        }

        public Usuario ObterGerenteProjeto()
        {
            // Resumo: Retorna o gerente do projeto selecionado.

            if (dadosProjeto == null)
            {
                dadosProjeto = new ProjetoRepository();
            }

            IdGerenteServ = dadosProjeto.ConsultarProjeto(IdProjetoSelecionado).IdGerente;
            dadosUsuario = new UsuarioRepository();

            return dadosUsuario.ConsultarUsuarioPorId(IdGerenteServ);
        }

        public string ObterNomeGerenteProjeto()
        {
            // Resumo: Retorna o nome do gerente do projeto selecionado.

            if (dadosProjeto == null)
            {
                dadosProjeto = new ProjetoRepository();
            }

            IdGerenteServ = dadosProjeto.ConsultarProjeto(IdProjetoSelecionado).IdGerente;
            dadosUsuario = new UsuarioRepository();

            return dadosUsuario.ConsultarUsuarioPorId(IdGerenteServ).NomeUsuario;
        }

        public ConviteLista ObterConvitesDoUsuarioLogado()
        {
            int idUsuario = (int)Application.Current.Properties["id"];

            if (dadosConvite == null)
            {
                dadosConvite = new ConviteRepository();
            }

            return dadosConvite.ConsultarConvitesDoUsuario(idUsuario);
        }

        public void RemoverConvite(int idProjeto, int idUsuario)
        {
            if (dadosConvite == null)
            {
                dadosConvite = new ConviteRepository();
            }

            dadosConvite.DeletarConviteProjeto(idProjeto, idUsuario);
        }

        public void AdicionarUsuarioAoProjeto(int idUsuario, int idProjeto)
        {
            // Resumo: Adiciona um usuário definido a um grupo.
            usuarioProjeto = new UsuarioProjeto()
            {
                IdUsuario = idUsuario,
                IdProjeto = idProjeto
            };

            if(dadosUsuarioProjeto == null)
            {
                dadosUsuarioProjeto = new UsuarioProjetoRepository();
            }            

            try
            {
                dadosUsuarioProjeto.IncluirUsuarioProjeto(usuarioProjeto);

                // Mensagem: Você foi adicionado ao projeto.
                Toast.ShortMessage(Mensagem.MENS_FORM_39);
            }
            catch (SQLiteException ex)
            {
                // Mensagem: Erro ao incluir associação de usuário e projeto no banco de dados.
                Toast.ShortMessage(Mensagem.MENS_FORM_40);
            }
        }

        public void RemoverColaboradorDoProjeto()
        {
            if (dadosUsuarioProjeto == null)
            {
                dadosUsuarioProjeto = new UsuarioProjetoRepository();
            }

            IdColaboradorSelecionado = (int) Application.Current.Properties["idColaborador"];

            dadosUsuarioProjeto.DeletarUsuarioProjeto(IdColaboradorSelecionado, IdProjetoSelecionado);
        }

        public List<Usuario> ObterUsuariosDoProjeto()
        {
            // Resumo: Obtém os usuários que participam do projeto selecionado.

            if (dadosUsuarioProjeto == null)
            {
                dadosUsuarioProjeto = new UsuarioProjetoRepository();
            }
            return dadosUsuarioProjeto.ConsultarUsuariosDoProjeto(IdProjetoSelecionado);
        }

        public List<Tarefa> ObterTarefasDoProjeto(bool avisarListaVazia = true)
        {
            int IdProjeto = (int)Application.Current.Properties["idProjeto"];

            if (dadosTarefaProjeto == null)
            {
                dadosTarefaProjeto = new TarefaProjetoRepository();
            }

            var dados = dadosTarefaProjeto.ConsultarTarefasDoProjeto(IdProjeto);

            if (avisarListaVazia)
            {
                if (dados.Count == 0)
                {
                    // Mensagem: Nenhuma tarefa foi vinculada ao projeto. 
                    Toast.ShortMessage(Mensagem.MENS_FORM_51);
                }
            }

            return dados;
        }

        public List<Projeto> ObterProjetosDoUsuarioLogado(bool avisarListaVazia = true)
        {
            // Resumo: Obtém todos os projetos que o usuário logado é gerente ou participa.

            int idUsuario = (int)(Application.Current.Properties["id"]);

            if (dadosUsuarioProjeto == null)
            {
                dadosUsuarioProjeto = new UsuarioProjetoRepository();
            }

            var dados = dadosUsuarioProjeto.ConsultarProjetosDoUsuario(idUsuario); // melhorar este retorno.

            if (avisarListaVazia)
            {
                if (dados.Count == 0)
                {
                    // Mensagem: Você ainda não criou ou participa de nenhum projeto.
                    Toast.ShortMessage(Mensagem.MENS_FORM_21);
                }

            }

            return dados;
        }

        public bool ContatoNaoFoiConvidado(ObservableCollection<ConviteProjeto> listaConvites)
        { 
            // Verifica se o contato que desejamos convidar já recebeu um convite

            IdContatoSelecionado = (int)Application.Current.Properties["idContato"];

            foreach (var convite in listaConvites)
            {
                if (IdContatoSelecionado == convite.IdDestinatario)
                {
                    // Mensagem: Usuário informado já foi convidado,\n por favor aguarde a resposta.
                    Toast.ShortMessage(Mensagem.MENS_FORM_38);
                    return false;
                }
            }
            return true;
        }

        public bool ContatoJaColaboraNoProjeto()
        {
            IdContatoSelecionado = (int)Application.Current.Properties["idContato"];

            var usuariosDoProjeto = ObterUsuariosDoProjeto();

            foreach (var usuario in usuariosDoProjeto)
            {
                if (IdContatoSelecionado == usuario.Id)
                {
                    // Mensagem: Este contato já é um colaborador do projeto.
                    Toast.ShortMessage(Mensagem.MENS_FORM_41);
                    return true;
                }
            }
            return false;
        }

        public ConviteProjeto CriarConviteProjeto()
        {
            // Resumo: Cria um convite de projeto para um contato sem vincular os dados do projeto ainda.

            int idUsuario = (int)Application.Current.Properties["id"];

            if (dadosUsuario == null)
            {
                dadosUsuario = new UsuarioRepository();
            }

            var usuarioLogado = dadosUsuario.ConsultarUsuarioPorId(idUsuario);

            var contato = dadosUsuario.ConsultarUsuarioPorId(IdContatoSelecionado);

            var conviteProjeto = new ConviteProjeto
            {
                IdRemetente = usuarioLogado.Id,
                IdDestinatario = contato.Id,
                NomeRemetente = usuarioLogado.NomeUsuario,
                NomeDestinatario = contato.NomeUsuario
            };

            // Mensagem: Convite enviado para o usuário.
            Toast.ShortMessage(Mensagem.MENS_FORM_19);
            return conviteProjeto;
        }

        public void EnviarConvitesProjeto(List<ConviteProjeto> convites, Projeto projeto)
        {
            // Resumo: Envia todos os convites adicionados a lista de convites para os respectivos destinatários.
            foreach(var convite in convites)
            {
                convite.IdProjeto = projeto.Id;
                convite.NomeProjeto = projeto.NomeProjeto;
                convite.Convite = $"{convite.NomeRemetente} convidou você para o projeto {projeto.NomeProjeto}.";
            }

            if(dadosConvite == null)
            {
                dadosConvite = new ConviteRepository();
            }
            dadosConvite.IncluirConvitesProjeto(convites);
        }

        public List<ConviteProjeto> ObterConvitesEnviadosDoProjeto()
        {
            // Resumo: Obtém todos os convites que foram enviados para participar do projeto selecionado.
            if (dadosConvite == null)
            {
                dadosConvite = new ConviteRepository();
            }

            return dadosConvite.ConsultarConvitesProjetoEnviados((int)Application.Current.Properties["idProjeto"]);
        }

    }
}
