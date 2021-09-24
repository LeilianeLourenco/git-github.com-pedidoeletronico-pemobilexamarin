namespace Xamarin.HLP.Mobile.AppPE.Model.Cadastros
{
    //public class NavigationsCommands
    //{
    //    public ICommand NavigateToCommand { get; set; }
    //    public ICommand NavidateToBackCommand { get; set; }
    //    public ICommand CancelCommand { get; set; }



    //    public NavigationsCommands()
    //    {
    //        CancelCommand = new Command(() => { UtilMessages.QuestionToBack(); });
    //        NavigateToCommand = new Command(NavigateTo);
    //        NavidateToBackCommand = new Command((async) => { UtilNavidate.PopAsync(); });
    //    }

    //    public async void NavigateTo(object param)
    //    {
    //        if (param == null) return;
    //        if (param.ToString() == "PageSync")
    //        {
    //            //UtilNavidate.PushAsync(StaticModel.PageSincronizacao);
    //            //UtilNavidate.PushModalAsync(StaticModel.PageSincronizacao);
    //            RootPage.SyncAuto2(false);
    //        }
    //        else if (param.ToString() == "PageEmpresa")
    //        {
    //            if (await App.IsConected() || App.CurrentAspnetUserModel.lEpresaAspnetUsersModel.Count == 1)
    //                UtilNavidate.PushAsync(new PageEmpresa());
    //            else
    //            {
    //                await App.Messages.ShowAsync("É necessário conexão com internet para trocar de empresa.");
    //            }
    //        }
    //        else if (param.ToString() == "PageListaPedido")
    //        {
    //            UtilNavidate.PushAsync(new PageListagemPedidoTemplate());
    //        }
    //        else if (param.ToString() == "PageInfinitListClientes")
    //        {
    //            UtilNavidate.PushAsync(new PageInfinitListClientes());
    //        }
    //        else if (param.ToString() == "PageListaPreco")
    //        {
    //            UtilNavidate.PushAsync(new PageListaPreco());
    //        }
    //        else if (param.ToString() == "PageCliente")
    //        {
    //            //UtilNavidate.PushAsync(new PageCliente());
    //        }
    //        else if (param.ToString() == "PageCliente+")
    //        {
    //            //StaticModel.StaticClientesModel = new ClientesModel();
    //            UtilNavidate.PushAsync(new PageCliente(new ClientesModel()));
    //        }
    //        else if (param.ToString() == "PageLproduto")
    //        {
    //            UtilNavidate.PushAsync(new PageLproduto());
    //        }
    //        else if (param.ToString() == "PageContato")
    //        {
    //            //UtilNavidate.PushAsync(new PageContato());
    //        }
    //        else if (param.ToString() == "PageContato+")
    //        {
    //            StaticModel.StaticClientesModel.objContatoModel = new ContatoModel();
    //            //UtilNavidate.PushAsync(new PageContato());
    //        }
    //        else if (param.ToString() == "PageEndereco+")
    //        {
    //            //StaticModel.StaticClientesModel.objEndereco = new EnderecoModel();
    //            //UtilNavidate.PushAsync(new PageEndereco());
    //        }
    //        else if (param.ToString() == "PagePedido+")
    //        {
    //            UtilNavidate.PushAsync(new PagePedidoNew(new PedidoVendaModel()));
    //        }
    //    }
    //}
}
