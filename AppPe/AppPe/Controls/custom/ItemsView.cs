using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace Xamarin.HLP.Mobile.AppPE.Controls.custom
{
    public class ItemsView : Grid
    {
        protected StackLayout PagingStackLayout;
        protected ScrollView ScrollView;
        protected readonly ICommand SelectedCommand;
        protected readonly StackLayout ItemsStackLayout;
        protected Boolean Wait = false;


        private Boolean _isScrollAutomaticInitialized;

        public ItemsView()
        {
            ScrollView = new ScrollView
            {
                Orientation = ScrollOrientation.Horizontal
            };

            ScrollView.Scrolled += ScrollView_Scrolled;

            ItemsStackLayout = new StackLayout
            {
                Orientation = StackOrientation.Horizontal,
                Padding = new Thickness(0),
                Spacing = 0,
                HorizontalOptions = LayoutOptions.FillAndExpand
            };

            ScrollView.Content = ItemsStackLayout;
            Children.Add(ScrollView);

            PagingStackLayout = new StackLayout()
            {
                HorizontalOptions = LayoutOptions.Center,
                Orientation = StackOrientation.Horizontal,
                VerticalOptions = LayoutOptions.End,
                Padding = Device.OnPlatform<Thickness>(
                new Thickness(0, 0, 0, 36),
                new Thickness(0, 0, 0, 36),
                new Thickness(0, 0, 0, 60)),
                Opacity = 0.5
            };
            Children.Add(PagingStackLayout);

            var leftArrow = new Image()
            {
                // Replace with your own arrow image
                Source = ImageSource.FromResource("Xamarin.HLP.Mobile.AppPE.Images.Carousel.arrow-left.png"),
                HorizontalOptions = LayoutOptions.Start,
                VerticalOptions = LayoutOptions.Center,
                WidthRequest = 50,
            };
            leftArrow.GestureRecognizers.Add(new TapGestureRecognizer
            {
                Command = new Command(async () =>
                {
                    this.ActualElementIndex--;
                    Wait = true;
                    await ScrollToActualAsync();
                })
            });
            Children.Add(leftArrow);

            var rightArrow = new Image()
            {
                // Replace with your own arrow image
                Source = ImageSource.FromResource("Xamarin.HLP.Mobile.AppPE.Images.Carousel.arrow-right.png"),
                HorizontalOptions = LayoutOptions.End,
                VerticalOptions = LayoutOptions.Center,
                WidthRequest = 50
            };
            rightArrow.GestureRecognizers.Add(new TapGestureRecognizer
            {
                Command = new Command(async () =>
                {
                    this.ActualElementIndex++;
                    Wait = true;
                    await ScrollToActualAsync();
                })
            });
            Children.Add(rightArrow);

            SelectedCommand = new Command<object>(item =>
            {
                var selectable = item as ISelectable;
                if (selectable == null) return;

                SetSelected(selectable);
                SelectedItem = selectable.IsSelected ? selectable : null;
            });

            PropertyChanged += (sender, e) =>
            {
                if (e.PropertyName == "Orientation")
                {
                    ItemsStackLayout.Orientation = ScrollView.Orientation == ScrollOrientation.Horizontal ? StackOrientation.Horizontal : StackOrientation.Vertical;
                }

            };
        }


        public int ItemsCount
        {
            get { return this.ItemsStackLayout.Children.Count; }
        }

        protected virtual void SetSelected(ISelectable selectable)
        {
            selectable.IsSelected = true;
        }

        public Forms.View ActualElement
        {
            get
            {
                return ItemsStackLayout.Children[ActualElementIndex];
            }
        }
        public int ActualElementIndex { get; set; }

        public bool ScrollToStartOnSelected { get; set; }

        public event EventHandler SelectedItemChanged;

        public static readonly BindableProperty ItemsSourceProperty =
            BindableProperty.Create<ItemsView, IEnumerable>(p => p.ItemsSource, default(IEnumerable<object>), BindingMode.TwoWay, null);

        public IEnumerable ItemsSource
        {
            get { return (IEnumerable)GetValue(ItemsSourceProperty); }
            set { SetValue(ItemsSourceProperty, value); }
        }

        public static readonly BindableProperty SelectedItemProperty =
            BindableProperty.Create<ItemsView, object>(p => p.SelectedItem, default(object), BindingMode.TwoWay, null, OnSelectedItemChanged);

        public object SelectedItem
        {
            get { return (object)GetValue(SelectedItemProperty); }
            set { SetValue(SelectedItemProperty, value); }
        }

        public static readonly BindableProperty ItemTemplateProperty =
            BindableProperty.Create<ItemsView, DataTemplate>(p => p.ItemTemplate, default(DataTemplate));

        public DataTemplate ItemTemplate
        {
            get { return (DataTemplate)GetValue(ItemTemplateProperty); }
            set { SetValue(ItemTemplateProperty, value); }
        }

        //private static void ItemsSourceChanged(BindableObject bindable, IEnumerable oldValue, IEnumerable newValue)
        //{
        //    var itemsLayout = (ItemsView)bindable;
        //    itemsLayout.SetItems();
        //    itemsLayout.SetPagination();
        //    itemsLayout.ScrollAutomaticAsync();
        //}

        public void Clear()
        {
            try
            {
                ItemsStackLayout.Children.Clear();
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public void AddItemToSource(object item)
        {
            try
            {
                if (item == null)
                    return;

                ItemsStackLayout.Children.Add(GetItemView(item));
                SetPagination();
            }
            catch (Exception ex)
            {

            }
        }


        public void NotifyPropertyChangedSource()
        {
            if (((IList)ItemsSource)?.Count > ItemsStackLayout.Children.Count)
            {
                foreach (var item in ItemsSource)
                {
                    if (ItemsStackLayout.Children.All(c => (c.BindingContext) != item))
                    {
                        ItemsStackLayout.Children.Add(GetItemView(item));
                    }
                }

            }
        }

        protected virtual void SetItems()
        {
            ItemsStackLayout.Children.Clear();

            if (ItemsSource == null)
                return;

            foreach (var item in ItemsSource)
                ItemsStackLayout.Children.Add(GetItemView(item));

            SelectedItem = ItemsSource.OfType<ISelectable>().FirstOrDefault(x => x.IsSelected);
        }

        protected virtual Forms.View GetItemView(object item)
        {
            var content = ItemTemplate.CreateContent();
            var view = content as Forms.View;
            if (view == null) return null;

            view.BindingContext = item;

            var gesture = new TapGestureRecognizer
            {
                Command = SelectedCommand,
                CommandParameter = item
            };

            AddGesture(view, gesture);

            return view;
        }

        protected void AddGesture(Forms.View view, TapGestureRecognizer gesture)
        {
            view.GestureRecognizers.Add(gesture);

            var layout = view as Layout<Forms.View>;

            if (layout == null)
                return;

            foreach (var child in layout.Children)
                AddGesture(child, gesture);
        }

        private static void OnSelectedItemChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var itemsView = (ItemsView)bindable;
            if (newValue == oldValue)
                return;

            var selectable = newValue as ISelectable;
            itemsView.SetSelectedItem(selectable ?? oldValue as ISelectable);
        }

        protected virtual void SetSelectedItem(ISelectable selectedItem)
        {
            var items = ItemsSource;

            foreach (var item in items.OfType<ISelectable>())
                item.IsSelected = selectedItem != null && item == selectedItem && selectedItem.IsSelected;

            var handler = SelectedItemChanged;
            if (handler != null)
                handler(this, EventArgs.Empty);
        }

        protected virtual async void ScrollAutomaticAsync()
        {
            while (!_isScrollAutomaticInitialized)
            {
                _isScrollAutomaticInitialized = true;
                if (Wait)
                {
                    Wait = false;
                    await Task.Delay(5000);
                }
                SetActivePage();
                await Task.Delay(5000);
                this.ActualElementIndex++;
                await ScrollToActualAsync();
                _isScrollAutomaticInitialized = false;

            }
        }

        private async Task ScrollToActualAsync()
        {
            if (this.ActualElementIndex == this.ItemsCount)
                this.ActualElementIndex = 0;

            if (this.ActualElementIndex < 0)
                this.ActualElementIndex = 0;

            try
            {
                await this.ScrollView.ScrollToAsync(this.ActualElement.X, 0, false);
            }
            catch
            {
                //invalid scroll: sometimes happen
            }
        }

        protected virtual void SetPagination()
        {
            this.ActualElementIndex = 0;
            PagingStackLayout.Children.Clear();
            for (int i = 0; i < this.ItemsCount; i++)
            {
                var view = new BoxView() { BackgroundColor = Color.White, WidthRequest = 10, HeightRequest = 10 };
                PagingStackLayout.Children.Add(view);
            }
        }

        protected virtual void SetActivePage()
        {
            try
            {
                for (int i = 0; i < this.ItemsCount; i++)
                {
                    (PagingStackLayout.Children[i] as BoxView).BackgroundColor = Color.White;
                }
                (PagingStackLayout.Children[this.ActualElementIndex] as BoxView).BackgroundColor = Color.Red;

            }
            catch { }
        }

        void ScrollView_Scrolled(object sender, ScrolledEventArgs e)
        {
            try
            {
                if (this.IsVisible)
                {
                    if (e.ScrollX % ItemsStackLayout.Children.First().Width != 0 && !Wait)
                        Wait = true;

                    for (int i = 1; i < this.ItemsCount; i++)
                    {
                        var previousItemX = ItemsStackLayout.Children[i - 1].X;
                        var actualItemX = i == this.ItemsCount ? this.ItemsCount : ItemsStackLayout.Children[i].X;

                        if (e.ScrollX >= previousItemX && e.ScrollX <= actualItemX)
                        {
                            this.ActualElementIndex = e.ScrollX == actualItemX ? i : i - 1;
                            SetActivePage();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ex.TrakException();
            }
        }
    }

    public interface ISelectable
    {
        bool IsSelected { get; set; }

        ICommand SelectCommand { get; set; }
    }
}
