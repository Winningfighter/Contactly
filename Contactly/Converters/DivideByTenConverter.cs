using System.Globalization;

namespace Contactly.Converters;

public class DivideByTenConverter : IValueConverter
{
    // Wandelt den Wert von der Datenbank (0-10) in den ProgressBar-Wert (0.0-1.0) um
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is double doubleValue)
        {
            return doubleValue / 10.0;
        }
        
        if (value is int intValue)
        {
            return intValue / 10.0;
        }

        return 0.0;
    }
    
    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}