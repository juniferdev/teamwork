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
    public class UsuarioTarefaRepository
    {
        private SQLiteConnection conexao;
        private UsuarioRepository dadosUsuario;

        public UsuarioTarefaRepository()
        {
            var config = DependencyService.Get<ISQLiteConfig>();
            conexao = new SQLiteConnection(config.Plataforma, Path.Combine(config.DiretorioSQLite, "TeamWork.db3"));
            conexao.CreateTable<Tarefa>();
            conexao.CreateTable<UsuarioTarefa>();
        }

        public void IncluirUsuarioTarefa(UsuarioTarefa usuarioTarefa)
        {
            conexao.Insert(usuarioTarefa);
        }
        
        public List<Tarefa> ConsultarTarefasDoUsuario(int idUsuario)
        {
            return conexao.Query<Tarefa>("SELECT * FROM Tarefa WHERE IdCriador = ? OR IdResponsavel = ?", idUsuario, idUsuario);
        }

        public List<Usuario> ConsultarResponsavelDaTarefa(int idTarefa)
        {
            var idUsuarios = conexao.Query<UsuarioTarefa>("SELECT * FROM UsuarioTarefa WHERE IdTarefa = ?", idTarefa);
            var usuarios = conexao.Query<Usuario>("SELECT * FROM Usuario");
            List<Usuario> usuarioResponsavel = new List<Usuario>();
            foreach(var usuario in usuarios)
            {
                foreach(var id in idUsuarios)
                {
                    if (usuario.Id == id.IdUsuario)
                    {
                        usuarioResponsavel.Add(usuario);
                    }
                }
            }
            return usuarioResponsavel;
        }

        public bool UsuarioResponsavelPelaTarefa(int idUsuario)
        {
            Usuario usuario = conexao.FindWithQuery<Usuario>("SELECT * FROM UsuarioTarefa WHERE IdUsuario = ?", idUsuario);
            return usuario == null ? false : true;
        }
    }
}