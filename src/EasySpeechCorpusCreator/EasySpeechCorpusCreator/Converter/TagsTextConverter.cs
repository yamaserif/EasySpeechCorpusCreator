using System;
using System.Globalization;
using System.Text.RegularExpressions;
using System.Windows.Data;

namespace EasySpeechCorpusCreator.Converter
{
    internal class TagsTextConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var strData = value as string ?? string.Empty;
            strData = Regex.Replace(strData, ", *", "] [");
            return "[" + strData;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var strData = value as string ?? string.Empty;
            if (strData.EndsWith(']'))
            {
                strData = strData.Remove(strData.Length - 1);
                strData += ", ";
            }

            strData = strData.TrimStart('[').TrimEnd(']');
            return Regex.Replace(strData, "\\].? *.?\\[", ", ");
        }
    }
}
