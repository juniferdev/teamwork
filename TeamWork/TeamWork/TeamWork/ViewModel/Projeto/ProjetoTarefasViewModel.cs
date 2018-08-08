using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TeamWork.Service;

namespace TeamWork.ViewModel.Projeto
{
    public class ProjetoTarefasViewModel: BaseViewModel
    {
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

        public ProjetoService servicoProjeto;

        public ProjetoTarefasViewModel()
        {
            servicoProjeto = new ProjetoService();
            Tarefas = ListarTarefas();
        }

        public ObservableCollection<Model.Tarefa> ListarTarefas()
        {
            Tarefas = new ObservableCollection<Model.Tarefa>(servicoProjeto.ObterTarefasDoProjeto());
            return Tarefas;
        }
    }
}
