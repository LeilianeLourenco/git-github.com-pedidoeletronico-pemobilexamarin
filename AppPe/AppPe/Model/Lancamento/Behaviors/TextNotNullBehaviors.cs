using Xamarin.Forms;

namespace Xamarin.HLP.Mobile.AppPE.Model.Lancamento.Behaviors
{
    public class TextNotNullBehaviors : Behavior<Entry>
    {
        static readonly BindablePropertyKey IsValidPropertyKey = BindableProperty.CreateReadOnly("IsValid", typeof(bool), typeof(EmailBehaviors), false);
        public static readonly BindableProperty IsValidProperty = IsValidPropertyKey.BindableProperty;

        public bool IsValid
        {
            get { return (bool)base.GetValue(IsValidProperty); }
            private set { base.SetValue(IsValidPropertyKey, value); }
        }

        protected override void OnAttachedTo(Entry bindable)
        {
            bindable.TextChanged += bindable_TextChanged;
        }

        private async void bindable_TextChanged(object sender, TextChangedEventArgs e)
        {
            var entry = sender as Entry;
            if (entry != null)
            {
                IsValid = !string.IsNullOrEmpty(entry.Text ) && entry.Text.Length >= 6;
            }

        }

        protected override void OnDetachingFrom(Entry bindable)
        {
            bindable.TextChanged -= bindable_TextChanged;
        }
    }
}
