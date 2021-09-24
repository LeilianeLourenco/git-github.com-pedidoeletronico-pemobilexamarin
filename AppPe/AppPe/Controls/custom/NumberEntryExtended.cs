using Xamarin.Forms;

namespace Xamarin.HLP.Mobile.AppPE.Controls.custom
{
    public class NumberEntryExtended : Entry
    {
        public NumberEntryExtended()
        {
            this.Keyboard = Keyboard.Numeric;

            TextChanged += (sender, args) =>
            {
                var xtext = Text.RetiraCaracterEspecial();
                if (xtext.Length <= MaxLength) return;
                xtext = xtext.Substring(0, MaxLength - 1);
                Text = xtext;
            };
        }

        /// <summary>
        /// The MaxLength property
        /// </summary>
        public static readonly BindableProperty MaxLengthProperty =
            BindableProperty.Create("MaxLength", typeof(int), typeof(NumberEntryExtended), int.MaxValue);

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
