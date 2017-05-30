// lindexi
// 16:03

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using Windows.ApplicationModel.DataTransfer;
using Windows.Data.Xml.Dom;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Foundation.Metadata;
using Windows.Graphics.Display;
using Windows.Graphics.Imaging;
using Windows.Storage;
using Windows.Storage.Pickers;
using Windows.Storage.Streams;
using Windows.UI.Notifications;
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
            Window.Current.VisibilityChanged += Current_VisibilityChanged;
            
            Unloaded += StampPage_Unloaded;
        }

        private void StampPage_Unloaded(object sender, RoutedEventArgs e)
        {
            Window.Current.VisibilityChanged -= Current_VisibilityChanged;
        }

        private void Current_VisibilityChanged(object sender, Windows.UI.Core.VisibilityChangedEventArgs e)
        {
            _visibility = e.Visible;
        }

        /// <summary>
        /// 判断已经不显示
        /// </summary>
        private bool _visibility = true;

        private string _name;

        private Stamp View
        {
            set;
            get;
        }

        private StorageFolder Folder => AccoutGoverment.AccountModel.Account.Folder;

        private async void ImageStorage_OnClick(object sender, RoutedEventArgs e)
        {
            FileOpenPicker pick = new FileOpenPicker();
            pick.FileTypeFilter.Add(".jpg");
            pick.FileTypeFilter.Add(".png");
            pick.FileTypeFilter.Add(".gif");
            BitmapImage bitmap = new BitmapImage();
            StorageFile file = await pick.PickSingleFileAsync();
            if (file == null)
            {
                return;
            }
            using (IRandomAccessStream stream = await file.OpenAsync(FileAccessMode.Read))
            {
                bitmap.SetSource(stream);
            }
            image.Source = bitmap;
            View.Image = bitmap;

            File = file;
            _upload = true;
        }
        private StorageFile File
        {
            set;
            get;
        }

        private bool _upload;
        private async void Storage_OnClick(object sender, RoutedEventArgs e)
        {
            if (!_upload)
            {
                View.Address = "没有选择图片";
                return;
            }
            if (File?.FileType == ".gif")
            {
                View.File = File;
            }
            else
            {
                DateTime time = DateTime.Now;
                string name = _name + time.Year + time.Month + time.Day + time.Hour + time.Minute + time.Second;

                var bitmap = new RenderTargetBitmap();
                StorageFile file = await Folder.CreateFileAsync(name + ".jpg",
                    CreationCollisionOption.GenerateUniqueName);
                await bitmap.RenderAsync(Stamp);
                var buffer = await bitmap.GetPixelsAsync();
                try
                {
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
                }
                catch (ArgumentException)
                {
                    View.Address = "没有选择图片";
                }
            }

            await View.Jcloud(() =>
            {
                if (_visibility && Window.Current.CoreWindow.Visible)
                {
                    //如果当前显示，自动复制
                    string str = "";
                    switch (_pivot)
                    {
                        case "Markdown":
                            str = View.LinkReminder;
                            break;
                        case "BBCode":
                            str = View.Bcode;
                            break;
                        case "":

                            break;
                    }
                    if (!string.IsNullOrEmpty(str))
                    {
                        DataPackage data = new DataPackage();
                        data.SetText(str);
                        Clipboard.SetContent(data);
                        //告诉说已经复制
                        ToastHelper.PopToast("已经复制", str);
                    }
                }
            });
            File = null;
            _upload = false;
            //上传完成

            
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
                    if ((file.FileType == ".png") || (file.FileType == ".jpg")
                        || (file.FileType == ".gif"))
                    {
                        await ImageStorageFile(file);
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

        private async System.Threading.Tasks.Task ImageStorageFile(StorageFile file)
        {
            if (file == null)
            {
                return;
            }
            File = file;
            _upload = true;
            try
            {
                BitmapImage bitmap = new BitmapImage();
                await bitmap.SetSourceAsync(await file.OpenAsync(FileAccessMode.Read));
                image.Source = bitmap;
                View.Image = bitmap;
            }
            catch (UnauthorizedAccessException)
            {
                //
            }
            catch (IOException)
            {

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
                    _upload = true;
                }
                else if (data.Contains(StandardDataFormats.StorageItems))
                {
                    StorageFile file = (await data.GetStorageItemsAsync()).First() as StorageFile;
                    await ImageStorageFile(file);
                }
            }
        }


        private void StrClipboard(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(View.LinkReminder))
            {
                DataPackage data = new DataPackage();
                data.SetText(View.LinkReminder);
                Clipboard.SetContent(data);
            }
        }

        private void BcodeClipboard(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(View.Bcode))
            {
                DataPackage data = new DataPackage();
                data.SetText(View.Bcode);
                Clipboard.SetContent(data);
            }
        }

        private void Pivot_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var pivot = e.AddedItems.FirstOrDefault() as PivotItem;
            var str = pivot?.Header.ToString();
            _pivot = str;
        }

        /// <summary>
        /// 表示当前 pivot 选择的
        /// </summary>
        private string _pivot;
    }




    //edi大神提供
    public static class ToastHelper
    {
        /// <summary>
        /// Show notification in Action Center
        /// </summary>
        /// <param name="title">Notification title</param>
        /// <param name="content">Notification content</param>
        /// <returns>ToastNotification</returns>
        public static ToastNotification PopToast(string title, string content)
        {
            return PopToast(title, content, null, null);
        }

        /// <summary>
        /// Show notification in Action Center
        /// </summary>
        /// <param name="title">Notification title</param>
        /// <param name="content">Notification content</param>
        /// <param name="tag">Tag</param>
        /// <param name="group">Group</param>
        /// <returns>ToastNotification</returns>
        public static ToastNotification PopToast(string title, string content, string tag, string group)
        {
            string xml = $@"<toast activationType='foreground'>
                                            <visual>
                                                <binding template='ToastGeneric'>
                                                </binding>
                                            </visual>
                                        </toast>";

            XmlDocument doc = new XmlDocument();
            doc.LoadXml(xml);

            var binding = doc.SelectSingleNode("//binding");

            var el = doc.CreateElement("text");
            el.InnerText = title;

            binding.AppendChild(el);

            el = doc.CreateElement("text");
            el.InnerText = content;
            binding.AppendChild(el);

            return PopCustomToast(doc, tag, group);
        }

        /// <summary>
        /// Show notification by custom xml
        /// </summary>
        /// <param name="xml">notification xml</param>
        /// <returns>ToastNotification</returns>
        public static ToastNotification PopCustomToast(string xml)
        {
            return PopCustomToast(xml, null, null);
        }

        /// <summary>
        /// Show notification by custom xml
        /// </summary>
        /// <param name="xml">notification xml</param>
        /// <param name="tag">tag</param>
        /// <param name="group">group</param>
        /// <returns>ToastNotification</returns>
        public static ToastNotification PopCustomToast(string xml, string tag, string group)
        {
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(xml);


            return PopCustomToast(doc, tag, group);
        }

        /// <summary>
        /// Show notification by custom xml
        /// </summary>
        /// <param name="doc">notification xml</param>
        /// <param name="tag">tag</param>
        /// <param name="group">group</param>
        /// <returns>ToastNotification</returns>
        [DefaultOverload]
        public static ToastNotification PopCustomToast(XmlDocument doc, string tag, string group)
        {
            var toast = new ToastNotification(doc);

            if (tag != null)
                toast.Tag = tag;

            if (group != null)
                toast.Group = group;

            ToastNotificationManager.CreateToastNotifier().Show(toast);

            return toast;
        }

        public static string ToString(ValueSet valueSet)
        {
            StringBuilder builder = new StringBuilder();

            foreach (var pair in valueSet)
            {
                if (builder.Length != 0)
                    builder.Append('\n');

                builder.Append(pair.Key);
                builder.Append(": ");
                builder.Append(pair.Value);
            }

            return builder.ToString();
        }
    }
}