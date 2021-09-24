using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;
using Xamarin.HLP.Mobile.AppPE.Common;

namespace Xamarin.HLP.Mobile.AppPE.Model
{
    public class SelectItemGenericModel : ModelComum
    {
        public ICommand CommandSelectItem { get; set; }

        private string title;
        public string Title
        {
            get { return title.ToUpper(); }
            set { title = value; base.NotifyPropertyChanged(); }
        }

        public string Footer => String.Format("TOTAL DE REGISTRO(s): {0}", this.LitemSelectModels != null ? this.LitemSelectModels.Count().ToString() : "0");


        private BasicPickerModel _currentItem;
        public BasicPickerModel CurrentItem
        {
            get { return _currentItem; }
            set { _currentItem = value; base.NotifyPropertyChanged(); }
        }


        private List<BasicPickerModel> _litemSelectModels = new List<BasicPickerModel>();
        public List<BasicPickerModel> LitemSelectModels
        {
            get { return _litemSelectModels; }
            set { _litemSelectModels = value; NotifyPropertyChanged(); }
        }

    }
}
