using System.Collections.Generic;
using Xamarin.Forms;
using Xamarin.HLP.Mobile.AppPE.Common;
using Xamarin.HLP.Mobile.AppPE.Model;
using Xamarin.HLP.Mobile.AppPE.ViewModel.Pesquisa;

namespace Xamarin.HLP.Mobile.AppPE.View.Pesquisas
{
    public partial class PageBasicList : ContentPage
    {

        public ListItemModel item { get; set; }
        public PageBasicList(ListItemModel _item, List<ListItemModel> lItens, string xTitle)
        {
            InitializeComponent();
            Title = xTitle;
            ViewModel.Itens = lItens;
            item = _item;
        }


        public BasicPesquisaViewModel ViewModel => BindingContext as BasicPesquisaViewModel;

        private void ListView_OnItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            if (e.SelectedItem != null)
            {

                item.Id = ViewModel.currentItemModel.Id;
                item.XId = ViewModel.currentItemModel.XId;
                item.Display = ViewModel.currentItemModel.Display;
                item.Detail = ViewModel.currentItemModel.Detail;

                UtilNavidate.PopModalAsync();
            }
        }
    }
}
