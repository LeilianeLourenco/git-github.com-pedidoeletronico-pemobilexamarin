using System.Windows.Input;
using Xamarin.Forms;

namespace Xamarin.HLP.Mobile.AppPE.Controls.custom
{
    public class ImageButton : Image
    {
        public static BindableProperty CommandProperty =
            BindableProperty.Create<ImageButton, ICommand>(bp => bp.Command, default(ICommand));


        public ICommand Command
        {
            get { return (ICommand)this.GetValue(CommandProperty); }
            set { this.SetValue(CommandProperty, value); }
        }


        public ImageButton()
        {
            var gesto = new TapGestureRecognizer();

            gesto.Tapped += (sender, e) =>
            {
                if (Command != null && Command.CanExecute(CommandParameter))
                    Command.Execute(CommandParameter);
            };
            GestureRecognizers.Add(gesto);
        }



        public object CommandParameter
        {
            get { return GetValue(CommandParameterProperty); }
            set { SetValue(CommandParameterProperty, value); }
        }


        public static readonly BindableProperty CommandParameterProperty =
          BindableProperty.Create<ImageButton, object>(o => o.CommandParameter, null);
    }
}
