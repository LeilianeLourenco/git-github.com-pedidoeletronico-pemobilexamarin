using Xamarin.Forms;

namespace Xamarin.HLP.Mobile.AppPE.Controls.custom
{
    public class LabelPadding : StackLayout
    {

        private Label LabelIn { get; set; }

        public LabelPadding()
        {
            this.Orientation = StackOrientation.Horizontal;
            this.Padding = new Thickness(10, this.Padding.Top, this.Padding.Right, this.Padding.Bottom);
            LabelIn = new Label();
            this.Children.Add(LabelIn);
        }

        public static BindableProperty TextProperty =
            BindableProperty.Create<LabelPadding, string>(o => o.Text, "", propertyChanged: OnTextChanged);

        public string Text
        {
            get { return (string)GetValue(TextProperty); }
            set { SetValue(TextProperty, value); }
        }


        private static void OnTextChanged(BindableObject bindable, string oldvalue, string newvalue)
        {
            var label = bindable as LabelPadding;
            if (label != null) label.LabelIn.Text = newvalue.ToString();
        }

        private bool _colorRequired;

        public bool ColorRequired
        {
            get { return _colorRequired; }
            set
            {
                _colorRequired = value;
                LabelIn.TextColor = value ? Color.FromHex("2B3D8C") : (new Label()).TextColor;
                LabelIn.FontAttributes = value ? FontAttributes.Bold : FontAttributes.None;
            }
        }
        
    }
}
