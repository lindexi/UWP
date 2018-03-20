using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.ApplicationModel.DataTransfer;
using Windows.Storage;
using Windows.Storage.Pickers;
using Windows.Storage.Streams;
using Windows.UI.Xaml.Media.Imaging;
using lindexi.uwp.Framework.ViewModel;

namespace BitStamp.ViewModel
{
    public class DrowilHuwfevfModel : NavigateViewModel
    {
        public override void OnNavigatedFrom(object sender, object obj)
        {
        }

        public override void OnNavigatedTo(object sender, object obj)
        {
        }
    }

    public class HrbHtladModel : ViewModelMessage
    {
        private BitmapImage _image;
        private string _martHzlxwlTcq;
        private bool _marTxanmvssTfnpqlz;
        private StorageFile _file;

        public BitmapImage Image
        {
            get => _image;
            set
            {
                _image = value;
                OnPropertyChanged();
            }
        }

        public async Task KkrfKuumt(DataPackageView data)
        {
            if (data.Contains(StandardDataFormats.Bitmap))
            {
                await SetClipimage(data);
            }
            else if (data.Contains(StandardDataFormats.StorageItems))
            {
                await KipfrgxqyTzt(data);
            }
        }

        private async System.Threading.Tasks.Task KipfrgxqyTzt(DataPackageView dataView)
        {
            var files = await dataView.GetStorageItemsAsync();
            StorageFile file = files.OfType<StorageFile>().First();
            if (ImageFileType.Any(temp => file.FileType == temp))
            {
                await SpydTmzmtzul(file);
            }
        }

        private async Task SpydTmzmtzul(StorageFile file)
        {
            BitmapImage bitmap = new BitmapImage();
            await bitmap.SetSourceAsync(await file.OpenAsync(FileAccessMode.Read));
            Image = bitmap;
        }

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

        public List<string> ImageFileType { set; get; } = new List<string>()
        {
            ".png",
            ".jpg",
            ".gif"
        };

        public void Upload()
        {
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
                _file = file;
            }
        }

        public async void ClipHnzSytrcwjt()
        {
            var dyhhfSyluomkgu = Clipboard.GetContent();

            if (dyhhfSyluomkgu != null)
            {
                await KkrfKuumt(dyhhfSyluomkgu);
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

        public void MarDcqHghnuz()
        {
            //复制 Markdown
        }

        private string _marTqqcyhuaKujem;

        private string _bbTkeozdDmady;

        public void BbHozTexwufz()
        {
            //复制
        }

        public override void OnNavigatedFrom(object sender, object obj)
        {
        }

        public override void OnNavigatedTo(object sender, object obj)
        {
        }
    }
}