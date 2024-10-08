﻿using System;
using System.Globalization;
using System.IO;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace JgYxs.UI.UserCtrl.Converter
{
    public class VisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var reverse = false;
            if (parameter != null && parameter is bool b)
            {
                reverse = b;
            }
            if (value is bool v)
            {
                return (reverse ? !v : v) ? System.Windows.Visibility.Visible : Visibility.Hidden;
            }

            return System.Windows.Visibility.Hidden;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
