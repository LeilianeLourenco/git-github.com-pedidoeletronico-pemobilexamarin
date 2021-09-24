using System;
using Xamarin.Forms;

namespace Xamarin.HLP.Mobile.AppPE.Model.Lancamento.Behaviors
{
    public class EmailBehaviors : Behavior<Entry>
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
            //   bindable.Completed += bindable_Completed;
            bindable.TextChanged += bindable_TextChanged;
        }

        void bindable_TextChanged(object sender, TextChangedEventArgs e)
        {
            var entry = sender as Entry;
            if (entry != null && entry.Text != "" && entry.Text.Contains("@"))
            {
                IsValid = entry.Text == "" || entry.Text.IsValidEmailAddress();
            }
        }

        void bindable_Completed(object sender, EventArgs e)
        {
            var entry = sender as Entry;
            if (entry != null)
            {
                IsValid = entry.Text == "" || entry.Text.IsValidEmailAddress();
            }
        }


        protected override void OnDetachingFrom(Entry bindable)
        {
            bindable.Completed -= bindable_Completed;
        }



    }
}
