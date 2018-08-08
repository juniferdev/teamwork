using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using TeamWork.Model;
using TeamWork.Service;
using TeamWork.View.Tarefa;
using Xamarin.Forms;

namespace TeamWork.ViewModel.Tarefa
{
    public class TarefasViewModel : BaseViewModel
    {
        public Command CriarTarefaCommand { get; set; }
        public Command EditarTarefaCommand { get; set; }
        public ObservableCollection<Model.Tarefa> tarefas;
        public ObservableCollection<Model.Tarefa> Tarefas
        {
            get { return tarefas; }
            set
            {
                tarefas = value;
                OnPropertyChanged(nameof(Tarefas));
            }
        }

        public TarefasViewModel()
        {
            CriarTarefaCommand = new Command(ChamarCriarTarefaView);
            EditarTarefaCommand = new Command(ChamarEditarTarefaView);
            Tarefas = ListarTarefas();
        }

        public TarefaService servicoTarefa;

        public ObservableCollection<Model.Tarefa> ListarTarefas()
        {
            servicoTarefa = new TarefaService();
            Tarefas = new ObservableCollection<Model.Tarefa>(servicoTarefa.ObterTarefasDoUsuarioLogado());
            return Tarefas;
        }

        public void ChamarCriarTarefaView()
        {
            // Verificar se não é possível deixar uma única NavigationPage com um voltar
            App.Current.MainPage.Navigation.PushAsync(new CriarTarefaView() { BarBackgroundColor = Color.FromHex("#3b5998") }, true);
        }

        public void ChamarEditarTarefaView()
        {
            App.Current.MainPage.Navigation.PushAsync(new TarefaDetalhesView() { BarBackgroundColor = Color.FromHex("#3b5998") }, true);
        }

        public async void ExcluirTarefa()
        {
            var resposta = await Application.Current.MainPage.DisplayAlert("Exclusão de Tarefa", "Tem certeza que deseja" + Environment.NewLine + "excluir esta tarefa? ", "Sim", "Não");
            if (resposta)
            {
                servicoTarefa.SalvarIdTarefaSelecionada();
                servicoTarefa.ExcluirTarefaSelecionada();
            }
            Tarefas = ListarTarefas();
        }


    }
}
