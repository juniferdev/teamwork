using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TeamWork.Model;
using TeamWork.Repository;
using TeamWork.Service;
using Xamarin.Forms;

namespace TeamWork.ViewModel.Conta
{
    public class ConvitesViewModel: BaseViewModel
    {
        public ObservableCollection<ConviteGrupo> convitesGrupo { get; set; }
        public ObservableCollection<ConviteGrupo> ConvitesGrupo { get { return convitesGrupo; } set { convitesGrupo = value; OnPropertyChanged(nameof(ConvitesGrupo)); } }

        public ObservableCollection<ConviteProjeto> convitesProjeto { get; set; }
        public ObservableCollection<ConviteProjeto> ConvitesProjeto { get { return convitesProjeto; } set { convitesProjeto = value; OnPropertyChanged(nameof(ConvitesProjeto)); } }

        //private ConviteLista convites;

        public GrupoService servicoGrupo;
        public ProjetoService servicoProjeto;
        public ContaService servicoConta;
        public ConviteRepository dadosConvite;

        public object labelGrupo = "Grupos";
        public object LabelGrupo
        {
            get { return labelGrupo; }
            set { labelGrupo = value; OnPropertyChanged(nameof(labelGrupo)); }
        }

        public object labelProjeto = "Projetos";
        public object LabelProjeto
        {
            get { return labelProjeto; }
            set { labelProjeto = value; OnPropertyChanged(nameof(labelProjeto)); }
        }

        public ConvitesViewModel()
        {
            servicoGrupo = new GrupoService();
            servicoProjeto = new ProjetoService();
            ConvitesGrupo = new ObservableCollection<ConviteGrupo>(servicoGrupo.ObterConvitesDoUsuarioLogado().ConvitesParaGrupos);
            ConvitesProjeto = new ObservableCollection<ConviteProjeto>(servicoProjeto.ObterConvitesDoUsuarioLogado().ConvitesParaProjetos);
            servicoConta = new ContaService();
            TemConviteGrupo();
            TemConviteProjeto();
        }

        public object TemConviteGrupo()
        {
            if (servicoConta.UsuarioTemNovosConvites())
            {
                if (dadosConvite == null)
                {
                    dadosConvite = new ConviteRepository();
                }

                int idUsuario = (int)Application.Current.Properties["id"];

                var convites = dadosConvite.ConsultarConvitesDoUsuario(idUsuario);

                if(convites.ConvitesParaGrupos.Count() > 0)
                {
                    LabelGrupo = $"Grupos ({convites.ConvitesParaGrupos.Count()})";
                }
            }

            return LabelGrupo;
        }

        public object TemConviteProjeto()
        {
            if (servicoConta.UsuarioTemNovosConvites())
            {
                if (dadosConvite == null)
                {
                    dadosConvite = new ConviteRepository();
                }

                int idUsuario = (int)Application.Current.Properties["id"];

                var convites = dadosConvite.ConsultarConvitesDoUsuario(idUsuario);

                if (convites.ConvitesParaProjetos.Count() > 0)
                {
                    LabelProjeto = $"Projetos ({convites.ConvitesParaProjetos.Count()})";
                }
            }

            return LabelProjeto;
        }
    }
}
