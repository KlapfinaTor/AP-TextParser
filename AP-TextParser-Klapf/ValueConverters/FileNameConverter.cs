using System;
using System.Globalization;
using System.Windows.Data;
using System.IO;

namespace AP_TextParser_Klapf.ValueConverters
{
    /// <summary>
    /// Converter that converts a full filepath to just the filename
    /// </summary>
    public class FileNameConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture) //viewmodel ->view
        {
            string rawPath = (string)value;
            return Path.GetFileNameWithoutExtension(rawPath);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) //view ->viewmmodel
        {
            throw new NotImplementedException();
        }
    }
}