using System.Collections.ObjectModel;
using Xamarin.Forms;
using Xamarin.HLP.Mobile.AppPE.Common;

namespace Xamarin.HLP.Mobile.AppPE.Model
{
    public class MenuModel : ModelComum
    {
        private ObservableCollection<MenuItemModel> _lMenuItemModel;
        public ObservableCollection<MenuItemModel> LMenuItemModel
        {
            get { return _lMenuItemModel; }
            set { _lMenuItemModel = value; base.NotifyPropertyChanged(); }
        }
        private string _icon;
        public string Icon
        {
            get { return _icon; }
            set { _icon = value; base.NotifyPropertyChanged(); }
        }

        public MenuModel()
        {
            Icon = Device.OnPlatform("ApplicationMenuSelect.png", "ApplicationMenuSelect.png", "Assets/ApplicationMenuSelect.png");
            LMenuItemModel = new ObservableCollection<MenuItemModel>(new MenuItemDataModel());
            

        }


       

    }
}
