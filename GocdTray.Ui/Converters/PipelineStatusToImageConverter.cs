using System;
using System.Drawing;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using GocdTray.App.Abstractions;
using GocdTray.Ui.Properties;

namespace GocdTray.Ui.Converters
{
    [ValueConversion(typeof(PipelineStatus), typeof(SolidColorBrush))]
    public class PipelineStatusToImageConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            switch ((PipelineStatus)value)
            {
                case PipelineStatus.Building:
                    return new SolidColorBrush(Colors.Gold);
                case PipelineStatus.Passed:
                    return new SolidColorBrush(Colors.Green);
                case PipelineStatus.Failed:
                    return new SolidColorBrush(Colors.Red);
                default:
                    throw new ArgumentOutOfRangeException(nameof(value), value, null);
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }
}