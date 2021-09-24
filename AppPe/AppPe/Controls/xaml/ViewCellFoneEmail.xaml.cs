using Xamarin.Forms;

namespace Xamarin.HLP.Mobile.AppPE.Controls.xaml
{
    public partial class ViewCellFoneEmail : ViewCell
    {
        public ViewCellFoneEmail(string xValor, TipoCompoenent _tipoCompoenent)
        {
            InitializeComponent();

            labelValor.Text = xValor;
            tipoCompoenent = _tipoCompoenent;
        }


        public enum TipoCompoenent
        {
            EMAIL,
            TELEFONE
        }

        private TipoCompoenent _tipoCompoenent;
        public TipoCompoenent tipoCompoenent
        {
            get { return _tipoCompoenent; }
            set
            {
                _tipoCompoenent = value;
                if (value == TipoCompoenent.EMAIL)
                {
                    labelTipo.Text = "E-MAIL";
                    SvgImageComp.SvgPath = "Xamarin.HLP.Mobile.AppPE.Images.PagesIcon.ApplicationEmail.svg";

                }
                else if (value == TipoCompoenent.TELEFONE)
                {
                    labelTipo.Text = "TELEFONE";
                    SvgImageComp.SvgPath = "Xamarin.HLP.Mobile.AppPE.Images.PagesIcon.ApplicationPhone.svg";
                }
            }
        }
    }
}
