using FFImageLoading.Forms;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace Xamarin.HLP.Mobile.AppPE.Controls.custom
{
    public class CustomCell : ViewCell
	{
		readonly CachedImage cachedImage = null;

		public CustomCell()
		{
			cachedImage = new CachedImage();
			View = cachedImage;
		}

		//protected override void OnBindingContextChanged()
		//{
		//	// you can also put cachedImage.Source = null; here to prevent showing old images occasionally
		//	cachedImage.Source = null;
		//	var item = BindingContext as ;

		//	if (item == null)
		//	{
		//		return;
		//	}

		//	cachedImage.Source = item;

		//	base.OnBindingContextChanged();
		//}
	}
}
