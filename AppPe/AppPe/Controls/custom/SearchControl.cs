using Xamarin.Forms;

namespace Xamarin.HLP.Mobile.AppPE.Controls.custom
{
    public class SearchControl : SearchBar
    {
        public SearchControl()
        {
            TextChanged += SearchControl_TextChanged;
        }

        void SearchControl_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (e.NewTextValue != string.Empty) return;
            if (e.OldTextValue == null) return;
            if (e.OldTextValue.Length > 0)
                if (SearchCommand != null)
                    SearchCommand.Execute(null);
        }

    }
}
