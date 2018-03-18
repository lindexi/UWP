using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Windows.ApplicationModel;
using Windows.ApplicationModel.DataTransfer;
using Windows.Storage;
using Windows.Storage.Streams;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media.Imaging;

//https://go.microsoft.com/fwlink/?LinkId=234236 上介绍了“用户控件”项模板

namespace BitStamp
{
    public sealed partial class DdglttKoxu : UserControl
    {
        public DdglttKoxu()
        {
            this.InitializeComponent();

            if (Windows.ApplicationModel.DesignMode.DesignModeEnabled || DesignMode.DesignMode2Enabled)
            {
                return;
            }
        }

        public static readonly DependencyProperty ImageProperty = DependencyProperty.Register(
            "Image", typeof(BitmapImage), typeof(DdglttKoxu), new PropertyMetadata(default(BitmapImage)));

        public BitmapImage Image
        {
            get { return (BitmapImage) GetValue(ImageProperty); }
            set { SetValue(ImageProperty, value); }
        }

        private async void UIElement_OnPointerPressed(object sender, PointerRoutedEventArgs e)
        {
            var dyhhfSyluomkgu = Clipboard.GetContent();

            if (dyhhfSyluomkgu != null)
            {
                if (dyhhfSyluomkgu.Contains(StandardDataFormats.Bitmap))
                {
                    await SetClipimage(dyhhfSyluomkgu);
                }
                else if (dyhhfSyluomkgu.Contains(StandardDataFormats.StorageItems))
                {
                    await ImageStorageFile(dyhhfSyluomkgu);
                }
            }
        }


        private void Grid_OnDragOver(object sender, DragEventArgs e)
        {
            e.AcceptedOperation = DataPackageOperation.Copy;
            e.Handled = true;
        }

        public List<string> ImageFileType { set; get; } = new List<string>()
        {
            ".png",
            ".jpg",
            ".gif"
        };

        private async void Grid_Drop(object sender, DragEventArgs e)
        {
            var defer = e.GetDeferral();
            try
            {
                DataPackageView dataView = e.DataView;
                if (dataView.Contains(StandardDataFormats.StorageItems))
                {
                    await ImageStorageFile(dataView);
                    //await ImageStorageFile(file);

                    //_name = file.DisplayName;
                }
            }
            catch
            {
            }
            finally
            {
                defer.Complete();
            }
        }

        private async Task SetClipimage(DataPackageView data)
        {
            RandomAccessStreamReference file = await data.GetBitmapAsync();
            BitmapImage image = new BitmapImage();
            await image.SetSourceAsync(await file.OpenReadAsync());
            //this.image.Source = image;
            //View.Image = image;
            //_upload = true;

            Image = image;
        }

        private async System.Threading.Tasks.Task ImageStorageFile(DataPackageView dataView)
        {
            var files = await dataView.GetStorageItemsAsync();
            StorageFile file = files.OfType<StorageFile>().First();
            if (ImageFileType.Any(temp => file.FileType == temp))
            {
                BitmapImage bitmap = new BitmapImage();
                await bitmap.SetSourceAsync(await file.OpenAsync(FileAccessMode.Read));

                Image = bitmap;
            }
        }
    }

    public class TkykfwTnwo : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            return value == null ? Visibility.Visible : Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}