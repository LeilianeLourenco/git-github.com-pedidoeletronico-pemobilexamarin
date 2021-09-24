using System;
using System.Windows.Input;
using Xamarin.Forms;

namespace Xamarin.HLP.Mobile.AppPE.Controls.xaml
{
    public partial class StepperDefault : Grid
    {
        public double Valor
        {
            get { return (double)GetValue(ValorProperty); }
            set { SetValue(ValorProperty, value); }
        }
        public static readonly BindableProperty ValorProperty = BindableProperty.Create<StepperDefault, double>(o => o.Valor, 0);

        public ICommand Command
        {
            get { return (ICommand)this.GetValue(CommandProperty); }
            set { this.SetValue(CommandProperty, value); }
        }
        public static BindableProperty CommandProperty = BindableProperty.Create<StepperDefault, ICommand>(bp => bp.Command, default(ICommand));



        public StepperDefault()
        {
            InitializeComponent();
        }

        private void ValueChanged_OnClicked(object sender, EventArgs e)
        {
            var btn = sender as Button;
            if (btn == btnMais)
            {
                Valor += 1;
            }
            else
            {
                if (Valor > 0)
                {
                    Valor -= 1;
                }
            }
            Command?.Execute(null);
        }



    }
}
