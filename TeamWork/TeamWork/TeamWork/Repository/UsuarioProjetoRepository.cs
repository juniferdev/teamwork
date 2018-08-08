using SQLite.Net;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TeamWork.Interface;
using TeamWork.Internal;
using TeamWork.Model;
using Xamarin.Forms;

namespace TeamWork.Repository
{
    public class UsuarioProjetoRepository
    {
        private SQLiteConnection conexao;
        private UsuarioRepository dadosUsuario;

        public UsuarioProjetoRepository()
        {
            var config = DependencyService.Get<ISQLiteConfig>();
            conexao = new SQLiteConnection(config.Plataforma, Path.Combine(config.DiretorioSQLite, "TeamWork.db3"));
            conexao.CreateTable<Projeto>();
            conexao.CreateTable<UsuarioProjeto>();
        }

        public void IncluirUsuarioProjeto(UsuarioProjeto usuarioProjeto)
        {
            conexao.Insert(usuarioProjeto);
        }

        public void DeletarUsuarioProjeto(int idUsuario, int idProjeto)
        {
            conexao.Query<UsuarioProjeto>("DELETE FROM UsuarioProjeto WHERE IdUsuario = ? AND IdProjeto = ?", idUsuario, idProjeto);
        }

        public List<Projeto> ConsultarProjetosDoUsuario(int idUsuario)
        {
            var idProjetos = conexao.Query<UsuarioProjeto>("SELECT * FROM UsuarioProjeto WHERE IdUsuario = ?",idUsuario);
            var projetos = conexao.Query<Projeto>("SELECT * FROM Projeto");
            List<Projeto> projetosDoUsuario = new List<Projeto>();
            foreach (var projeto in projetos)
            {
                foreach(var id in idProjetos)
                {
                    if(projeto.Id == id.IdProjeto)
                    {
                        projetosDoUsuario.Add(projeto);
                    }
                }
            }
            return projetosDoUsuario;
        }

        public List<Usuario> ConsultarUsuariosDoProjeto(int idProjeto)
        {
            var idUsuarios = conexao.Query<UsuarioProjeto>("SELECT * FROM UsuarioProjeto WHERE IdProjeto = ?", idProjeto);
            var usuarios = conexao.Query<Usuario>("SELECT * FROM Usuario");
            List<Usuario> usuariosDoProjeto = new List<Usuario>();
            foreach (var usuario in usuarios)
            {
                foreach (var id in idUsuarios)
                {
                    if (usuario.Id == id.IdUsuario)
                    {
                        usuariosDoProjeto.Add(usuario);
                    }
                }
            }
            return usuariosDoProjeto.ToList();
        }

        public bool UsuarioJaParticipaDeProjeto(int idUsuario)
        {
            Usuario usuario = conexao.FindWithQuery<Usuario>("SELECT * FROM UsuarioProjeto WHERE IdUsuario = ?",idUsuario);
            return usuario == null ? false : true;
        }

        public void LimparTabela()
        {
            conexao.DropTable<UsuarioProjeto>();
            conexao.CreateTable<UsuarioProjeto>();
        }
    }
}
