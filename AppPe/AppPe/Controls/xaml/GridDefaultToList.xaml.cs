using System.Collections;
using Xamarin.Forms;

namespace Xamarin.HLP.Mobile.AppPE.Controls.xaml
{
    public partial class GridDefaultToList : Grid
    {
        public GridDefaultToList()
        {
            InitializeComponent();
        }

        public static readonly BindableProperty IsRunningProperty = BindableProperty.Create<GridDefaultToList, bool>(p => p.IsRunning, false, propertyChanged: OnChangeValue);
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

        public IEnumerable ItemsSource
        {
            get { return (IEnumerable)GetValue(ItemsSourceProperty); }
            set { SetValue(ItemsSourceProperty, value); }
        }

        public static BindableProperty ItemsSourceProperty =
           BindableProperty.Create<GridDefaultToList, IEnumerable>(o => o.ItemsSource, default(IEnumerable));

        private static void OnChangeValue(BindableObject bindable, bool oldvalue, bool newvalue)
        {
            var ctrl = bindable as GridDefaultToList;

            if (ctrl == null) return;
            ctrl.PesquisandoStackLayout.IsVisible = false;
            ctrl.NenhumRegistroStackLayout.IsVisible = false;
            if (newvalue)
            {
                ctrl.PesquisandoStackLayout.IsVisible = true;
            }
            else
            {
                if (ctrl.ItemsSource == null)
                    ctrl.NenhumRegistroStackLayout.IsVisible = true;
                var list = ctrl.ItemsSource as IList;
                if (list != null && list.Count == 0)
                    ctrl.NenhumRegistroStackLayout.IsVisible = true;
            }
        }
    }
}
