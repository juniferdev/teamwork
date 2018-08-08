using SQLite.Net;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TeamWork.Interface;
using TeamWork.Model;
using Xamarin.Forms;

namespace TeamWork.Repository
{
    public class ConviteRepository
    {
        private SQLiteConnection conexao;

        public ConviteRepository()
        {
            var config = DependencyService.Get<ISQLiteConfig>();
            conexao = new SQLiteConnection(config.Plataforma, Path.Combine(config.DiretorioSQLite, "TeamWork.db3"));
            conexao.CreateTable<Usuario>();
            conexao.CreateTable<Grupo>();
            conexao.CreateTable<Projeto>();
            conexao.CreateTable<ConviteGrupo>();
            conexao.CreateTable<ConviteProjeto>();
        }

        public void IncluirConviteProjeto(ConviteProjeto convite)
        {
            conexao.Query<ConviteProjeto>("INSERT INTO ConviteProjeto (IdProjeto, IdDestinatario, IdRemetente, NomeDestinatario, NomeRemetente, Convite) VALUES (?,?,?,?,?,?)",
            convite.IdProjeto, convite.IdDestinatario, convite.IdRemetente, convite.NomeDestinatario, convite.NomeRemetente, convite.Convite );
        }

        public void IncluirConvitesProjeto(List<ConviteProjeto> convites)
        {
            foreach(var convite in convites)
            {
                conexao.Query<ConviteProjeto>("INSERT INTO ConviteProjeto (IdProjeto, IdDestinatario, IdRemetente, NomeProjeto, NomeDestinatario, NomeRemetente, Convite) VALUES (?,?,?,?,?,?,?)",
                convite.IdProjeto, convite.IdDestinatario, convite.IdRemetente, convite.NomeProjeto, convite.NomeDestinatario, convite.NomeRemetente, convite.Convite);
            }
        }

        public void DeletarConviteProjeto(int idProjeto, int idUsuario)
        {
            conexao.Query<ConviteProjeto>("DELETE FROM ConviteProjeto WHERE IdProjeto = ? AND IdDestinatario = ?", idProjeto, idUsuario);
        }

        public void DeletarConvitesProjeto(int idProjeto)
        {
            conexao.Query<ConviteProjeto>("DELETE FROM ConviteProjeto WHERE IdProjeto = ?", idProjeto);
        }

        public List<ConviteProjeto> ConsultarConvitesProjetoEnviados(int idProjeto)
        {
            return conexao.Query<ConviteProjeto>("SELECT * FROM ConviteProjeto WHERE IdProjeto = ?", idProjeto);
        }

        public void IncluirConviteGrupo(ConviteGrupo convite)
        {
            conexao.Query<ConviteGrupo>("INSERT INTO ConviteGrupo (IdGrupoRemetente, IdGrupoDestinatario, NomeRemetente, NomeDestinatario, IdDestinatario, Email, Convite, IdRemetente, ConviteContatos) VALUES (?,?,?,?,?,?,?,?,?)", convite.IdGrupoRemetente, convite.IdGrupoDestinatario, convite.NomeRemetente , convite.NomeDestinatario, convite.IdDestinatario, convite.Email, convite.Convite, convite.IdRemetente, convite.ConviteContatos);
        }

        public void IncluirConvitesGrupo(List<ConviteGrupo> convites)
        {
            foreach (var convite in convites)
            {
                conexao.Query<ConviteGrupo>("INSERT INTO ConviteGrupo (IdGrupoRemetente, IdDestinatario, IdRemetente, NomeGrupo, NomeRemetente, NomeDestinatario, Convite, ConviteContatos) VALUES (?,?,?,?,?,?,?,?)",
                convite.IdGrupoRemetente, convite.IdDestinatario, convite.IdRemetente, convite.NomeGrupo, convite.NomeRemetente, convite.NomeDestinatario, convite.Convite, convite.ConviteContatos);
            }
        }

        public void DeletarConviteGrupo(int idGrupo, int idUsuario)
        {
            conexao.Query<ConviteGrupo>("DELETE FROM ConviteGrupo WHERE IdGrupoRemetente = ? AND IdDestinatario = ?",idGrupo, idUsuario);
        }

        public void DeletarConvitesGrupo(int idGrupo)
        {
            conexao.Query<ConviteGrupo>("DELETE FROM ConviteGrupo WHERE IdGrupoRemetente = ?", idGrupo);
        }

        public List<ConviteGrupo> ConsultarConvitesGrupoEnviados(int idGrupo)
        {
            return conexao.Query<ConviteGrupo>("SELECT * FROM ConviteGrupo WHERE IdGrupoRemetente = ?", idGrupo);
        }

        public ConviteLista ConsultarConvitesEnviadosDoUsuario(int idUsuario)
        {
            List<ConviteGrupo> convitesGrupo = conexao.Query<ConviteGrupo>("SELECT * FROM ConviteGrupo WHERE IdRemetente = ?", idUsuario);

            ConviteLista convites = new ConviteLista();

            if (convitesGrupo != null)
            {
                convites.ConvitesParaGrupos = convitesGrupo;
            }
            return convites;
        }

        public ConviteLista ConsultarConvitesDoUsuario(int idUsuario)
        {
            List<ConviteProjeto> convitesProjeto = conexao.Query<ConviteProjeto>("SELECT * FROM ConviteProjeto WHERE IdDestinatario = ?", idUsuario);
            List<ConviteGrupo> convitesGrupo = conexao.Query<ConviteGrupo>("SELECT * FROM ConviteGrupo WHERE IdDestinatario = ?", idUsuario).ToList();

            ConviteLista convites = new ConviteLista();

            if (convitesProjeto != null)
            {
                convites.ConvitesParaProjetos = convitesProjeto;
            }

            if (convitesGrupo != null)
            {
                convites.ConvitesParaGrupos = convitesGrupo;
            }
            return convites;
        }

        public void LimparTabela()
        {
            conexao.DropTable<ConviteGrupo>();
            conexao.CreateTable<ConviteGrupo>();
            conexao.DropTable<ConviteProjeto>();
            conexao.CreateTable<ConviteProjeto>();
        }

    }
}
