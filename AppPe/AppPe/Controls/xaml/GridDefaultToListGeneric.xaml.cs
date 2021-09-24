using Xamarin.Forms;

namespace Xamarin.HLP.Mobile.AppPE.Controls.xaml
{
    public partial class GridDefaultToListGeneric : Grid
    {
        public GridDefaultToListGeneric()
        {
            InitializeComponent();
        }


        public StackLayout getNenhumRegistro => NenhumRegistroStackLayout;

        public static readonly BindableProperty IsRunningProperty = BindableProperty.Create<GridDefaultToListGeneric, bool>(p => p.IsRunning, false, propertyChanged: OnChangeRunninValue);
        public bool IsRunning
        {
            get
            {
                return (bool)GetValue(IsRunningProperty);
            }
            set
            {
                SetValue(IsRunningProperty, value);
            }
        }

        public static readonly BindableProperty IsVisibleListProperty = BindableProperty.Create<GridDefaultToListGeneric, bool>(p => p.IsVisibleList, false, propertyChanged: OnChangeListVisibleValue);
        public bool IsVisibleList
        {
            get
            {
                return (bool)GetValue(IsVisibleListProperty);
            }
            set
            {
                SetValue(IsVisibleListProperty, value);
            }
        }

        private static void OnChangeRunninValue(BindableObject bindable, bool oldvalue, bool newvalue)
        {
            var ctrl = bindable as GridDefaultToListGeneric;
            if (ctrl != null)
            {
                ctrl.PesquisandoStackLayout.IsVisible = newvalue;
                if (newvalue)
                    ctrl.NenhumRegistroStackLayout.IsVisible = false;
            }
        }

        private static void OnChangeListVisibleValue(BindableObject bindable, bool oldvalue, bool newvalue)
        {
            var ctrl = bindable as GridDefaultToListGeneric;
            if (ctrl != null)
            {
                ctrl.NenhumRegistroStackLayout.IsVisible = false;

                if (!newvalue) // se  a listview não estiver visivel.
                {
                    ctrl.NenhumRegistroStackLayout.IsVisible = true;
                }
            }

        }
    }
}
