using System;
using Xamarin.HLP.Mobile.AppPE.Controls.Custom;

namespace Xamarin.HLP.Mobile.AppPE.Controls.xaml
{
    public partial class GridToFind : GridButton
    {
        public GridToFind()
        {
            try
            {
                InitializeComponent();
            }
            catch (Exception ex)
            {
                throw;
            }
            
        }


        private bool _ShowImage = true;

        public bool ShowImage
        {
            get { return _ShowImage; }
            set
            {
                _ShowImage = value;
                if (!value)
                {
                    GridImage.IsVisible = false;
                }
            }
        }


    }
}
