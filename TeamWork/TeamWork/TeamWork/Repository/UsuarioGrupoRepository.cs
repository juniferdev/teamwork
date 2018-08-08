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
    public class UsuarioGrupoRepository
    {
        private SQLiteConnection conexao;
        public UsuarioGrupoRepository()
        {
            var config = DependencyService.Get<ISQLiteConfig>();
            conexao = new SQLiteConnection(config.Plataforma, Path.Combine(config.DiretorioSQLite, "TeamWork.db3"));
            conexao.CreateTable<Usuario>();
            conexao.CreateTable<Grupo>();
            conexao.CreateTable<UsuarioGrupo>();
        }

        public void IncluirUsuarioGrupo(UsuarioGrupo usuarioGrupo)
        {
            conexao.Insert(usuarioGrupo);
        }

        public List<Grupo> ConsultarGruposDoUsuario(int idUsuario)
        {
            // Consulta todos os grupos do usuário 
            // que foram criados por ele ou nos quais ele participa (exceto grupo de contatos);
            List<Grupo> gruposDoUsuario = new List<Grupo>();
            List<Grupo> gruposQueParticipo = new List<Grupo>();
            List<Grupo> gruposCriados = conexao.Query<Grupo>("select * from Grupo where IdCriador = ? and Contatos = ?", idUsuario, false);
            var usuarioGrupo = conexao.Query<UsuarioGrupo>("select * from UsuarioGrupo where IdUsuario = ?",idUsuario);
            foreach(var ug in usuarioGrupo)
            {
                gruposQueParticipo.Add(conexao.FindWithQuery<Grupo>("select * from Grupo where Id = ?",ug.IdGrupo));
            }

            gruposQueParticipo = gruposQueParticipo.FindAll(g => g.Contatos == false);

            foreach (var grupo in gruposQueParticipo)
            {
                gruposDoUsuario.Add(grupo);
            }
            foreach(var grupo in gruposCriados)
            {
                if(!gruposDoUsuario.Exists(g => g.Id == grupo.Id))
                {
                    gruposDoUsuario.Add(grupo);
                }
            }
            return gruposDoUsuario;
        }

        public List<Usuario> ConsultarUsuariosDoGrupo(int idGrupo)
        {
            var idUsuarios = conexao.Query<UsuarioGrupo>("SELECT * FROM UsuarioGrupo WHERE IdGrupo = ?", idGrupo);
            var usuarios = conexao.Query<Usuario>("SELECT * FROM Usuario");
            List<Usuario> usuariosDoGrupo = new List<Usuario>();
            foreach (var usuario in usuarios)
            {
                foreach (var id in idUsuarios)
                {
                    if (usuario.Id == id.IdUsuario)
                    {
                        usuariosDoGrupo.Add(usuario);
                    }
                }
            }
            return usuariosDoGrupo;
        }

        public void DeletarUsuarioDoGrupo(int idUsuario, int idGrupo)
        {
            conexao.Query<UsuarioGrupo>("DELETE FROM UsuarioGrupo WHERE IdUsuario = ? AND IdGrupo = ?", idUsuario, idGrupo);
        }

        public void LimparTabela()
        {
            conexao.DropTable<UsuarioGrupo>();
            conexao.CreateTable<UsuarioGrupo>();
        }
    }
}
