using Xamarin.Forms;
using Xamarin.HLP.Mobile.AppPE.Common;
using Xamarin.HLP.Mobile.AppPE.ViewModel.Image;

namespace Xamarin.HLP.Mobile.AppPE.View.Image
{
    public partial class PageShowImages : ContentPage
    {
        public PageShowImages()
        {
            InitializeComponent();
            ViewModel.CompCatagalogo = CompCatalogo;
        }
        public int idProduto { get; set; }
        public string xDisplay { get; set; }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            ViewModel.width = GridPrincipal.Width;
            ViewModel.idProduto = idProduto;
            LabelTitle.Text = xDisplay ?? "SEM TÍTULO";
            BuscarImagens();
        }

        private void BuscarImagens()
        {
            Device.StartTimer(UtilMethods.GetStartTime, ViewModel.OnApparing);

        }

        public ShowImageViewModel ViewModel => BindingContext as ShowImageViewModel;
    }
}
