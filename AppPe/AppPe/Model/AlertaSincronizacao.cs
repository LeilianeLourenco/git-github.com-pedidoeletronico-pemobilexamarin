using Xamarin.HLP.Mobile.AppPE.Common;

namespace Xamarin.HLP.Mobile.AppPE.Model
{
    public class AlertaSincronizacao : ModelComum
    {
        public AlertaSincronizacao()
        {
        }

        private string _Image;

        public string Image
        {
            get { return _Image; }
            set { _Image = value; NotifyPropertyChanged(); }
        }


        public int? idOffLine { get; set; }

        private string _Table;

        public string Table
        {
            get { return _Table; }
            set
            {
                _Table = value; NotifyPropertyChanged();

                if (value == TableMobile.TB_CLIENTES)
                {
                    Image = "Xamarin.HLP.Mobile.AppPE.Images.HomeIcon.ApplicationClienteHome.svg";
                }
                else if (value == TableMobile.TB_PRODUTO)
                {
                    Image = "Xamarin.HLP.Mobile.AppPE.Images.HomeIcon.ApplicationProdutoHome.svg";
                }
                else if (value == TableMobile.TB_CLIENTES)
                {
                    Image = "Xamarin.HLP.Mobile.AppPE.Images.HomeIcon.ApplicationClienteHome.svg";
                }
                else if (value == TableMobile.TB_PEDIDOVENDA)
                {
                    Image = "Xamarin.HLP.Mobile.AppPE.Images.HomeIcon.ApplicationPedidoHome.svg";
                }
            }
        }


        public string Display { get; set; }

        public string Detail { get; set; }

        public string DetailEstoque { get; set; }

        public bool bErro { get; set; } = true;
    }
}
