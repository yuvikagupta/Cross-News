using System;
using System.Globalization;
using MvvmCross.Converters;

namespace CrossNews.Core.Converters
{
    public class BoolToOnOffValueConverter : MvxValueConverter<bool, string>
    {
        protected override string Convert(bool value, Type targetType, object parameter, CultureInfo culture) => value ? "On" : "Off";
    }
}
