using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.IO;

namespace AP_TextParser_Klapf.ValueConverters
{
    public class FileNameConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture) //viewmodel ->view
        {
            
            string rawPath = (string)value;
            return  Path.GetFileNameWithoutExtension(rawPath);

        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) //view ->viewmmodel
        {
            throw new NotImplementedException();
        }
    }
}
