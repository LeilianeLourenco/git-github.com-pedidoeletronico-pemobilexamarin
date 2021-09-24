using System;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using Xamarin.Forms;
using Xamarin.HLP.Mobile.AppPE.Annotations;
using Xamarin.HLP.Mobile.AppPE.Common;
using Xamarin.HLP.Mobile.AppPE.Controls.custom;
using Xamarin.HLP.Mobile.AppPE.Model;
using Xamarin.HLP.Mobile.AppPE.View.Converter.Generic;

namespace Xamarin.HLP.Mobile.AppPE.Controls.xaml
{
    public partial class FoneEmailControl : Grid, INotifyPropertyChanged
    {
        public FoneEmailControl()
        {
            InitializeComponent();

            //StackLabel.Padding = Device.OnPlatform(new Thickness(0, 0, 0, 0), new Thickness(0, -1, 0, 0),
            //            new Thickness(0, -4, 0, 0));

        }

        public enum TipoCompoenent
        {
            EMAIL,
            TELEFONE
        }

        public string InputField
        {
            get
            {
                var inputs = "";
                foreach (Forms.View child in this.Inputs.Children)
                {
                    if (child.GetType() == typeof(ExtendedEntry))
                    {
                        var entry = child as ExtendedEntry;

                        if (tipoCompoenent == TipoCompoenent.EMAIL)
                        {
                            if (entry != null && entry.Text.IsValidEmailAddress())
                            {
                                inputs += entry.Text + ",";
                            }
                        }
                        else
                        {
                            if (entry != null) inputs += entry.Text + ",";
                        }
                    }
                }
                return inputs;
            }
            set { SetValue(InputFieldProperty, value); }
        }

        // Using a DependencyProperty as the backing store for InputField.  This enables animation, styling, binding, etc...
        public static readonly BindableProperty InputFieldProperty =
            BindableProperty.Create<FoneEmailControl, string>(o => o.InputField, string.Empty, propertyChanged: OnItemsSourceChanged);


        private static void OnItemsSourceChanged(BindableObject bindable, string oldvalue, string newvalue)
        {
            var ctrl = bindable as FoneEmailControl;

            if (ctrl == null) return;
            ctrl.Inputs.Children.Clear();
            if (!string.IsNullOrEmpty(newvalue))
                foreach (var item in newvalue.Split(',').Where(item => item != ""))
                {
                    if (ctrl.tipoCompoenent == TipoCompoenent.EMAIL)
                    {
                        if (item.IsValidEmailAddress())
                            ctrl.AddEntry(item);
                    }
                    else
                    {
                        ctrl.AddEntry(item);
                    }

                }
            ctrl.AddEntry();
        }


        private TipoCompoenent _tipoCompoenent;

        public TipoCompoenent tipoCompoenent
        {
            get { return _tipoCompoenent; }
            set
            {
                _tipoCompoenent = value; NotifyPropertyChanged();
                if (value == TipoCompoenent.EMAIL)
                {
                    LabelTitle.Text = "Email (em vermelho, não será salvo)";
                    //ImageHeader.SvgPath = "Xamarin.HLP.Mobile.AppPE.Images.PagesIcon.ApplicationEmail.svg";

                }
                else if (value == TipoCompoenent.TELEFONE)
                {
                    LabelTitle.Text = "Telefone";
                    //ImageHeader.SvgPath = "Xamarin.HLP.Mobile.AppPE.Images.PagesIcon.ApplicationPhone.svg";
                }
                AddEntry();
            }
        }

        public static void ValidaEmail(ExtendedEntry entry)
        {
            entry.TextColor = !entry.Text.IsValidEmailAddress() ? ColorStaticModel.VermelhoPrincipal : ColorStaticModel.Preto;
        }

        private void Entry_OnTextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                var entry = sender as ExtendedEntry;
                if (entry?.BindingContext.GetType() == typeof(FoneModel))
                    FormatarPhone(entry);

