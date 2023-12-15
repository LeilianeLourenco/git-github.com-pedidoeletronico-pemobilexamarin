using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Xamarin.HLP.Mobile.AppPE.Common;
using Xamarin.HLP.Mobile.AppPE.Model.Lancamento;
using Xamarin.HLP.Mobile.AppPE.ViewModel.Pedido;

namespace Xamarin.HLP.Mobile.AppPE.View.Pedido
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PageSignaturePedidoVenda : ContentPage
    {
        public static SignaturePedidoVendaViewModel viewmodelStatic { get; set; }
        private PedidoVendaListarModel _currentModel;
        public PageSignaturePedidoVenda(PedidoVendaListarModel currentModel)
        {
            _currentModel = currentModel;
            viewmodelStatic = new SignaturePedidoVendaViewModel(currentModel.idPedidoVendaOffLine);
            InitializeComponent();
        }

        private async void btnSubmit_Clicked(object sender, EventArgs e)
        {     
            var image = await signatureviews.GetImageStreamAsync(SignaturePad.Forms.SignatureImageFormat.Png);
            //var mStream = (MemoryStream)image;
            //byte[] data = mStream.ToArray();
            //string base64Val = Convert.ToBase64String(data);          
            viewmodelStatic.SalvarAssinatura(image);
            UtilNavidate.PopAsync();
        }
    }
}