using System;
using Xamarin.Forms;

[assembly: Dependency(typeof(Xamarin.HLP.Mobile.AppPE.iOS.Config))]
namespace Xamarin.HLP.Mobile.AppPE.iOS
{

    class Config : IConfig
    {
        private string _directoryDb;
        //private ISQLitePlatform _plataforma;

        public string DirectoryDB
        {
            get
            {
                if (string.IsNullOrEmpty(_directoryDb))
                {
                    var directory = System.Environment.GetFolderPath(Environment.SpecialFolder.Personal);
                    _directoryDb = System.IO.Path.Combine(directory, "..", "Library");
                }
                return _directoryDb;
            }
        }

        //public ISQLitePlatform Platforma
        //{
        //    get
        //    {
        //        if (_plataforma == null)
        //        {
        //            _plataforma = new SQLitePlatformIOS();
        //        }
        //        return _plataforma;

        //    }
        //}
    }
}
