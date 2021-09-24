using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.HLP.Mobile.AppPE.Common;

namespace Xamarin.HLP.Mobile.AppPE.Model.Home
{
    public class ImagesAppModel : NotifyCommon
    {
        private ImageSource _ImgEmpresa = null;
        public ImageSource ImgEmpresa
        {
            get { return _ImgEmpresa; }
            set
            {
                _ImgEmpresa = value;
                NotifyPropertyChanged();
            }
        }

        private ImageSource _ImgUser;
        public ImageSource ImgUser
        {
            get { return _ImgUser; }
            set { _ImgUser = value; NotifyPropertyChanged(); }
        }

        private ImageSource _ImgCapa;

        public ImageSource ImgCapa
        {
            get { return _ImgCapa; }
            set { _ImgCapa = value; NotifyPropertyChanged(); }
        }
    }
}
