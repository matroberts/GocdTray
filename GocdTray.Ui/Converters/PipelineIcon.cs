using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;
using GocdTray.App.Abstractions;

namespace GocdTray.Ui.Converters
{
    [ValueConversion(typeof(bool), typeof(string))]
    public class PipelineIcon : IValueConverter
    {
        public string IconPaused { get; set; }
        public string IconRunning { get; set; }
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            bool paused = (bool) value;
            if (paused)
            {
                return IconPaused;
            }
            else
            {
                return IconRunning;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }
}