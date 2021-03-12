using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media.Imaging;

namespace AcrylicWindow
{
    public class ImageConverter : IValueConverter
    {
        public object Convert(
            object value, Type targetType, object parameter, CultureInfo culture)
        {
            var bitmap = new BitmapImage();
            bitmap.BeginInit();

            var created = Uri.TryCreate(value?.ToString(), UriKind.Absolute, out var uri);

            if (!created)
                return App.Current.Resources["DefaultImage"] as BitmapImage;


            bitmap.UriSource = uri;
            bitmap.CacheOption = BitmapCacheOption.OnLoad;
            bitmap.DecodePixelHeight = 50;
            bitmap.EndInit();

            return bitmap;
        }

        public object ConvertBack(
            object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }
}
