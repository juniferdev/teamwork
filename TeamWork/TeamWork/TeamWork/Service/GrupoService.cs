using SQLite.Net;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TeamWork.Internal;
using TeamWork.Model;
using TeamWork.Repository;
using Xamarin.Forms;

namespace TeamWork.Service
{
    public class GrupoService
    {
        private string NomeServ;
        private string ObjetivoServ;
        private int IdCriadorServ;
        private int IdGrupoPaiServ;
        private int IdGrupoSelecionado;
        private int IdContatoSelecionado;
        private int IdMembroSelecionado;

        private Usuario usuario;
        private Grupo grupo;
        private UsuarioGrupo usuarioGrupo;
        private UsuarioGrupo usuario2Grupo;
        private ConviteGrupo conviteGrupo;

        private UsuarioRepository dadosUsuario;
        private GrupoRepository dadosGrupo;
        private UsuarioGrupoRepository dadosUsuarioGrupo;
        private ConviteRepository dadosConvite;

        public GrupoService() { }
        public GrupoService(string nome, string objetivo)
        {
            NomeServ = nome;
            ObjetivoServ = objetivo;
        }

        public bool ValidarFormCriarGrupo()
        {
            /* Resumo: Valida o preenchimento e valor
            dos campos do formulário de criação de grupo
            conforme as regras de negócio. */

            if (CamposPreenchidos())
            {
                return true;
            }
            return false;
        }

        public bool CamposPreenchidos()
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

        public bool CriarNovoGrupo(bool grupoContatos, bool incluirCriador)
        {
            // Resumo: Permite criar um grupo.

            if (dadosUsuario == null)
            {
                dadosUsuario = new UsuarioRepository();
            }
            if (dadosGrupo == null)
            {
                dadosGrupo = new GrupoRepository();
            }

            if (grupoContatos)
            {
                IdCriadorServ = dadosUsuario.UltimoIdInserido();

                grupo = new Grupo
                {
                    IdCriador = IdCriadorServ,
                    Contatos = true
                };

                dadosGrupo.IncluirGrupoContatos(grupo);
                return true;
            }
            else
            {
                IdCriadorServ = (int) Application.Current.Properties["id"];
                IdGrupoPaiServ = ObterGrupoContatosDoUsuarioLogado().Id;

                if (ValidarFormCriarGrupo())
                {
                    grupo = new Grupo
                    {
                        IdCriador = IdCriadorServ,
                        IdGrupoPai = IdGrupoPaiServ,
                        NomeGrupo = NomeServ,
                        ObjetivoGrupo = ObjetivoServ,
                        Contatos = false
                    };

                    dadosGrupo.IncluirGrupo(grupo);

                    int idGrupo = dadosGrupo.UltimoIdInserido();

                    if (incluirCriador)
                    {
                        usuarioGrupo = new UsuarioGrupo()
                        {
                            IdUsuario = IdCriadorServ,
                            IdGrupo = idGrupo++
                        };

                        if (dadosUsuarioGrupo == null)
                        {
                            dadosUsuarioGrupo = new UsuarioGrupoRepository();
                        }

                        try
                        {
                            dadosUsuarioGrupo.IncluirUsuarioGrupo(usuarioGrupo);

                            //Mensagem: Grupo criado com sucesso.
                            Toast.ShortMessage(Mensagem.MENS_FORM_24);
                            return true;
                        }
                        catch (SQLiteException ex)
                        {
                            // Mensagem: Erro ao incluir associação de usuário e grupo no banco de dados.
                            Toast.ShortMessage(Mensagem.MENS_FORM_14);
                        }
                    }
                    else
                    {
                        //Mensagem: Grupo criado com sucesso.
                        Toast.ShortMessage(Mensagem.MENS_FORM_24);
                        return true;
                    }                    
                }
                return false;
            }
        }

        // Futuramente
        public void CriarCopiaDoGrupoSelecionado(bool incluirCriador)
        {
            // Resumo: Permite criar um grupo que inicialmente conterá membros que já pertencem a outro grupo.
        }

