using System.Reflection;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;
using Xamarin.HLP.Mobile.AppPE.Common;

namespace Xamarin.HLP.Mobile.AppPE.Abstractions
{
    public class SvgImage : Image 
    {
        /// <summary>
        /// The path to the svg file
        /// </summary>
        public static readonly BindableProperty SvgPathProperty =
          BindableProperty.Create("SvgPath", typeof(string), typeof(SvgImage), default(string));

        /// <summary>
        /// The path to the svg file
        /// </summary>
        public string SvgPath
        {
            get { return (string)GetValue(SvgPathProperty); }
            set { SetValue(SvgPathProperty, value); }
        }

        /// <summary>
        /// The assembly containing the svg file
        /// </summary>
        public static readonly BindableProperty SvgAssemblyProperty =
          BindableProperty.Create("SvgAssembly", typeof(Assembly), typeof(SvgImage), default(Assembly));

        /// <summary>
        /// The assembly containing the svg file
        /// </summary>
        public Assembly SvgAssembly
        {
            get { return (Assembly)GetValue(SvgAssemblyProperty); }
            set { SetValue(SvgAssemblyProperty, value); }
        }

        public static BindableProperty CommandProperty =
          BindableProperty.Create<SvgImage, ICommand>(bp => bp.Command, default(ICommand));

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
          BindableProperty.Create<SvgImage, object>(o => o.CommandParameter, null);


        public SvgImage()
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
                            _bCanExceute = false;
                            var v = Parent as Forms.View;
                            if (v != null)
                            {
                                v.AnchorX = 0.48;
                                v.AnchorY = 0.48;
                                await v.ScaleTo(0.8, 50, Easing.Linear);
                                await Task.Delay(100);
                                await v.ScaleTo(1, 50, Easing.Linear);
                            }
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
