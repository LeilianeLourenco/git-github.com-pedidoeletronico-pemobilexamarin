using System;
using System.Windows.Input;
using Xamarin.Forms;
using Xamarin.HLP.Mobile.AppPE.Common;
using Xamarin.HLP.Mobile.AppPE.Model;
using Xamarin.HLP.Mobile.AppPE.Model.Repository;
using Xamarin.HLP.Mobile.AppPE.ViewModel.Home;
using ImageSource = Xamarin.Forms.ImageSource;

namespace Xamarin.HLP.Mobile.AppPE.ViewModel
{
    public class MenuViewModel : ViewModelComum<MenuModel>
    {



        public MenuViewModel()
        {
            try
            {
                currentModel = new MenuModel();
                Logoff = new Command(UtilNavidate.Logoff);
                //var empresa = EmpresaRepository.GetEmpresa();
                var usuario = EmpresaAspnetUsersRepository.GetUsuario();
                //UserImageSource = UtilMethods.GetLocalImage(empresa?.xCaminhoImgCapa,
                //      UtilMethods.TipoImagem.CapaEmpresa);
                UserImageSource = UtilMethods.GetLocalImage(usuario?.imUsuario,
                            UtilMethods.TipoImagem.Usuario);
            }
            catch (Exception ex)
            {
                ex.TrakException();
            }

        }

        private string _xNameEmpresa;

        public string xNameEmpresa
        {
            get { return _xNameEmpresa; }
            set { _xNameEmpresa = value; NotifyPropertyChanged(); }
        }

        private string _xEmail;

        public string xEmail
        {
            get { return _xEmail; }
            set { _xEmail = value; NotifyPropertyChanged(); }
        }




        public void AtualizaMenu(ImageSource _userImageSource, ImageSource _capaImageSource)
        {
            Device.BeginInvokeOnMainThread(() =>
            {
                xNameEmpresa = EmpresaRepository.GetNameEmpresa(App.CurrentAspnetUserModel.objEmpresaAspnetUsersModel.idEmpresa_aspnetUsers ?? 0);
                xEmail = App.CurrentAspnetUserModel.objEmpresaAspnetUsersModel.xEmail;

                UserImageSource = _userImageSource;
                capaImageSource = _capaImageSource;
            });
        }

        private ImageSource _capaImageSource = null;

        public ImageSource capaImageSource
        {
            get { return _capaImageSource; }
            set { _capaImageSource = value; NotifyPropertyChanged(); }
        }


        private ImageSource _userImageSource = null;
        public ImageSource UserImageSource
        {
            get { return _userImageSource; }
            set
            {
                _userImageSource = value;
                NotifyPropertyChanged();
            }
        }

        public ICommand Logoff { get; set; }



    }
}