        public void CriarConviteGrupoContatos(Usuario usuario)
        {
            // Resumo: Envia um convite a um usuário para adicioná-lo ao grupo contatos.

            // Parte 1: Valida se o usuário informado não é o seu próprio usuário

            int idUsuario = (int) Application.Current.Properties["id"];

            if (dadosUsuario == null)
            {
                dadosUsuario = new UsuarioRepository();
            }

            var usuarioLogado = dadosUsuario.ConsultarUsuarioPorId(idUsuario);

            if (usuario.Email == usuarioLogado.Email)
            {
                // Mensagem: Não é possível adicionar você mesmo aos seus contatos.
                Toast.ShortMessage(Mensagem.MENS_FORM_28);
            }
            else
            {
                // Parte 2: Valida se o usuário informado já não é um contato

                if(dadosUsuarioGrupo == null)
                {
                    dadosUsuarioGrupo = new UsuarioGrupoRepository();
                }

                var contatosDoUsuarioLogado = ObterGrupoContatosDoUsuarioLogado();

                int idGrupoDestinatario = ObterGrupoContatosDoUsuario(usuario.Id).Id;

                List<Usuario> meusContatos = dadosUsuarioGrupo.ConsultarUsuariosDoGrupo(contatosDoUsuarioLogado.Id);

                foreach(var contato in meusContatos)
                {
                    if (usuario.Email == contato.Email)
                    {
                        Toast.ShortMessage(Mensagem.MENS_FORM_37);
                        return;
                    }
                }

                // Parte 3: Criação do convite para o grupo

                if (dadosConvite == null)
                {
                    dadosConvite = new ConviteRepository();
                }

                List<ConviteGrupo> convitesEnviados = dadosConvite.ConsultarConvitesEnviadosDoUsuario(idUsuario).ConvitesParaGrupos;

                foreach(var convite in convitesEnviados)
                {
                    if (usuario.Email == convite.Email)
                    {
                        Toast.LongMessage(Mensagem.MENS_FORM_38);
                        return;
                    }
                }

                conviteGrupo = new ConviteGrupo
                {
                    IdGrupoRemetente = contatosDoUsuarioLogado.Id,
                    IdGrupoDestinatario = idGrupoDestinatario,
                    IdRemetente = usuarioLogado.Id,
                    IdDestinatario = usuario.Id,
                    Convite = usuarioLogado.NomeUsuario + " convidou você para ser seu contato.",
                    Email = usuario.Email,
                    ConviteContatos = true
                };
                
                dadosConvite.IncluirConviteGrupo(conviteGrupo);

                // Mensagem: Solicitação de amizade enviada.
                Toast.ShortMessage(Mensagem.MENS_FORM_29);
            }
        }

        public void AdicionarUsuarioAoGrupo(ConviteGrupo convite)
        {
            // Resumo: Adiciona dois usuários (rementente e destinatário) ao grupo contatos um do outro.

            usuarioGrupo = new UsuarioGrupo()
            {
                IdUsuario = convite.IdDestinatario,
                IdGrupo = convite.IdGrupoRemetente
            };

            if (convite.ConviteContatos)
            {
                usuario2Grupo = new UsuarioGrupo()
                {
                    IdUsuario = convite.IdRemetente,
                    IdGrupo = convite.IdGrupoDestinatario
                };
            }

            dadosUsuarioGrupo = new UsuarioGrupoRepository();

            try
            {
                dadosUsuarioGrupo.IncluirUsuarioGrupo(usuarioGrupo);

                if (convite.ConviteContatos)
                {
                    dadosUsuarioGrupo.IncluirUsuarioGrupo(usuario2Grupo);
                }

                // Mensagem: Você foi adicionado ao grupo.
                Toast.ShortMessage(Mensagem.MENS_FORM_25);
            }
            catch (SQLiteException ex)
            {
                // Mensagem: Erro ao incluir associação de usuário e grupo no banco de dados.
                Toast.ShortMessage(Mensagem.MENS_FORM_14);
            }
        }

        //---------------------------------------------------

        public bool ContatoNaoFoiConvidado(ObservableCollection<ConviteProjeto> listaConvites)
        {
            // Verifica se o contato que desejamos convidar já recebeu um convite

            IdContatoSelecionado = (int)Application.Current.Properties["idContato"];

            foreach (var convite in listaConvites)
            {
                if (IdContatoSelecionado == convite.IdDestinatario)
                {
                    // Mensagem: Usuário informado já foi convidado,\n por favor aguarde a resposta.
                    Toast.LongMessage(Mensagem.MENS_FORM_38);
                    return false;
                }
            }
            return true;
        }

        public bool ContatoJaParticipaDoGrupo()
        {
            IdContatoSelecionado = (int)Application.Current.Properties["idContato"];

            var membrosDoGrupo = ObterMembrosDoGrupo();

            foreach (var usuario in membrosDoGrupo)
            {
                if (IdContatoSelecionado == usuario.Id)
                {
                    // Mensagem: Este contato já é um membro do grupo.
                    Toast.ShortMessage(Mensagem.MENS_FORM_42);
                    return true;
                }
            }
            return false;
        }

