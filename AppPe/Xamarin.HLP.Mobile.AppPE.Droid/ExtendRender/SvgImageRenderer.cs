using System;
using System.IO;
using System.Threading.Tasks;
using Android.Runtime;
using Android.Widget;
using NGraphics.Android.Custom;
using NGraphics.Custom.Parsers;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using Xamarin.HLP.Mobile.AppPE.Abstractions;
using Xamarin.HLP.Mobile.AppPE.Droid.ExtendRender;

[assembly: ExportRenderer(typeof(SvgImage), typeof(SvgImageRenderer))]
namespace Xamarin.HLP.Mobile.AppPE.Droid.ExtendRender
{
    [Preserve(AllMembers = true)]
    public class SvgImageRenderer : ViewRenderer<SvgImage, ImageView>
    {
        public static void Init()
        {
            var temp = DateTime.Now;
        }

        private SvgImage _formsControl
        {
            get
            {
                return Element as SvgImage;
            }
        }

        protected override async void OnElementChanged(ElementChangedEventArgs<SvgImage> e)
        {
            base.OnElementChanged(e);

            if (_formsControl != null)
            {
                if (!string.IsNullOrEmpty(_formsControl.SvgPath))
                {
                    await Task.Run(async () =>
                    {
                        var svgStream = _formsControl.SvgAssembly.GetManifestResourceStream(_formsControl.SvgPath);

                        if (svgStream == null)
                        {
                            throw new Exception(
                                string.Format("Error retrieving {0} make sure Build Action is Embedded Resource",
                                    _formsControl.SvgPath));
                        }

                        var r = new SvgReader(new StreamReader(svgStream), new StylesParser(new ValuesParser()),
                            new ValuesParser());

                        var graphics = r.Graphic;

                        var width =
                            PixelToDP((int)_formsControl.WidthRequest <= 0 ? 100 : (int)_formsControl.WidthRequest);
                        var height =
                            PixelToDP((int)_formsControl.HeightRequest <= 0 ? 100 : (int)_formsControl.HeightRequest);

                        var scale = 1.0;

                        if (height >= width)
                        {
                            scale = height / graphics.Size.Height;
                        }
                        else
                        {
                            scale = width / graphics.Size.Width;
                        }

                        var canvas = new AndroidPlatform().CreateImageCanvas(graphics.Size, scale);
                        graphics.Draw(canvas);
                        var image = (BitmapImage)canvas.GetImage();

                        return image;
                    }).ContinueWith(taskResult =>
                    {
                        Device.BeginInvokeOnMainThread(() =>
                        {
                            try
                            {


                                var imageView = new ImageView(Context);

                                imageView.SetScaleType(ImageView.ScaleType.FitXy);
                                imageView.SetImageBitmap(taskResult.Result.Bitmap);

                                SetNativeControl(imageView);
                            }
                            catch (Exception ex)
                            {
                                throw ex;
                            }
                        });

                    });
                }
            }
        }

        public override SizeRequest GetDesiredSize(int widthConstraint, int heightConstraint)
        {
            return new SizeRequest(new Size(_formsControl.WidthRequest, _formsControl.WidthRequest));
        }

        /// <summary>
        /// http://stackoverflow.com/questions/24465513/how-to-get-detect-screen-size-in-xamarin-forms
        /// </summary>
        /// <param name="pixel"></param>
        /// <returns></returns>
        private int PixelToDP(int pixel)
        {
            var scale = Resources.DisplayMetrics.Density;
            return (int)((pixel * scale) + 0.5f);
        }
    }
}