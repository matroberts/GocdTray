using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
using GocdTray.Ui.Code;

namespace GocdTray.Ui.Converters
{
    [ValueConversion(typeof(PipelineSortOrder), typeof(Visibility))]
    public class PipelineSortOrderToSelectedOptionConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var sortOrder = (PipelineSortOrder) value;
            var option = (PipelineSortOrder) parameter;
            return sortOrder == option ? Visibility.Visible : Visibility.Hidden;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}