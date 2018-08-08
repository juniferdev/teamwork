using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TeamWork.Model;
using TeamWork.Service;

namespace TeamWork.ViewModel.Projeto
{
    public class ProjetoConvidadosViewModel: BaseViewModel
    {
        public ObservableCollection<ConviteProjeto> convidados { get; set; }
        public ObservableCollection<ConviteProjeto> Convidados { get { return convidados; } set { convidados = value; OnPropertyChanged(nameof(Convidados)); } }
        private ProjetoService servicoProjeto;

        public ProjetoConvidadosViewModel()
        {
            servicoProjeto = new ProjetoService();
            Convidados = new ObservableCollection<ConviteProjeto>(servicoProjeto.ObterConvitesEnviadosDoProjeto());
        }
    }
}
