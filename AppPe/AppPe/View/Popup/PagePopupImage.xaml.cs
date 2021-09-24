using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Rg.Plugins.Popup.Pages;
using Xamarin.Forms;

namespace Xamarin.HLP.Mobile.AppPE.View.Popup
{
    public partial class PagePopupImage : PopupPage
    {
        public PagePopupImage(ImageSource img)
        {
            InitializeComponent();
            ImgCard.Source = img;
            CloseWhenBackgroundIsClicked = true;
        }


        protected override void OnAppearing()
        {
            base.OnAppearing();
            FrameiMAGE.HeightRequest = base.Width;
            FrameiMAGE.WidthRequest = base.Width;
        }
    }
}
