using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using Windows.ApplicationModel.DataTransfer;
using Windows.Storage;
using Windows.Storage.Pickers;
using Windows.UI;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using BitStamp.Model;
using lindexi.MVVM.Framework.ViewModel;
using lindexi.uwp.Framework.ViewModel;
using lindexi.uwp.ImageShack.Model;
using Microsoft.Graphics.Canvas;
using Microsoft.Graphics.Canvas.Text;
using Microsoft.Graphics.Canvas.UI.Xaml;
using BitStamp.View;

namespace BitStamp.ViewModel
{
    public class HrbHtladModel : ViewModelMessage
    {
        private string _bbTkeozdDmady;
        private StorageFile _file;
        private ImageSource _image;
        private string _martHzlxwlTcq;

        private string _marTqqcyhuaKujem;
        private bool _marTxanmvssTfnpqlz;

        private CanvasBitmap CanvasBitmap { set; get; }

        public ImageSource Image
        {
            get => _image;
            set
            {
                _image = value;
                OnPropertyChanged();
            }
        }

        public Account Account { get; set; }

        public string MartHzlxwlTcq
        {
            get => _martHzlxwlTcq;
            set
            {
                _martHzlxwlTcq = value;
                OnPropertyChanged();
            }
        }

        public bool MarTxanmvssTfnpqlz
        {
            get => _marTxanmvssTfnpqlz;
            set
            {
                _marTxanmvssTfnpqlz = value;
                OnPropertyChanged();
            }
        }

        public List<string> ImageFileType { set; get; } = new List<string>
        {
            ".png",
            ".jpg",
            ".gif"
        };

        public async Task KkrfKuumt(DataPackageView data)
        {
            if (data.Contains(StandardDataFormats.Bitmap))
            {
                await SetClipimage(data);

                Upload();
            }
            else if (data.Contains(StandardDataFormats.StorageItems))
            {
                await KipfrgxqyTzt(data);

                Upload();
            }
        }

        private async Task KipfrgxqyTzt(DataPackageView dataView)
        {
            var files = await dataView.GetStorageItemsAsync();
            var file = files.OfType<StorageFile>().First();
            if (ImageFileType.Any(temp => file.FileType == temp))
            {
                await SpydTmzmtzul(file);
            }
        }

        private async Task SpydTmzmtzul(StorageFile file)
        {
            var bitmap = new BitmapImage();
            await bitmap.SetSourceAsync(await file.OpenAsync(FileAccessMode.Read));
            Image = bitmap;

            await HemdrisJelnunabis.SetImage(file);
        }

        public string BbTkeozdDmady
        {
            get => _bbTkeozdDmady;
            set
            {
                _bbTkeozdDmady = value;
                OnPropertyChanged();
            }
        }

        public string MarTqqcyhuaKujem
        {
            get => _marTqqcyhuaKujem;
            set
            {
                _marTqqcyhuaKujem = value;
                OnPropertyChanged();
            }
        }

        public async void Upload()
        {
            if (!HemdrisJelnunabis.Upload)
            {
                return;
            }

            if (MarTxanmvssTfnpqlz && !string.IsNullOrEmpty(MartHzlxwlTcq))
            {
                await HemdrisJelnunabis.WaterBerbouPelJicayweeno(MartHzlxwlTcq);
            }

            var heaaxThesolw = new HeaaxThesolw(Account, HemdrisJelnunabis.File);
            await heaaxThesolw.Jcloud(tggSqlaeprfo =>
            {
                if (tggSqlaeprfo)
                {
                    MarTqqcyhuaKujem = $"![]({heaaxThesolw.Url})";
                    BbTkeozdDmady = $"[img]{heaaxThesolw.Url}[/img]";

                    ToastHelper.PopToast("uwp 图床", "上传成功" + heaaxThesolw.Url);

                    HemdrisJelnunabis.Upload = false;
                }
                else
                {
                    MarTqqcyhuaKujem = "上传失败";
                    BbTkeozdDmady = "上传失败";

                    ToastHelper.PopToast("uwp 图床", "上传失败");
                }
            });
        }

        public async void FileHhhrSkq()
        {
            var pick = new FileOpenPicker();
            foreach (var temp in ImageFileType)
            {
                pick.FileTypeFilter.Add(temp);
            }

            var file = await pick.PickSingleFileAsync();
            if (file != null)
            {
                await SpydTmzmtzul(file);
            }
        }

        public async void ClipHnzSytrcwjt()
        {
            var dyhhfSyluomkgu = Clipboard.GetContent();

            if (dyhhfSyluomkgu != null) await KkrfKuumt(dyhhfSyluomkgu);
        }

        private async Task SetClipimage(DataPackageView data)
        {
            var file = await data.GetBitmapAsync();
            var image = new BitmapImage();
            await image.SetSourceAsync(await file.OpenReadAsync());

            await HemdrisJelnunabis.SetImage(await file.OpenReadAsync());

            Image = image;
        }

        public void MarDcqHghnuz()
        {
            //复制 Markdown
            TdczkfsepSfqpnd(MarTqqcyhuaKujem);
        }

        private static void TdczkfsepSfqpnd(string str)
        {
            if (string.IsNullOrEmpty(str))
            {
                return;
            }

            var data = new DataPackage();
            data.SetText(str);
            Clipboard.SetContent(data);
        }

        public void BbHozTexwufz()
        {
            //复制
            TdczkfsepSfqpnd(BbTkeozdDmady);
        }

        public override void OnNavigatedFrom(object sender, object obj)
        {
        }

        public override void OnNavigatedTo(object sender, object obj)
        {
            Account = (Account) obj;

            if (Account != null)
            {
                HemdrisJelnunabis.StDbvedbwpHxxz = Account.Folder;
            }

            if (HemdrisJelnunabis.StDbvedbwpHxxz == null)
            {
                HemdrisJelnunabis.StDbvedbwpHxxz = ApplicationData.Current.TemporaryFolder;
            }
        }

        public HemdrisJelnunabisImage HemdrisJelnunabis { get; } = new HemdrisJelnunabisImage();
    }
}