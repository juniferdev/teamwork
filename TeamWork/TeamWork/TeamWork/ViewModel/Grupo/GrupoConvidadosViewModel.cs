using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TeamWork.Model;
using TeamWork.Service;

namespace TeamWork.ViewModel.Grupo
{
    public class GrupoConvidadosViewModel: BaseViewModel
    {
        public ObservableCollection<ConviteGrupo> convidados { get; set; }
        public ObservableCollection<ConviteGrupo> Convidados { get { return convidados; } set { convidados = value; OnPropertyChanged(nameof(Convidados)); } }
        private GrupoService servicoGrupo;

        public GrupoConvidadosViewModel()
        {
            servicoGrupo = new GrupoService();
            Convidados = new ObservableCollection<ConviteGrupo>(servicoGrupo.ObterConvitesEnviadosDoGrupo());
        }
    }
}
