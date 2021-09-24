using Xamarin.Forms;

namespace Xamarin.HLP.Mobile.AppPE.Controls.xaml
{
    public partial class GridSeparador : Grid
    {
        public GridSeparador()
        {
            InitializeComponent();
        }

        public string Text
        {
            get { return (string)GetValue(TextProperty); }
            set { SetValue(TextProperty, value); }
        }


        public static readonly BindableProperty TextProperty =
          BindableProperty.Create<GridSeparador, string>(o => o.Text, string.Empty, propertyChanged: OnTextChanged);


        private static void OnTextChanged(BindableObject bindable, string oldvalue, string newvalue)
        {
            var ctrl = bindable as GridSeparador;
            if (ctrl != null) ctrl.LabelSeparador.Text = newvalue;
        }
    }
}
