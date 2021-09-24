using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Xamarin.Forms;
using Xamarin.HLP.Mobile.AppPE.Model;

namespace Xamarin.HLP.Mobile.AppPE.Controls.custom
{
    public class BindablePicker : Picker
    {
        public BindablePicker()
        {
            this.SelectedIndexChanged += OnSelectedIndexChanged;
        }

        public static BindableProperty ItemsSourceProperty =
            BindableProperty.Create<BindablePicker, IEnumerable>(o => o.ItemsSource, default(IEnumerable), propertyChanged: OnItemsSourceChanged);

        public IEnumerable ItemsSource
        {
            get { return (IEnumerable)GetValue(ItemsSourceProperty); }
            set { SetValue(ItemsSourceProperty, value); }
        }
        private static void OnItemsSourceChanged(BindableObject bindable, IEnumerable oldvalue, IEnumerable newvalue)
        {
            try
            {


                var picker = bindable as BindablePicker;
                if (picker == null) return;
                if (newvalue == null) return;
                picker.ItemsPickerModel = (newvalue as IEnumerable<BasicPickerModel>).ToList();
                picker.Items.Clear();
                var dictionary = newvalue as List<BasicPickerModel>;
                if (dictionary == null) return;
                foreach (var item in dictionary)
                {
                    picker.Items.Add(item.Display.ToString());
                }
            }
            catch (Exception ex)
            {
                ex.TrakException();
            }
        }

        public static BindableProperty SelectedItemProperty =
            BindableProperty.Create<BindablePicker, BasicPickerModel>(o => o.SelectedItem, default(BasicPickerModel), propertyChanged: OnSelectedItemChanged);



        public BasicPickerModel SelectedItem
        {
            get { return (BasicPickerModel)GetValue(SelectedItemProperty); }
            set { SetValue(SelectedItemProperty, value); }
        }



        private List<BasicPickerModel> ItemsPickerModel { get; set; }

        private void OnSelectedIndexChanged(object sender, EventArgs eventArgs)
        {
            try
            {


                if (SelectedIndex < 0 || SelectedIndex > Items.Count - 1)
                {
                   // SelectedItem = null;
                    //if (ItemsPickerModel != null && ItemsPickerModel.Any())
                    //    SelectedItem = ItemsPickerModel.FirstOrDefault();
                }
                else
                {
                    SelectedItem = ItemsPickerModel.ToList()[SelectedIndex];
                }
            }
            catch (Exception ex)
            {
                ex.TrakException();
            }
        }


        public void OnSelected(BasicPickerModel item)
        {
            try
            {
                SelectedItem = item;
            }
            catch (Exception ex)
            {
                ex.TrakException();
            }
        }

        private static void OnSelectedItemChanged(BindableObject bindable, object oldvalue, object newvalue)
        {
            try
            {
                var picker = bindable as BindablePicker;
                if (newvalue == null) return;
                if (newvalue.GetType() == typeof(BasicPickerModel))
                {
                    var basicPickerModel = newvalue as BasicPickerModel;
                    if (basicPickerModel == null) return;
                    if (picker?.ItemsPickerModel != null)
                    {
                        var objItem = picker.ItemsPickerModel.FirstOrDefault(c => c.Id == basicPickerModel.Id);
                        if (basicPickerModel.XId != null)
                        {
                            objItem = picker.ItemsPickerModel.FirstOrDefault(c => c.XId == basicPickerModel.XId);
                        }
                        if (objItem != null)
                            picker.SelectedIndex = picker.ItemsPickerModel.IndexOf(objItem);
                    }
                }
                else
                {
                    if (picker != null)
                        picker.SelectedIndex = picker.Items.IndexOf(newvalue.ToString());
                }
            }
            catch (Exception ex)
            {
                ex.TrakException();
            }
        }
    }
}
