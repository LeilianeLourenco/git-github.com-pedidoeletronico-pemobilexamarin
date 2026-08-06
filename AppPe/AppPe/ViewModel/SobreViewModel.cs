using System.Windows.Input;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.HLP.Mobile.AppPE.Common;
using Xamarin.HLP.Mobile.AppPE.Model;
using Xamarin.HLP.Mobile.AppPE.View;
using Xamarin.HLP.Mobile.AppPE.View.Sincronizacao;

namespace Xamarin.HLP.Mobile.AppPE.ViewModel
{
    public class SobreViewModel : ViewModelComum<SobreModel>
    {
        public ICommand EntrarContatoCommand { get; set; }
        public ICommand TermosEpoliticaCommand { get; set; }

        public ICommand ForcarSyncInitCommand { get; set; }

        public ICommand SuporteRemotoDownloadCommand { get; set; }

        public ICommand ExcluirContaCommand { get; set; }


        public SobreViewModel()
        {
            currentModel = new SobreModel { versao = App.Versao.GetVersion(), imagePe = "Logo".ToImagemPNG() };
            ForcarSyncInitCommand = new Command(() =>
            {
                var pageSync = new PageSyncNew("Total");
                pageSync.ViewModel.bForcarSyncInit = true;
                UtilNavidate.Sincronizar(pageSync);
            });

            //EntrarContatoCommand = new Command(async () =>
            //{
            //    if (await App.IsConected())
            //    {
            //        UtilNavidate.PushAsync(new PageGoWeb(URL: "http://pedidoeletronico.azurewebsites.net/Home/Contato",
            //            xTitle: "Contate-nos"));
            //    }
            //    else
            //    {
            //        UtilMessages.InternetNecessaria();
            //    }
            //});

            TermosEpoliticaCommand = new Command(() =>
            {
                //if (await App.IsConected())
                //{
                //    UtilNavidate.PushAsync(new PageGoWeb(URL: "http://pedidoeletronico.com/Home/TermosCondicoesUso", xTitle: "Política de privacidade"));
                //}
                //else
                //{
                //    UtilMessages.InternetNecessaria();
                //}

                UtilNavidate.PushAsync(new PageOpenHtml("TERMOS","Termos"));

            });

            SuporteRemotoDownloadCommand = new Command(async () =>
            {
                if (await App.IsConected())
                {
                    if (Device.OS == TargetPlatform.Android)
                        await Share.RequestAsync(new ShareTextRequest
                        { 
                            Uri = "https://play.google.com/store/apps/details?id=com.teamviewer.quicksupport.market&hl=pt-br"
                        });


                    //await Plugin.Share.CrossShare.Current.OpenBrowser("https://play.google.com/store/apps/details?id=com.teamviewer.quicksupport.market&hl=pt-br");
                    else if (Device.OS == TargetPlatform.iOS)
                        //await Plugin.Share.CrossShare.Current.OpenBrowser("https://itunes.apple.com/br/app/teamviewer-quicksupport/id661649585?mt=8#");
                        await Share.RequestAsync(new ShareTextRequest
                        {
                            Uri = "https://itunes.apple.com/br/app/teamviewer-quicksupport/id661649585?mt=8#"
                        });
                    else if (Device.OS == TargetPlatform.WinPhone)
                        await Share.RequestAsync(new ShareTextRequest
                        {
                            Uri = "https://www.microsoft.com/pt-br/store/p/teamviewer-quicksupport-preview/9nblggh5kpgl"
                        });
                    //await Plugin.Share.CrossShare.Current.OpenBrowser("https://www.microsoft.com/pt-br/store/p/teamviewer-quicksupport-preview/9nblggh5kpgl");
                }
                else
                {
                    UtilMessages.InternetNecessaria();
                }


            });

            ExcluirContaCommand = new Command(() =>
            {
                UtilNavidate.PushAsync(new PageExcluirConta());
            });
        }

    }
}
