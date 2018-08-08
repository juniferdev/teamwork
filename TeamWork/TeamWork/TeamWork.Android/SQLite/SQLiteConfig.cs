using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

using TeamWork.Interface;
using SQLite.Net.Interop;
using SQLite.Net.Platform.XamarinAndroid;
using Xamarin.Forms;

// A instrução abaixo associa esta classe como o serviço que responde a dependência da interface.
[assembly: Dependency(typeof(TeamWork.Droid.SQLite.SQLiteConfig))]
namespace TeamWork.Droid.SQLite
{
    public class SQLiteConfig : ISQLiteConfig
    {
        private string _diretorioSQLite;
        private ISQLitePlatform _plataforma;

        public string DiretorioSQLite
        {
            get
            {
                if (string.IsNullOrEmpty(_diretorioSQLite))
                {
                    _diretorioSQLite = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal);
                }
                return _diretorioSQLite;
            }
        }

        public ISQLitePlatform Plataforma
        {
            get
            {
                if(_plataforma == null)
                {
                    _plataforma = new SQLitePlatformAndroid();
                }
                return _plataforma;
            }
        }
    }
}