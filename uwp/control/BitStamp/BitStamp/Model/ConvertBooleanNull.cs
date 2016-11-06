using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Data;

namespace BitStamp.Model
{
    public class ConvertBooleanNull : IValueConverter
    {
        public object Convert(object value, Type targetType,
            object parameter,
            string language)
        {
            if (value is bool?)
            {
                bool? temp = value as bool?;
                if (temp == true)
                {
                    return true;
                }
                return false;
            }
            return false;
        }

        public object ConvertBack(object value, 
            Type targetType, 
            object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
