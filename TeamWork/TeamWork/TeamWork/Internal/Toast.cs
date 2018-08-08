using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TeamWork.Interface;
using Xamarin.Forms;

namespace TeamWork.Internal
{
    public class Toast
    {
        public static void ShortMessage(string message)
        {
            DependencyService.Get<IToast>().ShortAlert(message);
        }

        public static void LongMessage(string message)
        {
            DependencyService.Get<IToast>().LongAlert(message);
        }
    }
}
