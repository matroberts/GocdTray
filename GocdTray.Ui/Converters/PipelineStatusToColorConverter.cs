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
    public class PipelineStatusToColorConverter : IValueConverter
    {
        public SolidColorBrush BuildingColor { get; set; }
        public SolidColorBrush PassedColor { get; set; }
        public SolidColorBrush FailedColor { get; set; }
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            switch ((PipelineStatus)value)
            {
                case PipelineStatus.Building:
                    return BuildingColor;
                case PipelineStatus.Passed:
                    return PassedColor;
                case PipelineStatus.Failed:
                    return FailedColor;
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