                // validação de email
                if (entry != null && entry.BindingContext.GetType() != typeof(FoneModel))
                    ValidaEmail(entry);


                if (string.IsNullOrEmpty(e.OldTextValue) && e.NewTextValue != "")
                {
                    AddEntry();
                }
                entry?.Focus();

                if (e.NewTextValue != "" || (string.IsNullOrEmpty(e.OldTextValue))) return;
                if (Inputs.Children.Count() <= 1) return;
                if (entry == null) return;
                entry.IsVisible = false;
                //Device.OnPlatform(Android: SetTamanhoStackLayout);
                // SetTamanhoStackLayout();
                var control = Inputs.Children.FirstOrDefault(c =>
                {
                    var entry1 = c as ExtendedEntry;
                    return entry1 != null && (c.IsVisible && string.IsNullOrEmpty(entry1.Text));
                });
                control.Focus();
            }
            catch (Exception ex)
            {
                GoogleInsightsReportingConstants.TrakException("FoneEmailControl.Entry_OnTextChanged", ex.Message, true);
            }
        }

        public static void FormatarPhone(ExtendedEntry entry)
        {


            if ((entry.Text ?? "").Length <= 15)
            {
                var valor = System.Convert.ToString((entry.Text ?? ""));
                valor = valor.Replace("-", "")
                    .Replace("(", "")
                    .Replace(")", "")
                    .Replace(".", "")
                    .Replace(" ", "");

                var fone = valor.ToPhoneFormat();

                if (entry.Text != fone)
                {
                    entry.Text = fone;
                }
            }

        }

        private void AddEntry(string value = "")
        {
            var newEntry = new ExtendedEntry
            {
                Placeholder = tipoCompoenent == TipoCompoenent.EMAIL ? "novo email" : "novo telefone",
                MaxLength = tipoCompoenent == TipoCompoenent.EMAIL ? 150 : 15,
                Keyboard = tipoCompoenent == TipoCompoenent.EMAIL ? Keyboard.Email : Keyboard.Telephone,
                Text = value
            };

            if (tipoCompoenent == TipoCompoenent.TELEFONE)
            {
                var fone = new FoneModel();
                newEntry.BindingContext = fone;
                newEntry.SetBinding(Entry.TextProperty, "Display", BindingMode.TwoWay, new PhoneNumberConverter());
                fone.Display = value;
            }
            Inputs.Children.Add(newEntry);
            newEntry.TextChanged += Entry_OnTextChanged;
            newEntry.Completed += newEntry_Completed;

            //  SetTamanhoStackLayout();
        }

        void SetTamanhoStackLayout()
        {
            var iCount = Inputs.Children.Count(c => c.IsVisible);
            var height = 74;
            var param = 80;
            if (Device.OS == TargetPlatform.Android || Device.OS == TargetPlatform.iOS)
            {
                height = 39;
                param = 45;
            }
            Inputs.HeightRequest = height + ((iCount - 1) * param);
        }

        private void newEntry_Completed(object sender, EventArgs e)
        {
            var entry = sender as ExtendedEntry;
            if (string.IsNullOrEmpty(entry?.Text)) return;

            var entryFocus = Inputs.Children.FirstOrDefault(c =>
            {
                var entry1 = c as ExtendedEntry;
                return entry1 != null && string.IsNullOrEmpty(entry1.Text);
            });

            entryFocus?.Focus();
        }




        private void Button_OnClicked(object sender, EventArgs e)
        {
            Inputs.Children.Add(new ExtendedEntry());
        }



        public event PropertyChangedEventHandler PropertyChanged;
        [NotifyPropertyChangedInvocator]
        protected virtual void NotifyPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }


    public class FoneModel : ModelComum
    {
        private string _display;

        public string Display
        {
            get { return _display; }
            set
            {
                _display = value;
                NotifyPropertyChanged();
            }
        }

    }
}