        public ConviteGrupo CriarConviteGrupo()
        {
            // Resumo: Cria um convite de grupo para um contato sem vincular os dados do grupo ainda.

            int idUsuario = (int)Application.Current.Properties["id"];

            if (dadosUsuario == null)
            {
                dadosUsuario = new UsuarioRepository();
            }

            var usuarioLogado = dadosUsuario.ConsultarUsuarioPorId(idUsuario);

            var contato = dadosUsuario.ConsultarUsuarioPorId(IdContatoSelecionado);

            var conviteGrupo = new ConviteGrupo
            {
                IdRemetente = usuarioLogado.Id,
                IdDestinatario = contato.Id,
                NomeRemetente = usuarioLogado.NomeUsuario,
                NomeDestinatario = contato.NomeUsuario,
                ConviteContatos = false
            };

            // Mensagem: Convite enviado para o usuário.
            Toast.ShortMessage(Mensagem.MENS_FORM_19);
            return conviteGrupo;
        }

        public void EnviarConvitesGrupo(List<ConviteGrupo> convites, Grupo grupo)
        {
            // Resumo: Envia todos os convites adicionados a lista de convites para os respectivos destinatários.
            foreach (var convite in convites)
            {
                convite.IdGrupoRemetente = grupo.Id;
                convite.NomeGrupo = grupo.NomeGrupo;
                convite.Convite = $"{convite.NomeRemetente} convidou você para o grupo {grupo.NomeGrupo}.";
            }

            if (dadosConvite == null)
            {
                dadosConvite = new ConviteRepository();
            }
            dadosConvite.IncluirConvitesGrupo(convites);
        }

        public List<ConviteGrupo> ObterConvitesEnviadosDoGrupo()
        {
            // Resumo: Obtém todos os convites que foram enviados para participar do grupo selecionado.

            if (dadosConvite == null)
            {
                dadosConvite = new ConviteRepository();
            }

            return dadosConvite.ConsultarConvitesGrupoEnviados((int)Application.Current.Properties["idGrupo"]);
        }

    //---------------------------------------------------

