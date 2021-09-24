using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using FFImageLoading.Transformations;
using FFImageLoading.Work;
using Xamarin.Forms;
using Xamarin.HLP.Mobile.AppPE.Common;
using Xamarin.HLP.Mobile.AppPE.View.Popup;
using ImageSource = Xamarin.Forms.ImageSource;

namespace Xamarin.HLP.Mobile.AppPE.Controls.xaml
{
    public partial class MyCardView : Grid
    {


        public static readonly BindableProperty ShowImageProperty =
            BindableProperty.Create<MyCardView, bool?>(p => p.ShowImage, null);

        public bool? ShowImage
        {
            get { return (bool?)GetValue(ShowImageProperty); }
            set { SetValue(ShowImageProperty, value); }
        }



        public static readonly BindableProperty IsCardViewProperty =
            BindableProperty.Create<MyCardView, bool?>(p => p.IsCardView, null, propertyChanged: OnChangeValue);

        public bool? IsCardView
        {
            get { return (bool?)GetValue(IsCardViewProperty); }
            set { SetValue(IsCardViewProperty, value); }
        }

        private static void OnChangeValue(BindableObject bindable, bool? oldvalue, bool? newvalue)
        {
            var controle = bindable as MyCardView;
            if (newvalue != null && controle != null)
            {
                controle.GridListItem.IsVisible = controle.GridCard.IsVisible = false;
                if (controle.IsCardView ?? true)
                    controle.GridCard.IsVisible = true;
                else
                    controle.GridListItem.IsVisible = true;
            }

        }



        public ImageSource CardImage
        {
            get { return (ImageSource)GetValue(CardImageProperty); }
            set { SetValue(CardImageProperty, value); }
        }

        public static readonly BindableProperty CardImageProperty = BindableProperty.Create(propertyName: "CardImage",
            returnType: typeof(ImageSource),
            declaringType: typeof(MyCardView),
            defaultValue: default(ImageSource),
            defaultBindingMode: BindingMode.TwoWay,
            validateValue: null,
            propertyChanged: (bindable, oldvalue, newvalue) =>
            {
                try
                {
                    var controle = bindable as MyCardView;
                    if (newvalue != null && controle != null)
                        if (controle.IsCardView ?? true)
                            controle.ImgCard1.Source = newvalue as ImageSource;
                        else
                            controle.ImgCard2.Source = newvalue as ImageSource;
                }
                catch (Exception)
                {
                }
            });


        public string Title
        {
            get { return (string)GetValue(TitleProperty); }
            set { SetValue(TitleProperty, value); }
        }

        public static readonly BindableProperty TitleProperty = BindableProperty.Create(propertyName: "Title",
           returnType: typeof(string),
           declaringType: typeof(MyCardView),
           defaultValue: "",
           defaultBindingMode: BindingMode.TwoWay,
           validateValue: null,
           propertyChanged: (bindable, oldvalue, newvalue) =>
           {
               var controle = bindable as MyCardView;
               if (newvalue != null && controle != null)
                   if (controle.IsCardView ?? true)
                       controle.LabelTitle1.Text = Convert.ToString(newvalue);
                   else
                       controle.LabelTitle2.Text = Convert.ToString(newvalue);
           });

        public string Display
        {
            get { return (string)GetValue(DisplayProperty); }
            set { SetValue(DisplayProperty, value); }
        }

        public static readonly BindableProperty DisplayProperty = BindableProperty.Create(propertyName: "Display",
           returnType: typeof(string),
           declaringType: typeof(MyCardView),
           defaultValue: "",
           defaultBindingMode: BindingMode.TwoWay,
           validateValue: null,
           propertyChanged: (bindable, oldvalue, newvalue) =>
           {
               var controle = bindable as MyCardView;
               if (newvalue != null && controle != null)
                   if (controle.IsCardView ?? true)
                       controle.LabelDisplay1.Text = Convert.ToString(newvalue);
                   else
                       controle.LabelDisplay2.Text = Convert.ToString(newvalue);
           });



     

        public MyCardView()
        {
            InitializeComponent(); 

            ButtonClick.Command = new Command(() =>
            {
                if (ShowImage ?? false)
                {
                    UtilNavidate.ShowPopupNew(new PagePopupImage(CardImage));
                }
            });

            ImgCard1.Transformations = new List<ITransformation> { new CropTransformation(0.5, 0, 0) };
            ImgCard2.Transformations = new List<ITransformation> { new CircleTransformation(), new CropTransformation(1, 0, 0) };

        }
    }
}
