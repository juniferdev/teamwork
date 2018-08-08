using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TeamWork.Model;
using TeamWork.Repository;
using TeamWork.Service;
using Xamarin.Forms;

namespace TeamWork.ViewModel
{
    public class UsuariosViewModel: INotifyPropertyChanged
    {

        public ObservableCollection<Model.ConviteProjeto> conviteProjetos;
        public ObservableCollection<Model.ConviteProjeto> ConviteProjetos
        {
            get
            {
                return conviteProjetos;
            }
            set
            {
                conviteProjetos = value;
                OnPropertyChanged(nameof(ConviteProjetos));
            }
        }

        public Command ApagarUsuarioCommand { get; set; }
        public Command ApagarProjetoCommand { get; set; }
        public Command ApagarTarefaCommand { get; set; }
        public Command ApagarGrupoCommand { get; set; }
        public Command ApagarUsuarioProjetoCommand { get; set; }
        public Command ApagarUsuarioGrupoCommand { get; set; }
        public Command ApagarUsuarioTarefaCommand { get; set; }
        public Command ApagarConviteCommand { get; set; }

        public Command VoltarCommand { get; set; }

        private ContaService servicoConta;

        public event PropertyChangedEventHandler PropertyChanged;

        public ProjetoRepository pdados;
        public UsuarioRepository udados;
        public UsuarioProjetoRepository updados;
        public GrupoRepository gdados;
        public ConviteRepository cdados;
        public UsuarioGrupoRepository ugdados;

        public UsuariosViewModel()
        {
            ApagarUsuarioCommand = new Command(ApagarDadosUsuario);
            ApagarProjetoCommand = new Command(ApagarDadosProjeto);
            ApagarTarefaCommand = new Command(ApagarDadosTarefa);
            ApagarGrupoCommand = new Command(ApagarDadosGrupo);
            ApagarUsuarioProjetoCommand = new Command(ApagarDadosUsuarioProjeto);
            ApagarUsuarioGrupoCommand = new Command(ApagarDadosUsuarioGrupo);
            ApagarUsuarioTarefaCommand = new Command(ApagarDadosUsuarioTarefa);
            ApagarConviteCommand = new Command(ApagarDadosConvite);

            pdados = new ProjetoRepository();
            updados = new UsuarioProjetoRepository();
            gdados = new GrupoRepository();
            udados = new UsuarioRepository();
            ugdados = new UsuarioGrupoRepository();
            cdados = new ConviteRepository();    
        }

        public void ApagarDadosUsuario()
        {
            udados.LimparTabela();
            cdados.LimparTabela();
        }
        public void ApagarDadosProjeto()
        {
            pdados.LimparTabela();
            cdados.LimparTabela();
        }
        public void ApagarDadosTarefa()
        {
            // Limpar tabela tarefa nesta linha.
            cdados.LimparTabela();
        }
        public void ApagarDadosGrupo()
        {
            gdados.LimparTabela();
            cdados.LimparTabela();
        }
        public void ApagarDadosUsuarioProjeto()
        {
            updados.LimparTabela();
        }
        public void ApagarDadosUsuarioGrupo()
        {
            ugdados.LimparTabela();
        }
        public void ApagarDadosUsuarioTarefa()
        {

        }
        public void ApagarDadosConvite()
        {
            cdados.LimparTabela();
        }

        public void VoltarPagina()
        {
            Application.Current.MainPage.Navigation.PopModalAsync();
        }

        private void OnPropertyChanged(string propertyName)
        {
            var changed = PropertyChanged;
            if (changed != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}
