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
using BitStamp.Model.Cimage;
using BitStamp.View;
using lindexi.uwp.Framework.ViewModel;
using lindexi.uwp.ImageShack.Model;
using Microsoft.Graphics.Canvas;
using Microsoft.Graphics.Canvas.Text;
using Microsoft.Graphics.Canvas.UI.Xaml;

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
                TlljvlfTcbzqe = true;

                Upload();
            }
            else if (data.Contains(StandardDataFormats.StorageItems))
            {
                await KipfrgxqyTzt(data);
                TlljvlfTcbzqe = true;

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
                TlljvlfTcbzqe = true;
            }
        }

        public bool TlljvlfTcbzqe { get; set; }

        private async Task SpydTmzmtzul(StorageFile file)
        {
            var bitmap = new BitmapImage();
            await bitmap.SetSourceAsync(await file.OpenAsync(FileAccessMode.Read));
            Image = bitmap;

            CanvasBitmap = await CanvasBitmap.LoadAsync(new CanvasDevice(), await file.OpenAsync(FileAccessMode.Read));

            TlljvlfTcbzqe = true;
        }

        private async Task KfuconihiKvqy()
        {
            var duvDbecdgiu =
                CanvasBitmap;

            using (var canvasRenderTarget = new CanvasRenderTarget(duvDbecdgiu, duvDbecdgiu.Size))
            {
                using (var dc = canvasRenderTarget.CreateDrawingSession())
                {
                    dc.DrawImage(duvDbecdgiu);
                    if (MarTxanmvssTfnpqlz && !string.IsNullOrEmpty(MartHzlxwlTcq))
                    {
                        var canvasTextFormat = new CanvasTextFormat()
                        {
                            FontSize = 15f,
                            WordWrapping = CanvasWordWrapping.NoWrap
                        };
                        var canvasTextLayout =
                            new CanvasTextLayout(canvasRenderTarget, MartHzlxwlTcq, canvasTextFormat, 0, 0);

                        var kjrjuxzaKrbgwk = canvasTextLayout.LayoutBounds;

                        if (kjrjuxzaKrbgwk.Width < duvDbecdgiu.Size.Width)

                            dc.DrawText("lindexi",
                                new Vector2((float) (duvDbecdgiu.Size.Width / 2), (float) duvDbecdgiu.Size.Height / 2),
                                Colors.Black);
                    }
                }

                var file = await KuaxShft();

                await canvasRenderTarget.SaveAsync(await file.OpenAsync(FileAccessMode.ReadWrite),
                    CanvasBitmapFileFormat.Jpeg);

                _file = file;
            }
        }

        private async Task<StorageFile> KuaxShft()
        {
            StringBuilder tcnkvprzTxe = new StringBuilder();
            var sasTvhqc = DateTime.Now;
            tcnkvprzTxe.Append(sasTvhqc.Year.ToString() + sasTvhqc.Month.ToString() + sasTvhqc.Day.ToString() +
                               sasTvhqc.Hour.ToString() + sasTvhqc.Minute.ToString() + sasTvhqc.Second.ToString() +
                               ran.Next(1000).ToString() + ran.Next(10).ToString());

            tcnkvprzTxe.Append(".jpg");

            StorageFile file;
            try
            {
                file = await StDbvedbwpHxxz.CreateFileAsync(tcnkvprzTxe.ToString());
            }

            catch (FileNotFoundException )
            {
                StDbvedbwpHxxz = ApplicationData.Current.TemporaryFolder;
                file = await StDbvedbwpHxxz.CreateFileAsync(tcnkvprzTxe.ToString());
            }
            return file;
        }

        private static Random ran = new Random();

        public StorageFolder StDbvedbwpHxxz { get; set; }

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
            if (!TlljvlfTcbzqe)
            {
                return;
            }

            TlljvlfTcbzqe = false;

            await KfuconihiKvqy();

            var heaaxThesolw = new HeaaxThesolw
            {
                File = _file,
                Account = Account
            };
            await heaaxThesolw.Jcloud(tggSqlaeprfo =>
            {
                if (tggSqlaeprfo)
                {
                    MarTqqcyhuaKujem = $"![]({heaaxThesolw.Url})";
                    BbTkeozdDmady = $"[img]{heaaxThesolw.Url}[/img]";

                    ToastHelper.PopToast("uwp 图床", "上传成功" + heaaxThesolw.Url);
                }
                else
                {
                    MarTqqcyhuaKujem = "上传失败";
                    BbTkeozdDmady = "上传失败";

                    ToastHelper.PopToast("uwp 图床", "上传失败");

                    TlljvlfTcbzqe = true;
                }
            });
        }

        public async void FileHhhrSkq()
        {
            var pick = new FileOpenPicker();
            pick.FileTypeFilter.Add(".jpg");
            pick.FileTypeFilter.Add(".png");

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

            CanvasBitmap = await CanvasBitmap.LoadAsync(new CanvasDevice(), await file.OpenReadAsync());

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
                StDbvedbwpHxxz = Account.Folder;
            }

            if (StDbvedbwpHxxz == null)
            {
                StDbvedbwpHxxz = ApplicationData.Current.TemporaryFolder;
            }
        }
    }

    public class HeaaxThesolw
    {
        public Account Account { get; set; }

        public StorageFile File { get; set; }

        private UploadImageTask NewUploadImageTask(ImageShackEnum imageShack, StorageFile file)
        {
            switch (imageShack)
            {
                case ImageShackEnum.Jiuyou:
                    return new JyUploadImage(file);
                case ImageShackEnum.Smms:
                    return new SmmsUploadImage(file);
                case ImageShackEnum.Qin:
                    return new QnUploadImage(file)
                    {
                        Accound = Account.CloundAccound
                    };
                case ImageShackEnum.Cimage:
                    return new Cimage(file);
                default:
                    throw new ArgumentOutOfRangeException(nameof(imageShack), imageShack, null);
            }

            //return new JyUploadImage(file);
        }

        public string Url { get; set; }

        public async Task Jcloud(Action<bool> onUpload)
        {
            ImageShackEnum imageShack = Account.ImageShack;
            if (File.FileType == ".gif" && imageShack == ImageShackEnum.Jiuyou)
            {
                imageShack = ImageShackEnum.Qin;
            }

            var size = (await File.GetBasicPropertiesAsync()).Size;

            //1M
            //1024k
            //‪125000‬
            if (size > 12500000)
            {
                imageShack = ImageShackEnum.Smms;
            }
            //4326  24,447 

            imageShack = CheckShack(imageShack);

#if DEBUG
            //imageShack = ImageShackEnum.Cimage;
#endif

            UploadImageTask uploadImageTask = NewUploadImageTask(
                imageShack, File);
            uploadImageTask.OnUploaded += (s, e) =>
            {
                if (!(s is UploadImageTask uploadImage))
                {
                    onUpload?.Invoke(false);
                    return;
                }

                Url = uploadImage.Url;

                onUpload?.Invoke(e);
            };
            uploadImageTask.UploadImage();
        }

        private ImageShackEnum CheckShack(ImageShackEnum imageShack)
        {
            //检查当前是否可以使用
            if (imageShack == ImageShackEnum.Qin)
            {
                //如果图床账号错误
                if (string.IsNullOrEmpty(Account.CloundAccound.AccessKey) ||
                    string.IsNullOrEmpty(Account.CloundAccound.Bucket) ||
                    string.IsNullOrEmpty(Account.CloundAccound.SecretKey) ||
                    string.IsNullOrEmpty(Account.CloundAccound.Url))
                {
                    return ImageShackEnum.Smms;
                }
            }

            return imageShack;
        }
    }
}