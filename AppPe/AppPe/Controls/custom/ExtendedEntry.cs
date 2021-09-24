using Xamarin.Forms;
using Xamarin.HLP.Mobile.AppPE.Controls.xaml;

namespace Xamarin.HLP.Mobile.AppPE.Controls.custom
{
    public class ExtendedEntry : Entry
    {
        public ExtendedEntry()
        {
            TextChanged += (sender, args) =>
            {
                if (isEmail)
                {
                    FoneEmailControl.ValidaEmail(this);
                }

                var xtext = Text ?? "";
                if (xtext.Length <= MaxLength) return;
                xtext = xtext.Remove(xtext.Length - 1);
                //xtext = xtext.Substring(1, xtext.Length - 1);
                Text = xtext;


               
            };
        }


        public bool isEmail { get; set; }


        /// <summary>
        /// The MaxLength property
        /// </summary>
        public static readonly BindableProperty MaxLengthProperty =
            BindableProperty.Create("MaxLength", typeof(int), typeof(ExtendedEntry), int.MaxValue);

        /// <summary>
        /// Gets or sets the MaxLength
        /// </summary>
        public int MaxLength
        {
            get { return (int)this.GetValue(MaxLengthProperty); }
            set { this.SetValue(MaxLengthProperty, value); }
        }

    }
}
