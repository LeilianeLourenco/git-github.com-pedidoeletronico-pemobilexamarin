using System.Runtime.Serialization;
using System.Windows.Input;
using Xamarin.Forms;
using Xamarin.HLP.Mobile.AppPE.Common;

namespace Xamarin.HLP.Mobile.AppPE.Model
{
    public class LoginViewModel : ModelComum
    {
        
        [IgnoreDataMember]
        public ICommand LoginCommand { get; set; }


        private string _email = "";
        public string Email
        {
            get { return _email; }
            set
            {
                _email = value;
                NotifyPropertyChanged();
                LoginCommand.ChangeCanExecute();
            }
        }

        private string _password = "";
        public string Password
        {
            get { return _password; }
            set { _password = value; NotifyPropertyChanged(); LoginCommand.ChangeCanExecute(); }
        }

        private bool _bProcessando;
        public bool BProcessando
        {
            get { return _bProcessando; }
            set
            {
                _bProcessando = value;
                if (value)
                {
                    ControleNavigationCommand?.Execute(null);
                }

                NotifyPropertyChanged();
                LoginCommand.ChangeCanExecute();
            }
        }

        public ICommand ControleNavigationCommand { get; set; }

        
    }
}
