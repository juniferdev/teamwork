using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TeamWork.View.Grupo;
using TeamWork.View.Projeto;
using TeamWork.View.Tarefa;
using TeamWork.View.Cronograma;
using Xamarin.Forms;
using TeamWork.Model;
using TeamWork.Service;
using TeamWork.View.Conta;
using TeamWork.View;
using TeamWork.ViewModel.Tarefa;
using System.Collections.ObjectModel;
using TeamWork.Internal;

namespace TeamWork.ViewModel
{
    public class PrincipalViewModel: BaseViewModel
    {
        public Command ContatosCommand { get; set; }
        public Command EditarContaCommand { get; set; }
        public Command SairCommand { get; set; }
        public Command ProjetosCommand { get; set; }
        public Command TarefasCommand { get; set; }
        public Command EquipesCommand { get; set; }
        public Command CronogramasCommand { get; set; }
        public Command NotificacoesCommand { get; set; }

        private string nomeUsuario;
        public string NomeUsuario
        {
            get
            {
                return nomeUsuario;
            }
            set
            {
                nomeUsuario = value;
                OnPropertyChanged(nameof(NomeUsuario));
            }
        }

        public ContaService servicoConta;
        public Model.Tarefa tarefaTeste;
        public TarefaService servicoTarefa;
        public ObservableCollection<Model.Tarefa> tarefas;
        public ObservableCollection<Model.Tarefa> Tarefas { get { return tarefas; } set { tarefas = value; OnPropertyChanged(nameof(Tarefas)); } }

        public PrincipalViewModel()
        {
            ContatosCommand = new Command(ChamarContatosView);
            EditarContaCommand = new Command(ChamarEditarContaView);
            SairCommand = new Command(SairDaConta);
            ProjetosCommand = new Command(ChamarProjetosView);
            TarefasCommand = new Command(ChamarTarefasView);
            EquipesCommand = new Command(ChamarGruposView);
            CronogramasCommand = new Command(ChamarCronogramasView);
            NotificacoesCommand = new Command(ChamarNotificacoesView);
            DadosAutomaticos();
        }

        private void DadosAutomaticos()
        {
            servicoConta = new ContaService();
            servicoTarefa = new TarefaService();
            Tarefas = new ObservableCollection<Model.Tarefa>(servicoTarefa.ObterTarefasDoUsuarioLogado(false).Where(c => c.Situacao == "Atrasada"));
            NomeUsuario = "Bem vindo, " + servicoConta.ObterUsuarioPorIdLogado().NomeUsuario;
        }

        private void SairDaConta()
        {
            Application.Current.Properties["id"] = 0;
            Application.Current.Properties["idProjeto"] = 0;
            Application.Current.Properties["idTarefa"] = 0;
            Application.Current.Properties["idGrupo"] = 0;
            Application.Current.Properties["idContato"] = 0;
            Application.Current.Properties["idMembro"] = 0;
            Application.Current.MainPage = new EntrarView();
        }

        private void ChamarNotificacoesView()
        {
            Application.Current.MainPage.Navigation.PushModalAsync(new NavigationPage(new ConvitesView()) { BarBackgroundColor = Color.FromHex("#3b5998") }, true);
        }

        private void ChamarContatosView()
        {
            Application.Current.MainPage.Navigation.PushModalAsync(new NavigationPage(new ContatosView()) { BarBackgroundColor = Color.FromHex("#3b5998") }, true);
        }

        private void ChamarEditarContaView()
        {
            Application.Current.MainPage.Navigation.PushModalAsync(new NavigationPage(new EditarContaView()) { BarBackgroundColor = Color.FromHex("#3b5998") }, true);
        }

        private void ChamarProjetosView()
        {
            Application.Current.MainPage.Navigation.PushAsync(new ProjetosView(), true);
        }

        private void ChamarTarefasView()
        {
            Application.Current.MainPage.Navigation.PushAsync(new TarefasView(), true);
        }

        private void ChamarGruposView()
        {
            Application.Current.MainPage.Navigation.PushAsync(new GruposView(), true);
        }

        private void ChamarCronogramasView()
        {
            Application.Current.MainPage.Navigation.PushAsync(new CronogramasView(), true);
        }

        public void ChamarEditarTarefaView()
        {
            App.Current.MainPage.Navigation.PushAsync(new TarefaDetalhesView() { BarBackgroundColor = Color.FromHex("#3b5998") }, true);
        }
    }
}