        public void RemoverConvite(ConviteGrupo convite)
        {
            if (dadosConvite == null)
            {
                dadosConvite = new ConviteRepository();
            }

            dadosConvite.DeletarConviteGrupo(convite.IdGrupoRemetente, convite.IdDestinatario);
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

        public Grupo ObterUltimoGrupoInserido()
        {
            if (dadosGrupo == null)
            {
                dadosGrupo = new GrupoRepository();
            }
            return dadosGrupo.ConsultarGrupo(dadosGrupo.UltimoIdInserido());
        }

        public void SalvarIdGrupoSelecionado()
        {
            // Resumo: Identifica para a aplicação que agora vamos interagir com um grupo específico.

            IdGrupoSelecionado = (int)Application.Current.Properties["idGrupo"];
        }

        public int ObterIdGrupoSelecionado()
        {
            // Resumo: Devolve o identificador do grupo que estamos interagindo.

            return IdGrupoSelecionado;
        }

        public Grupo ObterGrupoSelecionado()
        {
            // Resumo: Permite consultar os dados do grupo selecionado.

            if (dadosGrupo == null)
            {
                dadosGrupo = new GrupoRepository();
            }
            grupo = dadosGrupo.ConsultarGrupo(IdGrupoSelecionado);
            return grupo;
        }

        public Usuario ObterCriadorGrupo()
        {
            // Resumo: Retorna o usuário criador do grupo selecionado.

            if (dadosGrupo == null)
            {
                dadosGrupo = new GrupoRepository();
            }

            IdCriadorServ = dadosGrupo.ConsultarGrupo(IdGrupoSelecionado).IdCriador;
            dadosUsuario = new UsuarioRepository();

            return dadosUsuario.ConsultarUsuarioPorId(IdCriadorServ);
        }

        public bool ContatoNaoFoiConvidado(ObservableCollection<ConviteGrupo> listaConvites)
        {
            // Verifica se o contato que desejamos convidar já recebeu um convite

            IdContatoSelecionado = (int)Application.Current.Properties["idContato"];

            foreach (var convite in listaConvites)
            {
                if (IdContatoSelecionado == convite.IdDestinatario)
                {
                    // Mensagem: Usuário informado já foi convidado,\n por favor aguarde a resposta.
                    Toast.LongMessage(Mensagem.MENS_FORM_38);
                    return false;
                }
            }
            return true;
        }

        public Grupo ObterGrupoContatosDoUsuarioLogado()
        {
            int idUsuario = (int)(Application.Current.Properties["id"]);

            if(dadosGrupo == null)
            {
                dadosGrupo = new GrupoRepository();
            }
            return dadosGrupo.ConsultarGrupoContatos(idUsuario);
        }

        public Grupo ObterGrupoContatosDoUsuario(int idUsuario)
        {
            if (dadosGrupo == null)
            {
                dadosGrupo = new GrupoRepository();
            }
            return dadosGrupo.ConsultarGrupoContatos(idUsuario);
        }

        public List<Usuario> ObterMembrosDoGrupo()
        {
            if (dadosUsuarioGrupo == null)
            {
                dadosUsuarioGrupo = new UsuarioGrupoRepository();
            }

            return dadosUsuarioGrupo.ConsultarUsuariosDoGrupo(IdGrupoSelecionado);
        }

        public List<Usuario> ObterMembrosDoGrupoContatos()
        {
            if(dadosUsuarioGrupo == null)
            {
                dadosUsuarioGrupo = new UsuarioGrupoRepository();
            }

            return dadosUsuarioGrupo.ConsultarUsuariosDoGrupo(ObterGrupoContatosDoUsuarioLogado().Id);
        }

        public List<Grupo> ObterGruposDoUsuarioLogado()
        {
            // Resumo: Obtém todos os grupos que o usuário logado criou ou participa.

            int idUsuario = (int)(Application.Current.Properties["id"]);

            if (dadosUsuarioGrupo == null)
            {
                dadosUsuarioGrupo = new UsuarioGrupoRepository();
            }
            var dados = dadosUsuarioGrupo.ConsultarGruposDoUsuario(idUsuario); // melhorar este retorno.

            if (dados.Count == 0)
            {
                // Mensagem: Você ainda não criou ou participa de nenhum grupo.
                Toast.ShortMessage(Mensagem.MENS_FORM_22);
            }

            return dados;
        }

        public List<Usuario> ObterUsuariosDoGrupoSelecionado()
        {
            // Resumo: Obtém todos os membros de um grupo.

            int idGrupo = (int)(Application.Current.Properties["idGrupo"]);

            if (dadosUsuarioGrupo == null)
            {
                dadosUsuarioGrupo = new UsuarioGrupoRepository();
            }

            var dados = dadosUsuarioGrupo.ConsultarUsuariosDoGrupo(idGrupo); // melhorar este retorno.

            if (dados.Count == 0)
            {
                // Mensagem: Você ainda não criou ou participa de nenhum grupo.
                Toast.ShortMessage(Mensagem.MENS_FORM_22);
            }

            return dados;
        }

        public void AlterarGrupo(Grupo grupo)
        {
            // Resumo: Permite alterar dados de um grupo.

            if (dadosGrupo == null)
            {
                dadosGrupo = new GrupoRepository();
            }
            try
            {
                dadosGrupo.AlterarGrupo(grupo);
                // Mensagem: Grupo alterado com sucesso.
                Toast.ShortMessage(Mensagem.MENS_FORM_31);
            }
            catch (SQLiteException ex)
            {
                // Mensagem: Erro ao tentar alterar grupo no banco de dados.
                Toast.ShortMessage(Mensagem.MENS_FORM_15);
            }
        }

        public void ExcluirGrupoSelecionado()
        {
            // Resumo: Permite excluir o projeto selecionado.

            if (dadosGrupo == null)
            {
                dadosGrupo = new GrupoRepository();
            }
            dadosGrupo.DeletarGrupo(IdGrupoSelecionado);

            if (dadosConvite == null)
            {
                dadosConvite = new ConviteRepository();
            }
            dadosConvite.DeletarConvitesGrupo(IdGrupoSelecionado);
        }

        public void ExcluirContatoSelecionado()
        {
            // Resumo: Permite excluir o projeto selecionado.

            if (dadosUsuarioGrupo == null)
            {
                dadosUsuarioGrupo = new UsuarioGrupoRepository();
            }
            dadosUsuarioGrupo.DeletarUsuarioDoGrupo(IdContatoSelecionado,ObterGrupoContatosDoUsuarioLogado().Id);
        }

        public int ObterIdContatoSelecionado()
        {
            // Resumo: Devolve o identificador do contato que estamos interagindo.

            return IdContatoSelecionado;
        }

        public void SalvarIdContatoSelecionado()
        {
            // Resumo: Identifica para a aplicação que agora vamos interagir com um contato específico.

            IdContatoSelecionado = (int)Application.Current.Properties["idContato"];
        }

        public bool ContatoJaColaboraNoProjeto()
        {
            IdContatoSelecionado = (int)Application.Current.Properties["idContato"];

            var membrosDoGrupo = ObterMembrosDoGrupo();

            foreach (var usuario in membrosDoGrupo)
            {
                if (IdContatoSelecionado == usuario.Id)
                {
                    // Mensagem: Este contato já é um membro do grupo.
                    Toast.ShortMessage(Mensagem.MENS_FORM_42);
                    return true;
                }
            }
            return false;
        }

        public void RemoverUsuarioDoGrupo()
        {
            // Resumo: Remove o usuário selecionado de um grupo.

            if (dadosUsuarioGrupo == null)
            {
                dadosUsuarioGrupo = new UsuarioGrupoRepository();
            }

            IdMembroSelecionado = (int) Application.Current.Properties["idMembro"];

            dadosUsuarioGrupo.DeletarUsuarioDoGrupo(IdMembroSelecionado, IdGrupoSelecionado);
        }
    }
}
