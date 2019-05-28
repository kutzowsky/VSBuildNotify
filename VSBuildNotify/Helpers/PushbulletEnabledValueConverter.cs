using System;
using System.Windows.Data;
using VSBuildNotify.Notifiers;

namespace VSBuildNotify.Helpers
{
    public class PushbulletEnabledValueConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if ((NotifierType)value == NotifierType.PUSHBULLET)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
