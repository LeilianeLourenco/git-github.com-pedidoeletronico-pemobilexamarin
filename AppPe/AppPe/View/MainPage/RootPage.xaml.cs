using System;
using System.Collections.Generic;
using FFImageLoading.Transformations;
using FFImageLoading.Work;
using Xamarin.Forms;
using Xamarin.HLP.Mobile.AppPE.Common;
using Xamarin.HLP.Mobile.AppPE.Model;
using Xamarin.HLP.Mobile.AppPE.View.Home;
using Xamarin.HLP.Mobile.AppPE.View.Sincronizacao;
using Xamarin.HLP.Mobile.AppPE.ViewModel;

namespace Xamarin.HLP.Mobile.AppPE.View.MainPage
{
    public partial class RootPage : MasterDetailPage
    {
        public static MenuViewModel currentMenuStatic { get; set; }

        public RootPage()
        {
            try
            {
                InitializeComponent();
                currentMenuStatic = ViewModel;
                //Detail = new NavigationPage(new PageHome());
                Detail = new NavigationPage(new PageHomeNew());
                //Detail = new NavigationPage(new PagePedidoNew(new PedidoVendaModel()));
                userImage.Transformations = new List<ITransformation> { new CircleTransformation(), new CropTransformation(1, 0, 0) };
            }
            catch (Exception ex)
            {
                ex.TrakException();
            }
        }

        public MenuViewModel ViewModel => this.BindingContext as MenuViewModel;


        async void NavigateTo(MenuItemModel menu)
        {
            if (menu == null)
                return;
            if (menu.TargetType != null)
            {
                Page displayPage = null;

                if (menu.TargetType == typeof(PageOpenHtml))
                {
                    if (await App.IsConected())
                    {
                        Detail = new NavigationPage(new PageOpenHtml("chat", "Chat On-line"));
                        return;
                    }
                    else
                    {
                        UtilMessages.InternetNecessaria();
                        return;
                    }
                }
                else if (menu.TargetType == typeof(PageHomeNew))
                {
                    displayPage = StaticModel.PageHome;
                }
                else if (menu.TargetType != typeof(PageSyncNew))
                    displayPage = (Page)Activator.CreateInstance(menu.TargetType);



                if (displayPage != null && displayPage.GetType() == typeof(PageHomeNew))
                    Detail = new NavigationPage(new PageHomeNew());
                else
                {
                    if (menu.TargetType == typeof(PageSyncNew))
                    {
                        UtilNavidate.Sincronizar(new PageSyncNew("Total"));
                    }
                    UtilNavidate.PushAsync(displayPage);
                }

            }
            else if (menu.Display.ToUpper().Equals("SAIR"))
            {
                UtilNavidate.Logoff();
            }
            IsPresented = false;
        }

        private void MenuList_OnItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            if (e.SelectedItem == null) return;
            var menuSelected = (e.SelectedItem as MenuItemModel);
            NavigateTo(menuSelected);
            MenuList.SelectedItem = null;
            IsPresented = false;
        }


        protected override bool OnBackButtonPressed()
        {
            if (Device.OS == TargetPlatform.Android || Device.OS == TargetPlatform.iOS)
            {

                if (Device.OS == TargetPlatform.Android)
                {
                    var navigationPage = Detail as NavigationPage;
                    if (navigationPage != null && navigationPage.CurrentPage.GetType() == typeof(PageHomeNew))
                    {
                        return true;
                    }
                }


                if (pageClickedMenu == null)
                    return base.OnBackButtonPressed();

                var detail = Detail as NavigationPage;



                if (detail != null)
                {
                    if (detail.CurrentPage.GetType() == typeof(PageHomeNew))
                        return base.OnBackButtonPressed();
                    if (detail.CurrentPage.GetType() == pageClickedMenu.GetType())
                    {
                        Detail = new NavigationPage(new PageHomeNew());
                        return true;
                    }
                }
                return base.OnBackButtonPressed();
            }
            return base.OnBackButtonPressed();
        }

        private static Page pageClickedMenu { get; set; }

    }
}
