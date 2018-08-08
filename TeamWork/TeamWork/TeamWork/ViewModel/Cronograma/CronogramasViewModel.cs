using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TeamWork.Internal;
using TeamWork.Repository;
using TeamWork.Service;
using Xamarin.Forms;

namespace TeamWork.ViewModel.Cronograma
{
    public class CronogramasViewModel : BaseViewModel
    {
        private readonly TarefaService servicoTarefa;

        public Command NoPrazoCommand { get; set; }
        public Command AtrasadaCommand { get; set; }
        public Command FinalizadaCommand { get; set; }

        public CronogramasViewModel()
        {
            NoPrazoCommand = new Command(SinalizarNoPrazo);
            AtrasadaCommand = new Command(SinalizarAtrasada);
            FinalizadaCommand = new Command(SinalizarFinalizada);
            servicoTarefa = new TarefaService();
            Tarefas = new ObservableCollection<Model.Tarefa>(ListaTarefas);
        }

        public List<Model.Tarefa> ListaTarefas => servicoTarefa.ObterTarefasDoUsuarioLogado();

        public ObservableCollection<Model.Tarefa> Tarefas { get; set; }
     
        public void SinalizarNoPrazo()
        {
            Toast.ShortMessage(Mensagem.MENS_FORM_48);
        }

        public void SinalizarAtrasada()
        {
            Toast.ShortMessage(Mensagem.MENS_FORM_49);
        }

        public void SinalizarFinalizada()
        {
            Toast.ShortMessage(Mensagem.MENS_FORM_50);
        }

    }
}
