using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Windows.Data;
using VSBuildNotify.Notifiers;

namespace VSBuildNotify.Helpers
{
    public class NotifierTypeValueConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is NotifierType format)
            {
                return GetString(format);
            }

            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is string stringValue)
            {
                return GetEnumValueFromDescription(stringValue);
            }

            return null;
        }

        public string[] Strings => GetStrings();

        private static string GetString(NotifierType notifierType)
        {
            return GetDescription(notifierType);
        }

        private static string[] GetStrings()
        {
            List<string> strings = new List<string>();
            foreach (NotifierType format in Enum.GetValues(typeof(NotifierType)))
            {
                strings.Add(GetString(format));
            }

            return strings.ToArray();
        }

        private static string GetDescription(NotifierType notifierType)
        {
            return notifierType.GetType().GetMember(notifierType.ToString())[0].GetCustomAttribute<DescriptionAttribute>().Description;
        }

        private static NotifierType GetEnumValueFromDescription(string description)
        {
            var type = typeof(NotifierType);
            FieldInfo[] fields = type.GetFields();
            var field = fields
                            .SelectMany(f => f.GetCustomAttributes(
                                typeof(DescriptionAttribute), false), (
                                    f, a) => new { Field = f, Att = a })
                            .Where(a => ((DescriptionAttribute)a.Att)
                                .Description == description).SingleOrDefault();
            return field == null ? default(NotifierType) : (NotifierType)field.Field.GetRawConstantValue();
        }
    }
}
