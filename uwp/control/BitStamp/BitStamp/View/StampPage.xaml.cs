// lindexi
// 16:03

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.ApplicationModel.DataTransfer;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Graphics.Display;
using Windows.Graphics.Imaging;
using Windows.Storage;
using Windows.Storage.Pickers;
using Windows.Storage.Streams;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;
using BitStamp.Model;
using BitStamp.ViewModel;

// The User Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234236

namespace BitStamp.View
{
    public sealed partial class StampPage : UserControl
    {
        public StampPage()
        {
            View = new Stamp();
            this.InitializeComponent();
            Folder = KnownFolders.PicturesLibrary;
        }

        private string _name;

        private Stamp View
        {
            set;
            get;
        }

        private StorageFolder Folder
        {
            set;
            get;
        }

        private async void ImageStorage_OnClick(object sender, RoutedEventArgs e)
        {
            FileOpenPicker pick = new FileOpenPicker();
            pick.FileTypeFilter.Add(".jpg");
            pick.FileTypeFilter.Add(".png");
            BitmapImage bitmap = new BitmapImage();
            StorageFile file = await pick.PickSingleFileAsync();
            using (IRandomAccessStream stream = await file.OpenAsync(FileAccessMode.Read))
            {
                bitmap.SetSource(stream);
            }
            image.Source = bitmap;
            View.Image = bitmap;
        }

        private async void Storage_OnClick(object sender, RoutedEventArgs e)
        {
            DateTime time = DateTime.Now;
            string name = _name + time.Year + time.Month + time.Day + time.Hour + time.Minute + time.Second;

            var bitmap = new RenderTargetBitmap();
            StorageFile file = await Folder.CreateFileAsync(name + ".jpg",
                CreationCollisionOption.GenerateUniqueName);
            await bitmap.RenderAsync(Stamp);
            var buffer = await bitmap.GetPixelsAsync();
            using (IRandomAccessStream stream = await file.OpenAsync(FileAccessMode.ReadWrite))
            {
                var encod = await BitmapEncoder.CreateAsync(
                    BitmapEncoder.JpegEncoderId, stream);
                encod.SetPixelData(BitmapPixelFormat.Bgra8,
                    BitmapAlphaMode.Ignore,
                    (uint) bitmap.PixelWidth,
                    (uint) bitmap.PixelHeight,
                    DisplayInformation.GetForCurrentView().LogicalDpi,
                    DisplayInformation.GetForCurrentView().LogicalDpi,
                    buffer.ToArray()
                );
                await encod.FlushAsync();
            }
            View.File = file;
            View.Jcloud();
        }

        private void Grid_OnDragOver(object sender, DragEventArgs e)
        {
            e.AcceptedOperation = DataPackageOperation.Copy;
            e.Handled = true;
        }

        private async void Grid_Drop(object sender, DragEventArgs e)
        {
            var defer = e.GetDeferral();
            try
            {
                DataPackageView dataView = e.DataView;
                if (dataView.Contains(StandardDataFormats.StorageItems))
                {
                    var files = await dataView.GetStorageItemsAsync();
                    StorageFile file = files.OfType<StorageFile>().First();
                    if ((file.FileType == ".png") || (file.FileType == ".jpg"))
                    {
                        BitmapImage bitmap = new BitmapImage();
                        await bitmap.SetSourceAsync(await file.OpenAsync(FileAccessMode.Read));
                        image.Source = bitmap;
                        View.Image = bitmap;
                    }
                    _name = file.DisplayName;
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

        private async void TextBox_Clipboard(object sender, TextControlPasteEventArgs e)
        {
            DataPackageView data = Clipboard.GetContent();
            if (data != null)
            {
                if (data.Contains(StandardDataFormats.Bitmap))
                {
                    RandomAccessStreamReference file = await data.GetBitmapAsync();
                    BitmapImage image = new BitmapImage();
                    await image.SetSourceAsync(await file.OpenReadAsync());
                    this.image.Source = image;
                    View.Image = image;
                }
            }
        }


        private void StrClipboard(object sender, RoutedEventArgs e)
        {
            DataPackage data = new DataPackage();
            data.SetText(View.LinkReminder);
            Clipboard.SetContent(data);
        }
    }
}