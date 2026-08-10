using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Xamarin.HLP.Mobile.AppPE.Common
{
    [ContentProperty("Source")]
    public class ImageResourceExtension : IMarkupExtension
    {
        public string Source { get; set; }

        public object ProvideValue(IServiceProvider serviceProvider)
        {
            if (Source == null)
                return null;
            // ImageSource.FromResource(string) usa Assembly.GetCallingAssembly() internamente, que
            // pode resolver o assembly errado em builds AOT (Release) no iOS, fazendo o icone nao
            // aparecer sem nenhum erro. Passar o assembly explicitamente evita essa ambiguidade.
            var imageSource = ImageSource.FromResource(Source, System.Reflection.Assembly.GetExecutingAssembly());
            return imageSource;
        }
    }
}
