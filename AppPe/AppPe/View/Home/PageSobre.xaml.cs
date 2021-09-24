using System;
using Xamarin.Forms;

namespace Xamarin.HLP.Mobile.AppPE.View.Home
{
    public partial class PageSobre : ContentPage
    {
        public PageSobre()
        {
            try
            {
                InitializeComponent();
            }
            catch (Exception ex)
            {
                ex.TrakException();
            }
        }
    }
}
