using System;
using System.Collections.Generic;
using Xamarin.Forms;
using Xamarin.HLP.Mobile.AppPE.Common;
using Xamarin.HLP.Mobile.AppPE.Droid;
using Xamarin.HLP.Mobile.AppPE.View;
using Xamarin.HLP.Mobile.AppPE.View.Agenda;
using Xamarin.HLP.Mobile.AppPE.View.Cliente;
using Xamarin.HLP.Mobile.AppPE.View.Home;
using Xamarin.HLP.Mobile.AppPE.View.ListaPreco;
using Xamarin.HLP.Mobile.AppPE.View.Pedido;
using Xamarin.HLP.Mobile.AppPE.View.Produto;
using Xamarin.HLP.Mobile.AppPE.View.Sincronizacao;

namespace Xamarin.HLP.Mobile.AppPE.Model
{
    public class MenuItemModel : ModelComum
    {
        private string _display;
        public string Display
        {
            get { return (_display ?? ""); }
            set { _display = value; NotifyPropertyChanged(); }
        }
        private string _image = null;
        public string Image
        {
            get { return _image; }
            set
            {
                _image = value.ToPathSvgMenu(); NotifyPropertyChanged();
            }
        }




        public Type TargetType { get; set; }
    }


    public class MenuItemDataModel : List<MenuItemModel>
    {
        public
            MenuItemDataModel()

        {

            this.Add(new MenuItemModel
            {
                Display = "Home",
                Image = "ApplicationMenuHome",
                TargetType = typeof(PageHomeNew)
            });

            this.Add(new MenuItemModel
            {
                Display = "Pedido",
                Image = "ApplicationMenuLancamento",
                TargetType = typeof(PageListarPedidos)
            });

            this.Add(new MenuItemModel
            {
                Display = "Clientes",
                Image = "ApplicationMenuCliente",
                TargetType = typeof(PageInfinitListClientes)

            });

            this.Add(new MenuItemModel
            {
                Display = "Preços",
                Image = "ApplicationListaPreco",
                TargetType = typeof(PageListaPreco)
            });

            this.Add(new MenuItemModel
            {
                Display = "Produtos",
                Image = "ApplicationMenuProduto",
                TargetType = typeof(PageInfinitListProdutos)
            });

            this.Add(new MenuItemModel
            {
                Display = "Agenda/CRM",
                Image = "ApplicationMenuAgenda",
                TargetType = typeof(PageListagemEventos)
            });

            this.Add(new MenuItemModel
            {
                Display = "Sincronização",
                Image = "ApplicationMenuSync",
                TargetType = typeof(PageSyncNew)
            });

            //this.Add(new MenuItemModel
            //{
            //    Display = "Empresa",
            //    Detail = "informações atualizadas...",
            //    Image = "ApplicationMenuEmpresa",
            //    ImageArrow = "ApplicationArrowRightMenuEmpresa",
            //    Cor = Color.FromHex("F44336"),
            //    TargetType = typeof(PageEmpresa)
            //});

            if (Device.OS != TargetPlatform.iOS)
                this.Add(new MenuItemModel
                {
                    Display = "Portal",
                    Image = "ApplicationMenuRelatorio",
                    TargetType = typeof(PageLiberadoSoWeb)
                });


            this.Add(new MenuItemModel
            {                
                Display = "Chat On-Line",
                Image = "ApplicationMenuChat",
                TargetType = typeof(ChatOnLine)
            });
            this.Add(new MenuItemModel
            {
                Display = "Sobre",
                Image = "ApplicationMenuSobre",
                TargetType = typeof(PageSobre)
            });

            this.Add(new MenuItemModel()
            {
                Display = "Sair",
                Image = "ApplicationMenuExit",
            });

            //this.Add(new MenuItemModel()
            //{
            //    Title = "Configuração",
            //    Detail = "configurações básicas...",
            //    IconSource = Device.OnPlatform("ApplicationMenuInfo.png", "ApplicationMenuInfo.png", "Assets/ApplicationMenuInfo.png"), //"ApplicationMenuInfo.png",
            //    TargetType = typeof(PageHome)
            //});


        }
    }
}
