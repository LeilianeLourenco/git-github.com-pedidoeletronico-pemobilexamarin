using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;
using Xamarin.HLP.Mobile.AppPE.Common;
using Xamarin.HLP.Mobile.AppPE.Controls.custom;
using ImageButton = Xamarin.HLP.Mobile.AppPE.Controls.custom.ImageButton;

namespace Xamarin.HLP.Mobile.AppPE.Controls.Custom
{
    public class GridButton : Grid
    {
        public static BindableProperty CommandProperty =
            BindableProperty.Create<ImageButton, ICommand>(bp => bp.Command, default(ICommand));


        public ICommand Command
        {
            get { return (ICommand)this.GetValue(CommandProperty); }
            set { this.SetValue(CommandProperty, value); }
        }

        public object CommandParameter
        {
            get { return GetValue(CommandParameterProperty); }
            set { SetValue(CommandParameterProperty, value); }
        }


        public static readonly BindableProperty CommandParameterProperty =
          BindableProperty.Create<GridButton, object>(o => o.CommandParameter, null);



        public GridButton()
        {
            Initialize();
        }


        public void Initialize()
        {
            GestureRecognizers.Add(new TapGestureRecognizer()
            {
                Command = TransitionCommand
            });
        }


        public bool _bCanExceute { get; set; } = true;

        private ICommand TransitionCommand
        {
            get
            {
                return new Command(async () =>
                {
                    if (Command != null)
                    {
                        if (_bCanExceute)
                        {
                            AnchorX = 0.48;
                            AnchorY = 0.48;
                            await this.ScaleTo(0.95, 50, Easing.Linear);
                            await Task.Delay(100);
                            await this.ScaleTo(1, 50, Easing.Linear);
                            Command.Execute(CommandParameter);
                            Device.StartTimer(UtilMethods.GetStartTime, ChangeCanExecute);
                        }
                    }
                });
            }
        }

        private bool ChangeCanExecute()
        {
            _bCanExceute = true;
            return false;
        }
    }
}
