using System;
using System.Globalization;
using Xamarin.Forms;

namespace VHCircularLayoutXF.Converters
{
    public class BoolToStringConverter : IValueConverter
    {
        public BoolToStringConverter()
        {
        }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            bool isToggled = (bool)value;
            if (isToggled)
            {
                return "Assemble";
            }
            else
            {
                return "";
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
