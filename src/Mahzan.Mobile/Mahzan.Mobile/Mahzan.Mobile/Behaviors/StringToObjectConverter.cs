using System;
using System.Globalization;
using Xamarin.Forms;

namespace Mahzan.Mobile.Behaviors
{
    public class StringToObjectConverter<T>: IValueConverter
    {
        public T FalseObject { set; get; }

        public T TrueObject { set; get; }

        public object Convert(object value, Type targetType,
            object parameter, CultureInfo culture)
        {
            if (value != null)
            {
                if (value =="" || value ==string.Empty)
                {
                    return this.FalseObject;
                }
                else
                {
                    return this.TrueObject;
                }
            }
            else
                return this.FalseObject;

        }

        public object ConvertBack(object value, Type targetType,
            object parameter, CultureInfo culture)
        {
            return ((T)value).Equals(this.TrueObject);
        }
    }
}