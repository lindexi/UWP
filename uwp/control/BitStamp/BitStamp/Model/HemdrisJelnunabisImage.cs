using System;
using System.IO;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Graphics.Canvas;
using Microsoft.Graphics.Canvas.Brushes;
using Microsoft.Graphics.Canvas.Text;
using Windows.Storage;
using Windows.Storage.Streams;
using Windows.UI;

namespace BitStamp.Model
{
    /// <summary>
    /// 处理图片
    /// </summary>
    public class HemdrisJelnunabisImage
    {
        public async Task WaterBerbouPelJicayweeno(string str)
        {
            if (ImageFile == ImageEnum.Gif)
            {
                return;
            }

            var duvDbecdgiu = CanvasBitmap;

            using (var canvasRenderTarget = new CanvasRenderTarget(duvDbecdgiu, duvDbecdgiu.Size))
            {
                using (var dc = canvasRenderTarget.CreateDrawingSession())
                {
                    dc.DrawImage(duvDbecdgiu);
                    if (!string.IsNullOrEmpty(str))
                    {
                        using (var canvasTextFormat = new CanvasTextFormat()
                        {
                            FontSize = 15f,
                            WordWrapping = CanvasWordWrapping.NoWrap
                        })
                        using (var canvasTextLayout =
                            new CanvasTextLayout(canvasRenderTarget, str, canvasTextFormat, 0, 0))
                        {
                            var kjrjuxzaKrbgwk = canvasTextLayout.LayoutBounds;

                            if (
                                kjrjuxzaKrbgwk.Width < duvDbecdgiu.Size.Width
                                && kjrjuxzaKrbgwk.Height < duvDbecdgiu.Size.Height
                            )
                            {
                                var width = duvDbecdgiu.Size.Width - kjrjuxzaKrbgwk.Width;
                                var height = duvDbecdgiu.Size.Height - kjrjuxzaKrbgwk.Height;

                                var canvasGradientStop = new CanvasGradientStop[2];
                                canvasGradientStop[0] = new CanvasGradientStop()
                                {
                                    Position = 0,
                                    Color = Colors.White
                                };

                                canvasGradientStop[1] = new CanvasGradientStop()
                                {
                                    Position = 1,
                                    Color = Colors.Black
                                };

                                var canvasLinearGradientBrush =
                                    new CanvasLinearGradientBrush(CanvasDevice, canvasGradientStop)
                                    {
                                        StartPoint = new Vector2(0, (float) height / 2),
                                        EndPoint = new Vector2(0, (float) height / 2 + (float) kjrjuxzaKrbgwk.Height)
                                    };

                                dc.DrawText(str,
                                    new Vector2((float) (width / 2),
                                        (float) height / 2),
                                    canvasLinearGradientBrush, canvasTextFormat);
                            }
                        }
                    }
                }

                var file = await KuaxShft();
                using (var stream = await file.OpenAsync(FileAccessMode.ReadWrite))
                {
                    await canvasRenderTarget.SaveAsync(stream,
                        ImageEnumToCanvasBitmapFileFormat(SaveImage));
                }

                ImageFile = SaveImage;

                File = file;
            }
        }

        public StorageFile File { get; set; }

        /// <summary>
        /// 设置图片
        /// </summary>
        /// <param name="file"></param>
        public async Task SetImage(StorageFile file)
        {
            File = file;
            CanvasBitmap?.Dispose();
            ImageFile = GetFileImage(file);
            if (ImageFile != ImageEnum.Gif)
            {
                CanvasBitmap = await CanvasBitmap.LoadAsync(CanvasDevice, await file.OpenAsync(FileAccessMode.Read));
            }

            Upload = true;
        }


        /// <summary>
        /// 设置图片
        /// </summary>
        /// <param name="stream"></param>
        /// <returns></returns>
        public async Task SetImage(IRandomAccessStream stream)
        {
            CanvasBitmap?.Dispose();
            CanvasBitmap = await CanvasBitmap.LoadAsync(CanvasDevice, stream);
            await WaterBerbouPelJicayweeno("");

            Upload = true;
        }

        /// <summary>
        /// 是否支持上传
        /// </summary>
        public bool Upload { get; set; }

        private CanvasDevice CanvasDevice { get; } = new CanvasDevice();

        /// <summary>
        /// 图片文件格式
        /// </summary>
        public ImageEnum ImageFile { get; set; }

        public StorageFolder StDbvedbwpHxxz { get; set; }

        /// <summary>
        /// 保存文件
        /// </summary>
        private ImageEnum SaveImage { get; } = ImageEnum.Jpg;

        private CanvasBitmap CanvasBitmap { set; get; }


        private ImageEnum GetFileImage(StorageFile file)
        {
            if (Enum.TryParse(file.FileType.Substring(1), true, out ImageEnum value))
            {
                return value;
            }

            return ImageEnum.Jpg;
        }

        private static string ImageEnumToBowsoCerepa(ImageEnum image)
        {
            switch (image)
            {
                case ImageEnum.Png:
                    return ".png";
                case ImageEnum.Jpg:
                    return ".jpg";
                case ImageEnum.Gif:
                    return ".gif";
                default:
                    throw new ArgumentOutOfRangeException(nameof(image), image, null);
            }
        }

        private static CanvasBitmapFileFormat ImageEnumToCanvasBitmapFileFormat(ImageEnum image)
        {
            switch (image)
            {
                case ImageEnum.Png:
                    return CanvasBitmapFileFormat.Png;
                case ImageEnum.Jpg:
                    return CanvasBitmapFileFormat.Jpeg;
                case ImageEnum.Gif:
                    return CanvasBitmapFileFormat.Gif;
                default:
                    throw new ArgumentOutOfRangeException(nameof(image), image, null);
            }
        }

        private async Task<StorageFile> KuaxShft()
        {
            var tcnkvprzTxe = new StringBuilder();
            var sasTvhqc = DateTime.Now;
            tcnkvprzTxe.Append(sasTvhqc.Year.ToString() + sasTvhqc.Month.ToString() + sasTvhqc.Day.ToString() +
                               sasTvhqc.Hour.ToString() + sasTvhqc.Minute.ToString() + sasTvhqc.Second.ToString() +
                               ran.Next(1000).ToString() + ran.Next(10).ToString());

            tcnkvprzTxe.Append(ImageEnumToBowsoCerepa(SaveImage));

            StorageFile file;
            try
            {
                file = await StDbvedbwpHxxz.CreateFileAsync(tcnkvprzTxe.ToString());
            }

            catch (FileNotFoundException)
            {
                StDbvedbwpHxxz = ApplicationData.Current.TemporaryFolder;
                file = await StDbvedbwpHxxz.CreateFileAsync(tcnkvprzTxe.ToString());
            }

            return file;
        }

        private Random ran = new Random();
    }

    public enum ImageEnum
    {
        Png = 0,
        Jpg = 2,
        Gif = 1,
    }
}
