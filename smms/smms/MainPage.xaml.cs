using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.ApplicationModel.DataTransfer;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;

//“空白页”项模板在 http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409 上有介绍

namespace smms
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();
        }

        private ViewModel View
        {
            set;
            get;
        }=new ViewModel();

        private void Grid_OnDragOver(object sender, DragEventArgs e)
        {
            e.AcceptedOperation = DataPackageOperation.Copy;
            if (e.DragUIOverride != null)
            {
                e.DragUIOverride.Caption = "拖放打开";
            }

            e.Handled = true;
        }

        private async void Grid_OnDrop(object sender, DragEventArgs e)
        {
            var defer = e.GetDeferral();

            try
            {
                DataPackageView dataView = e.DataView;
                // 拖放类型为文件存储。
                if (dataView.Contains(StandardDataFormats.StorageItems))
                {
                    var files = await dataView.GetStorageItemsAsync();
                    StorageFile file = files.OfType<StorageFile>().First();
                    if (file.FileType == ".png" || file.FileType == ".jpg"
                        || file.FileType == ".gif")
                    {
                        // 拖放的是图片文件。
                        BitmapImage bitmap = new BitmapImage();
                        await bitmap.SetSourceAsync(await file.OpenAsync(FileAccessMode.Read));
                        Image.Source = bitmap;
                        View.File = file;
                    }
                }
            }
            finally
            {
                defer.Complete();
            }
        }
    }
}
