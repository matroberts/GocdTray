using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Markup;
using System.Windows.Media;

namespace GocdTray.Ui.Converters
{
    // Original version from: https://www.codeproject.com/Tips/634540/Using-Font-Icons
    public class ImageFromFont : MarkupExtension
    {
        public string Text { get; set; }
        public FontFamily FontFamily { get; set; }
        public FontStyle Style { get; set; }
        public FontWeight Weight { get; set; }
        public FontStretch Stretch { get; set; }
        public Brush Brush { get; set; }
        public ImageFromFont()
        {
            Text = "G";
            FontFamily = new FontFamily("/GocdTray.Ui;Component/Fonts/FontAwesome.otf#Font Awesome 5 Free Solid");
            Style = FontStyles.Normal;
            Weight = FontWeights.Normal;
            Stretch = FontStretches.Normal;
            Brush = new SolidColorBrush(Colors.Black);
        }

        private static ImageSource CreateGlyph(string text, FontFamily fontFamily, FontStyle fontStyle, FontWeight fontWeight, FontStretch fontStretch, Brush foreBrush)
        {
            if(string.IsNullOrEmpty(text))
                throw new ArgumentException("Text must be specified");
            if(fontFamily == null)
                throw new ArgumentException("FontFamiliy must be specified");

            var typeface = new Typeface(fontFamily, fontStyle, fontWeight, fontStretch);

            if (!typeface.TryGetGlyphTypeface(out var glyphTypeface))
            {
                typeface = new Typeface(new FontFamily(new Uri("pack://application:,,,"), fontFamily.Source), fontStyle, fontWeight, fontStretch);
                if (!typeface.TryGetGlyphTypeface(out glyphTypeface))
                    throw new InvalidOperationException("No glyphtypeface found");
            }

            ushort[] glyphIndexes = new ushort[text.Length];
            double[] advanceWidths = new double[text.Length];

            for (int n = 0; n < text.Length; n++)
            {
                ushort glyphIndex;
                try
                {
                    glyphIndex = glyphTypeface.CharacterToGlyphMap[text[n]];

                }
                catch (Exception)
                {
                    glyphIndex = 42;
                }
                glyphIndexes[n] = glyphIndex;
                advanceWidths[n] = glyphTypeface.AdvanceWidths[glyphIndex] * 1.0;
            }

            var glyphRun = new GlyphRun(glyphTypeface, 0, false, 1.0, glyphIndexes, new Point(0, 0), advanceWidths, null, null, null, null, null, null);
            var glyphRunDrawing = new GlyphRunDrawing(foreBrush, glyphRun);
            return new DrawingImage(glyphRunDrawing);
        }

        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            return CreateGlyph(Text, FontFamily, Style, Weight, Stretch, Brush);
        }
    }
}