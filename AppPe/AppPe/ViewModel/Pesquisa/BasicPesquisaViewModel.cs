using System.Collections.Generic;
using Xamarin.HLP.Mobile.AppPE.Common;
using Xamarin.HLP.Mobile.AppPE.Model;

namespace Xamarin.HLP.Mobile.AppPE.ViewModel.Pesquisa
{
    public class BasicPesquisaViewModel : NotifyCommon
    {

        private ListItemModel _currentItemModel;

        public ListItemModel currentItemModel
        {
            get { return _currentItemModel; }
            set { _currentItemModel = value; NotifyPropertyChanged();  }
        }



        private List<ListItemModel> _Itens = new List<ListItemModel>();

        public List<ListItemModel> Itens
        {
            get { return _Itens; }
            set { _Itens = value; NotifyPropertyChanged(); }
        }



    }
}
