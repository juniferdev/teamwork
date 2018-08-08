using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace TeamWork.ViewModel
{
    public class BaseViewModel: INotifyPropertyChanged
    {
        private string alertaForm;
        public string AlertaForm
        {
            get
            {
                return alertaForm;
            }
            set
            {
                alertaForm = value;
                OnPropertyChanged(nameof(AlertaForm));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            var changed = PropertyChanged;
            if (changed != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

    }
}
