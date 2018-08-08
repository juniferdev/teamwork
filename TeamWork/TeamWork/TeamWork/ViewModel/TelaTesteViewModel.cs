using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TeamWork.Model;
using TeamWork.Service;

namespace TeamWork.ViewModel
{
    public class TelaTesteViewModel: BaseViewModel
    {
        public ObservableCollection<Model.Projeto> projetos { get; set; }
        public ObservableCollection<Model.Projeto> Projetos { get { return projetos; } set { projetos = value; OnPropertyChanged(nameof(Projetos)); } }

        public string projetoSelecionado { get; set; }
        public string ProjetoSelecionado { get { return projetoSelecionado; } set { projetoSelecionado = value; OnPropertyChanged(nameof(ProjetoSelecionado)); } }

        public ProjetoService servicoProjeto;

        public TelaTesteViewModel()
        {
            servicoProjeto = new ProjetoService();
            Projetos = new ObservableCollection<Model.Projeto>(servicoProjeto.ObterProjetosDoUsuarioLogado());
           //ProjetoSelecionado = servicoProjeto.ObterProjetoSelecionado().Id;
        }
    }
}
