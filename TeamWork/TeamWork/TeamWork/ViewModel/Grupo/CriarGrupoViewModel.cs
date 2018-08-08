using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TeamWork.Model;
using TeamWork.Service;
using Xamarin.Forms;

namespace TeamWork.ViewModel.Grupo
{
    public class CriarGrupoViewModel : BaseViewModel
    {
        public string NomeView { get; set; }
        public string ObjetivoView { get; set; }
        public bool incluirCriador { get; set; }
        public bool IncluirCriador { get { return incluirCriador; } set { incluirCriador = value; OnPropertyChanged(nameof(IncluirCriador)); } }

        public bool habilitarBotaoConvidar = false;
        public bool HabilitarBotaoConvidar { get { return habilitarBotaoConvidar; } set { habilitarBotaoConvidar = value; OnPropertyChanged(nameof(HabilitarBotaoConvidar)); } }

        public ObservableCollection<Usuario> Contatos { get; set; }
        public ObservableCollection<ConviteGrupo> Convidados { get; set; }

        public Command CriarGrupoCommand { get; set; }
        //public Command AdicionarContatoCommand { get; set; }
        //public Command RemoverContatoCommand { get; set; }
        public Command ConvidarContatoGrupoCommand { get; set; }

        public GrupoService servicoGrupo;

        public CriarGrupoViewModel()
        {
            servicoGrupo = new GrupoService();
            CriarGrupoCommand = new Command(CriarGrupo);
            ConvidarContatoGrupoCommand = new Command(ConvidarContatoGrupo);
            //AdicionarContatoCommand = new Command(AdicionarUsuario);
            Contatos = new ObservableCollection<Usuario>(servicoGrupo.ObterMembrosDoGrupoContatos());
            Convidados = new ObservableCollection<ConviteGrupo>();
            IncluirCriador = false;
        }

        //private void AdicionarUsuario()
        //{
        //    if (servicoGrupo == null)
        //    {
        //        servicoGrupo = new GrupoService();
        //    }
        //    int idGrupo = servicoGrupo.ObterIdGrupoSelecionado();
        //    servicoGrupo.AdicionarUsuarioAoGrupo(servicoGrupo.ObterIdContatoSelecionado(),idGrupo);
        //    // = ListarUsuariosDoGrupo();
        //}

        private void ConvidarContatoGrupo()
        {
            if (servicoGrupo.ContatoNaoFoiConvidado(Convidados))
            {
                Convidados.Add(servicoGrupo.CriarConviteGrupo());
            }
        }

        private void CriarGrupo()
        {
            servicoGrupo = new GrupoService(NomeView, ObjetivoView);

            if (servicoGrupo.CriarNovoGrupo(false, IncluirCriador)) // alterar o segundo false para a propriedade IncluirCriador
            {
                if (Convidados.Count > 0)
                {
                    servicoGrupo.EnviarConvitesGrupo(Convidados.ToList(), servicoGrupo.ObterUltimoGrupoInserido());
                }
                Application.Current.MainPage.Navigation.PopAsync();
            }

        }

        // Futuramente
        //private void CriarCopiaDoGrupo()
        //{

        //}

        public ObservableCollection<Model.Usuario> ListarUsuariosDoGrupo()
        {
            if (servicoGrupo == null)
            {
                servicoGrupo = new GrupoService();
            }
            return new ObservableCollection<Model.Usuario>(servicoGrupo.ObterUsuariosDoGrupoSelecionado());
        }
    }
}
