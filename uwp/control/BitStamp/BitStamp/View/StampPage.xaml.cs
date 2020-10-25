using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using System.Threading.Tasks;
using BitStamp.ViewModel;
using Windows.ApplicationModel.Core;
using Windows.ApplicationModel.DataTransfer;
using Windows.Data.Xml.Dom;
using Windows.Foundation.Collections;
using Windows.Foundation.Metadata;
using Windows.Graphics.Display;
using Windows.Graphics.Imaging;
using Windows.Storage;
using Windows.Storage.Pickers;
using Windows.Storage.Streams;
using Windows.UI.Core;
using Windows.UI.Notifications;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media.Imaging;

//using Microsoft.Advertising.WinRT.UI;

// The User Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234236

namespace BitStamp.View
{
    public sealed partial class StampPage : UserControl
    {
        public StampPage()
        {
            View = new Stamp();
            InitializeComponent();

            Window.Current.VisibilityChanged += Current_VisibilityChanged;
            Window.Current.Activated += Current_Activated;
            Unloaded += StampPage_Unloaded;
        }

        private void Current_Activated(object sender, WindowActivatedEventArgs e)
        {
            _visibility = e.WindowActivationState != CoreWindowActivationState.Deactivated;
        }

        private string _name;

        /// <summary>
        ///     表示当前 pivot 选择的
        /// </summary>
        private string _pivot;

        private bool _upload;

        /// <summary>
        ///     判断已经不显示
        /// </summary>
        private bool _visibility = true;


        private void StampPage_Unloaded(object sender, RoutedEventArgs e)
        {
            Window.Current.VisibilityChanged -= Current_VisibilityChanged;
            Window.Current.Activated -= Current_Activated;
        }

        private void Current_VisibilityChanged(object sender, VisibilityChangedEventArgs e)
        {
            _visibility = e.Visible;
        }

        private Stamp View { set; get; }

        private StorageFolder Folder => AccoutGoverment.AccountModel.Account.Folder;

        private async void ImageStorage_OnClick(object sender, RoutedEventArgs e)
        {
            FileOpenPicker pick = new FileOpenPicker();
            pick.FileTypeFilter.Add(".jpg");
            pick.FileTypeFilter.Add(".png");
            pick.FileTypeFilter.Add(".gif");
            BitmapImage bitmap = new BitmapImage();
            View.Visibility = Visibility.Visible;
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

            View.Visibility = Visibility.Collapsed;
            File = file;
            _upload = true;
        }

        private StorageFile File { set; get; }

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
                var str = View.Account.Account.Str;

                if (!ToggleSwitch.IsOn)
                {
                    View.Account.Account.Str = "";
                }

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

                View.Account.Account.Str = str;
            }


            await View.Jcloud(() =>
            {
                if (_visibility)
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

        private async Task ImageStorageFile(StorageFile file)
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
                    await SetClipimage(data);
                }
                else if (data.Contains(StandardDataFormats.StorageItems))
                {
                    StorageFile file = (await data.GetStorageItemsAsync()).First() as StorageFile;
                    await ImageStorageFile(file);
                }
            }
        }

        private async Task SetClipimage(DataPackageView data)
        {
            RandomAccessStreamReference file = await data.GetBitmapAsync();
            BitmapImage image = new BitmapImage();
            await image.SetSourceAsync(await file.OpenReadAsync());
            this.image.Source = image;
            View.Image = image;
            _upload = true;
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


        private async void Pasteup_OnPointerPressed(object sender, PointerRoutedEventArgs e)
        {
            //尝试从剪贴板获得图片
            if (File == null)
            {
                var clip = Clipboard.GetContent();
                if (clip.Contains(StandardDataFormats.Bitmap))
                {
                    await SetClipimage(clip);
                    Storage_OnClick(sender, e);
                }
            }
        }
    }


    //edi大神提供
    public static class ToastHelper
    {
        /// <summary>
        ///     Show notification in Action Center
        /// </summary>
        /// <param name="title">Notification title</param>
        /// <param name="content">Notification content</param>
        /// <returns>ToastNotification</returns>
        public static ToastNotification PopToast(string title, string content)
        {
            return PopToast(title, content, null, null);
        }

        /// <summary>
        ///     Show notification in Action Center
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
        ///     Show notification by custom xml
        /// </summary>
        /// <param name="xml">notification xml</param>
        /// <param name="tag">tag</param>
        /// <param name="group">group</param>
        /// <returns>ToastNotification</returns>
        public static ToastNotification PopCustomToast(string xml, string tag = null, string group = null)
        {
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(xml);


            return PopCustomToast(doc, tag, group);
        }

        /// <summary>
        ///     Show notification by custom xml
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
