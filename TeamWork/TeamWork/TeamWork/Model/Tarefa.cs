using SQLite.Net.Attributes;
using SQLiteNetExtensions.Attributes;
using System;
using System.Collections.Generic;
using System.Text;
using TeamWork.Internal;
using Xamarin.Forms;

namespace TeamWork.Model
{
    public class Tarefa
    {
        public Tarefa()
        {
            Estado = Estado.Aberta;
        }

        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        [MaxLength(200)]
        public string NomeTarefa { get; set; }

        [ForeignKey(typeof(Usuario))]
        public int IdCriador { get; set; }

        [ForeignKey(typeof(Usuario))]
        public int IdResponsavel { get; set; }

        [ForeignKey(typeof(Projeto))]
        public int IdProjeto { get; set; }

        public TipoTarefa TipoTarefa { get; set; }

        [MaxLength(400)]
        public string DescricaoTarefa { get; set; }

        public DateTime DataPrevInicio { get; set; }

        public DateTime DataPrevTermino { get; set; }

        public DateTime DataInicio { get; set; }

        public DateTime DataTermino { get; set; }

        public Estado Estado { get; set; }

        public Razao Motivo { get; set; }

        public Color CorSituacao => CorTarefa();

        public Color CorTarefa()
        {
            if(Situacao == "No Prazo")
            {
                return Color.DarkSlateBlue;
            }
            else if (Situacao == "Atrasada")
            {
                return Color.Red;
            }
            else
            {
                return Color.DarkGreen;
            }
        }

        public string TextTarefa => Id.ToString() + " - " + NomeTarefa;

        public string DetailTarefa => SituacaoTarefa() + " - " + DataPrevTermino.Date.ToString();

        public string Situacao => SituacaoTarefa();

        public string SituacaoTarefa()
        {
            if (DateTime.Now.Date <= DataPrevTermino.Date && Estado == Internal.Estado.Aberta ||
                Estado == Internal.Estado.Iniciada)
            {
                return "No Prazo";
            }
            else if (DateTime.Now.Date > DataPrevTermino.Date && Estado == Internal.Estado.Aberta ||
                Estado == Internal.Estado.Iniciada)
            {
                return "Atrasada";
            }
            else
            {
                return "Finalizada";
            }
        }
    }
}
