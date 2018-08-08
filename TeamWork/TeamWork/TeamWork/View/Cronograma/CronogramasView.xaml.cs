using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TeamWork.Internal;
using TeamWork.ViewModel.Cronograma;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TeamWork.View.Cronograma
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CronogramasView : ContentPage
    {
        private ScrollView scroll;
        private Grid grid;
        private CronogramasViewModel vm;
        private Label NomeTarefa, Duracao;
        private DateTime menorData, maiorData;
        private List<DateTime> datasDoCronograma;

        public CronogramasView()
        {
            InitializeComponent();

            vm = new CronogramasViewModel();
            BindingContext = vm;

            Title = "Cronograma";

            grid = new Grid() { BackgroundColor = Color.FromHex("#e9ebee") };

            DesenharCronograma();

            var scroll = new ScrollView() { Orientation = ScrollOrientation.Both };
            scroll.Content = grid;

            Content = scroll;

        }

        public void DesenharCronograma()
        {
            if(vm.Tarefas.Count == 0)
            {
                Toast.ShortMessage(Mensagem.MENS_FORM_46);
                return;
            }
            else
            {
                AdicionarBarraCalendario();
                ListarNomes();
                ListarDuracoes();
            }

        }

        public void AdicionarLinhaVazia()
        {
            grid.RowDefinitions.Add(new RowDefinition { Height = 40 });
            grid.Children.Add(new Label() ,0,0);
        }

        public void AdicionarBarraCalendario()
        {
            int coluna = 1, linha = 0;
            datasDoCronograma = new List<DateTime>();

            grid.RowDefinitions.Add(new RowDefinition { Height = 20 });

            menorData = vm.Tarefas.Min(d => d.DataPrevInicio);
            maiorData = vm.Tarefas.Max(d => d.DataPrevTermino);

            for (var data = menorData; data <= maiorData.AddDays(1); data = data.AddDays(1))
            {
                datasDoCronograma.Add(data);
            }

            grid.ColumnSpacing = 0;
            grid.RowSpacing = 3;

            grid.ColumnDefinitions.Add(new ColumnDefinition { Width = 100 });
            grid.Children.Add(new Label { Text = "Tarefas", HorizontalTextAlignment = TextAlignment.Center, BackgroundColor = Color.LightGray });

            foreach (var data in datasDoCronograma)
            {
                grid.ColumnDefinitions.Add(new ColumnDefinition { Width = 50 });
                grid.Children.Add(new Label { Text = data.ToString("|  dd/MM"), BackgroundColor = Color.LightGray }, coluna, linha);
                coluna++;
            }
        }

        public void ListarNomes()
        {
            int linha = 1, coluna = 0;
            var tarefasOrdenadas = vm.Tarefas.OrderBy(d => d.DataPrevInicio);

            foreach (var tarefa in tarefasOrdenadas)
            {
                grid.RowDefinitions.Add(new RowDefinition { Height = 30 });
                grid.Children.Add(new Label { Text = tarefa.NomeTarefa }, coluna, linha);
                linha++;
            }
        }

        public void ListarDuracoes()
        {
            int linha = 1;
            Label duracaoLabel;

            var tarefasOrdenadas = vm.Tarefas.OrderBy(d => d.DataPrevInicio);

            foreach (var tarefa in tarefasOrdenadas)
            {
                grid.ColumnDefinitions.Add(new ColumnDefinition { Width = 50 });
                duracaoLabel = new Label { BackgroundColor = CorTarefa(tarefa) };
                grid.Children.Add(duracaoLabel, DuracaoInicio(tarefa.DataPrevInicio), linha);
                Grid.SetColumnSpan(duracaoLabel, DuracaoFinal(tarefa.DataPrevInicio, tarefa.DataPrevTermino));
                linha++;
            }

        }

        public int DuracaoInicio(DateTime dataPrevInicio)
        {
            int duracaoInicio = (dataPrevInicio.Date - menorData.Date).Days;

            // Incrementamos + 1 pois as colunas começam do 0 e as datas da coluna 1.

            return duracaoInicio + 1; 
        }

        public int DuracaoFinal(DateTime dataPrevInicio, DateTime dataPrevTermino)
        {
            int diferencaDias = (dataPrevTermino.Date - dataPrevInicio.Date).Days;

            return diferencaDias == 0 ? 1 : diferencaDias + 1;
        }

        public Color CorTarefa(Model.Tarefa tarefa)
        {
            if (DateTime.Now.Date <= tarefa.DataPrevTermino.Date && tarefa.Estado == Internal.Estado.Aberta ||
                tarefa.Estado == Internal.Estado.Iniciada)
            {
                return Color.FromHex("#010870");
            }
            else if (DateTime.Now.Date > tarefa.DataPrevTermino.Date && tarefa.Estado == Internal.Estado.Aberta ||
                tarefa.Estado == Internal.Estado.Iniciada)
            {
                return Color.DarkRed;
            }
            else
            {
                return Color.Green;
            }
        }
    }
}