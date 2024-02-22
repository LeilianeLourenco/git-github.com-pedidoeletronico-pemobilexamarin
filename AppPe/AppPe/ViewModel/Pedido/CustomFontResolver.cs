using PdfSharpCore.Fonts;
using System.IO;

public class CustomFontResolver : IFontResolver
{
    private readonly byte[] _fontData;

    public CustomFontResolver(byte[] fontData)
    {
        _fontData = fontData;
    }

    public string DefaultFontName => throw new System.NotImplementedException();

    public byte[] GetFont(string faceName)
    {
        return _fontData;
    }

    public FontResolverInfo ResolveTypeface(string familyName, bool isBold, bool isItalic)
    { 
        if (familyName == "Belfast Regular")                 
            return new FontResolverInfo("Belfast Regular", isBold, isItalic);
       
        else                 
            return null;
        
    }
}
