using SQLite.Net;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using TeamWork.Interface;
using TeamWork.Model;
using Xamarin.Forms;

namespace TeamWork.Repository
{
    public class UsuarioRepository: IDisposable
    {
        private SQLiteConnection conexao;

        public UsuarioRepository()
        {
            var config = DependencyService.Get<ISQLiteConfig>();
            conexao = new SQLiteConnection(config.Plataforma,Path.Combine(config.DiretorioSQLite, "TeamWork.db3"));
            conexao.CreateTable<Usuario>();
        }

        public void IncluirUsuario(Usuario usuario)
        {
            conexao.Insert(usuario);
        }

        public void ExcluirUsuario(Usuario usuario)
        {
            conexao.Delete(usuario);
        }

        public void AlterarUsuario(int idUsuario, Usuario usuario)
        {
            conexao.Query<Usuario>("UPDATE Usuario SET NomeUsuario = ?, Email = ?, Senha = ? WHERE Id = ?",
            usuario.NomeUsuario, usuario.Email, usuario.Senha, idUsuario);
        }

        public Usuario ConsultarUsuarioPorId(int idUsuario)
        {
            return conexao.Table<Usuario>().FirstOrDefault(campo => campo.Id == idUsuario);
        }

        public Usuario AutenticarContaDeUsuario(string email, string senha)
        {
           return conexao.Table<Usuario>().FirstOrDefault(campo => campo.Email == email && campo.Senha == senha);  
        }

        public Usuario ConsultarUsuarioPorEmail(string email)
        {
            //return conexao.Table<Usuario>().FirstOrDefault(campo => campo.Email == email);
            return conexao.FindWithQuery<Usuario>("SELECT * FROM Usuario WHERE Email = ?", email);
        }

        // O método abaixo deverá ser removido posteriormente
        public List<Usuario> ConsultarUsuarios()
        {
            return conexao.Table<Usuario>().OrderBy(campo => campo.NomeUsuario).ToList();
        }

        public int UltimoIdInserido()
        {
            return conexao.Table<Usuario>().LastOrDefault().Id;
        }

        public void LimparTabela()
        {
            conexao.DropTable<Usuario>();
            conexao.CreateTable<Usuario>();
        }

        public void Dispose()
        {
            conexao.Dispose();
        }
    }
}
