using System;
using System.Globalization;
using System.Windows.Data;
using AcensiPhotoShop;

namespace PhotoShop
{
    public class WpfColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var color = (Color)value;
            return new System.Windows.Media.Color
            {
                R = color.R,
                G = color.G,
                B = color.B,
                A =  255
            };
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var wpfColor = (System.Windows.Media.Color)value;
            return new Color(wpfColor.R, wpfColor.G, wpfColor.B);
        }
    }
}