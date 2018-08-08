using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;
using TeamWork.Model;
using TeamWork.View.Projeto;
using TeamWork.Service;

namespace TeamWork.ViewModel.Projeto
{

    public class ProjetosViewModel: BaseViewModel
    {
        public Command CriarProjetoCommand { get; set; }
        public Command ProjetoDetalhesCommand { get; set; }

        public ObservableCollection<Model.Projeto> projetos;
        public ObservableCollection<Model.Projeto> Projetos
        {
            get { return projetos; }
            set { projetos = value; OnPropertyChanged(nameof(Projetos)); }
        }
        
        public ProjetoService servicoProjeto;

        public ProjetosViewModel()
        {
            Projetos = ListarProjetos();
            CriarProjetoCommand = new Command(ChamarCriarProjetoView);
            ProjetoDetalhesCommand = new Command(ChamarProjetoDetalhesView);
        }

        public ObservableCollection<Model.Projeto> ListarProjetos()
        {
            if(servicoProjeto == null)
            {
                servicoProjeto = new ProjetoService();
            }
            return new ObservableCollection<Model.Projeto>(servicoProjeto.ObterProjetosDoUsuarioLogado());
        }

        public void ChamarCriarProjetoView()
        {
            App.Current.MainPage.Navigation.PushAsync(new CriarProjetoView() { BarBackgroundColor = Color.FromHex("#3b5998") }, true);
        }

        public void ChamarProjetoDetalhesView()
        {
            App.Current.MainPage.Navigation.PushAsync(new ProjetoDetalhesView() { BarBackgroundColor = Color.FromHex("#3b5998") }, true);
        }

    }
}
