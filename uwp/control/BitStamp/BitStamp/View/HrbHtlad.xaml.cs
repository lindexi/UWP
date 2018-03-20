using System;
using System.Collections.Generic;
using System.Numerics;
using Windows.Foundation;
using Windows.Storage;
using Windows.Storage.Pickers;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using BitStamp.ViewModel;
using Microsoft.Graphics.Canvas;
using Microsoft.Graphics.Canvas.Brushes;

// https://go.microsoft.com/fwlink/?LinkId=234238 上介绍了“空白页”项模板

namespace BitStamp
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class HrbHtlad : Page
    {
        public HrbHtlad()
        {
            this.InitializeComponent();
        }

        public HrbHtladModel ViewModel { get; set; }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            ViewModel = (HrbHtladModel) e.Parameter;
            DataContext = ViewModel;

            base.OnNavigatedTo(e);
        }

        private async void KfuconihiKvqy_OnClick(object sender, RoutedEventArgs e)
        {
            var duvDbecdgiu =
                await CanvasBitmap.LoadAsync(new CanvasDevice(), await _file.OpenAsync(FileAccessMode.Read));


            using (var canvasRenderTarget = new CanvasRenderTarget(duvDbecdgiu, duvDbecdgiu.Size))
            {
                using (var dc = canvasRenderTarget.CreateDrawingSession())
                {
                    dc.DrawImage(duvDbecdgiu);
                    dc.DrawText("lindexi",
                        new Vector2((float) (duvDbecdgiu.Size.Width / 2), (float) duvDbecdgiu.Size.Height/2), Colors.Black);
                }

                var pick = new FileSavePicker();
                pick.FileTypeChoices.Add("image", new List<string>() {".jpg"});

                var file = await pick.PickSaveFileAsync();

                await canvasRenderTarget.SaveAsync(await file.OpenAsync(FileAccessMode.ReadWrite),CanvasBitmapFileFormat.Jpeg);
            }
        }

        private async void KknqymKehfobtlv_OnClick(object sender, RoutedEventArgs e)
        {
            var pick = new FileOpenPicker();
            pick.FileTypeFilter.Add(".jpg");
            pick.FileTypeFilter.Add(".png");

            var file = await pick.PickSingleFileAsync();

            _file = file;
        }

        private StorageFile _file;
    }
}
