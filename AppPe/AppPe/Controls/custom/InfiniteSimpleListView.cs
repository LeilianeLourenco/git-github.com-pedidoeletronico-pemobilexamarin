using System;
using System.Collections;
using System.Windows.Input;
using Xamarin.Forms;

namespace Xamarin.HLP.Mobile.AppPE.Controls.custom
{
    public class InfiniteSimpleListView : ListView
    {
        /// <summary>
        /// Respresents the command that is fired to ask the view model to load additional data bound collection.
        /// </summary>
        public static readonly BindableProperty LoadMoreCommandProperty = BindableProperty.Create<InfiniteSimpleListView, ICommand>(bp => bp.LoadMoreCommand, default(ICommand));
         

        /// <summary>
        /// Gets or sets the command binding that is called whenever the listview is getting near the bottomn of the list, and therefore requiress more data to be loaded.
        /// </summary>
        public ICommand LoadMoreCommand
        {
            get { return (ICommand)GetValue(LoadMoreCommandProperty); }
            set { SetValue(LoadMoreCommandProperty, value); }
        }

        /// <summary>
        /// Creates a new instance of a <see cref="InfiniteSimpleListView" />
        /// </summary>
        public InfiniteSimpleListView()
        {
            Init();
        }

        public InfiniteSimpleListView(ListViewCachingStrategy strategy) : base(strategy)
        {
            Init();
        }


        public void Init()
        {
            try
            {
                if (Device.RuntimePlatform != Device.iOS)
                {
                    this.SetBinding(IsRefreshingProperty, "IsBusy");
                }
                ItemAppearing += InfiniteSimpleListView_ItemAppearing;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        void InfiniteSimpleListView_ItemAppearing(object sender, ItemVisibilityEventArgs e)
        {
            try
            {
                var items = ItemsSource as IList;

                if (items != null && e.Item == items[items.Count - 1])
                {
                    if (LoadMoreCommand != null && LoadMoreCommand.CanExecute(null))
                        LoadMoreCommand.Execute(null);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
    }
}
