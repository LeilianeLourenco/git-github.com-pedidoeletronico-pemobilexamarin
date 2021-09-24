using Xamarin.Forms;

[assembly: Dependency(typeof(Xamarin.HLP.Mobile.AppPE.Droid.Config))]

namespace Xamarin.HLP.Mobile.AppPE.Droid
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
                    _directoryDb = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal);
                return _directoryDb;
            }
        }
        //public ISQLitePlatform Platforma
        //{
        //    get
        //    {
        //        if (_plataforma == null)
        //        {
        //            _plataforma = new SQLite.Net.Platform.XamarinAndroid.SQLitePlatformAndroidN();
        //        }
        //        return _plataforma;
        //    }
        //}
         
    }
}