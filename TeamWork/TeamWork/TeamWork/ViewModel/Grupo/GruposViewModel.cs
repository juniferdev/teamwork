using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using TeamWork.Model;
using TeamWork.Service;
using TeamWork.View.Grupo;
using Xamarin.Forms;

namespace TeamWork.ViewModel.Grupo
{
    public class GruposViewModel: BaseViewModel
    {
        public ObservableCollection<Model.Grupo> grupos;
        public ObservableCollection<Model.Grupo> Grupos
        {
            get { return grupos; }
            set { grupos = value; OnPropertyChanged(nameof(Grupos)); }
        }
        public ICommand CriarGrupoCommand { get; set; }
        public ICommand GrupoDetalhesCommand { get; set; }

        public GrupoService servicoGrupo;

        public GruposViewModel()
        {
            Grupos = ListarGrupos();
            CriarGrupoCommand = new Command(ChamarCriarGrupoView);
            GrupoDetalhesCommand = new Command(ChamarGrupoDetalhesView);
        }

        public ObservableCollection<Model.Grupo> ListarGrupos()
        {
            if(servicoGrupo == null)
            {
                servicoGrupo = new GrupoService();
            }
            return new ObservableCollection<Model.Grupo>(servicoGrupo.ObterGruposDoUsuarioLogado());
        }

        public void ChamarCriarGrupoView()
        {
            App.Current.MainPage.Navigation.PushAsync(new CriarGrupoView() , true);
        }

        public void ChamarGrupoDetalhesView()
        {
            App.Current.MainPage.Navigation.PushAsync(new GrupoDetalhesView() ,true);
        }
    }
}