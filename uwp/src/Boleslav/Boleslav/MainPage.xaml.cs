using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;
using Newtonsoft.Json;
using Windows.ApplicationModel;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;
using Windows.System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Animation;
using Windows.UI.Xaml.Navigation;
using Windows.Web.Syndication;

// https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x804 上介绍了“空白页”项模板

namespace Boleslav
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();
            Read();

            App.Current.Suspending += OnSuspending;
        }

        private async void OnSuspending(object sender, SuspendingEventArgs e)
        {
            var deferral = e.SuspendingOperation.GetDeferral();

            var shripati = new List<Shripati>();
            shripati.AddRange(Shripati);
            var folder = ApplicationData.Current.RoamingFolder;
            var file = await folder.CreateFileAsync("Shripati", CreationCollisionOption.ReplaceExisting);
            string str = JsonConvert.SerializeObject(shripati);
            await FileIO.WriteTextAsync(file, str);

            deferral.Complete();
        }


        private async void Read()
        {
            //从本地获取

            var folder = ApplicationData.Current.RoamingFolder;
            //尝试获取文件
            try
            {
                var file = await folder.GetFileAsync("Shripati");
                string str = await FileIO.ReadTextAsync(file);
                var shripati = JsonConvert.DeserializeObject<List<Shripati>>(str);
                Shripati.Clear();
                foreach (var temp in shripati)
                {
                    Shripati.Add(temp);
                }
            }
            catch
            {

            }
        }

        public ObservableCollection<Caleb> Caleb { get; set; } = new ObservableCollection<Caleb>();

        private List<Uri> Godafrid { set; get; } = new List<Uri>()
        {
            //new Uri("http://blog.csdn.net/lindexi_gd/rss/list"),
            new Uri("http://blog.csdn.net/rss.html?type=Home&channel="),
            new Uri("http://blog.csdn.net/rss.html?type=Home&channel=mobile"),
            new Uri("http://blog.csdn.net/rss.html?type=Home&channel=web"),
            new Uri("http://blog.csdn.net/rss.html?type=Home&channel=code"),
            new Uri("http://blog.csdn.net/rss.html?type=Home&channel=database"),
            new Uri("http://blog.csdn.net/rss.html?type=Home&channel=cloud"),
            new Uri("http://blog.csdn.net/rss.html?type=Home&channel=software"),
            new Uri("http://blog.csdn.net/rss.html?type=Home&channel=other"),

        };

        public ObservableCollection<Shripati> Shripati { get; set; } = new ObservableCollection<Shripati>()
        {
            "win10,windows","uwp","wpf","java"
        };

        private async void feedClick_Click(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            if (button == null)
                return;

            button.IsEnabled = false;

            Caleb.Clear();

            foreach (var temp in Godafrid)
            {
                var caleb = await CharmainGodafrid(temp);
                caleb = KaranGodafrid(caleb);
                foreach (var c in caleb)
                {
                    Caleb.Add(c);
                }
            }
            KaranGodafrid(Caleb);
            button.IsEnabled = true;

            TextBlock.Visibility = Visibility.Visible;
            TextBlock.Opacity = 1;
            TextBlock.Text = "获得文章" + Caleb.Count;
            Storyboard storyboard = new Storyboard();
            var animation = new DoubleAnimation { From = 1, To = 0, Duration = new Duration(TimeSpan.FromSeconds(5)) };
            Storyboard.SetTarget(animation, TextBlock);
            Storyboard.SetTargetProperty(animation, "Opacity");
            storyboard.Children.Add(animation);
            storyboard.Completed += (s, _) =>
            {
                TextBlock.Visibility = Visibility.Collapsed;
            };
            storyboard.Begin();

        }

        private void KaranGodafrid(ObservableCollection<Caleb> caleb)
        {
            //去除重复
            for (int i = 0; i < caleb.Count; i++)
            {
                for (int j = i + 1; j < caleb.Count; j++)
                {
                    if (caleb[i].Godafrid == caleb[j].Godafrid)
                    {
                        caleb.RemoveAt(j);
                        j--;
                    }
                }
            }
        }

        /// <summary>
        /// 过滤
        /// </summary>
        /// <param name="caleb"></param>
        private List<Caleb> KaranGodafrid(List<Caleb> caleb)
        {
            return caleb.Where(KaranGodafrid).ToList();
        }

        private bool KaranGodafrid(Caleb caleb)
        {
            //如果存在标题或描述 关键词，那么返回true
            return Shripati.Where(str => !string.IsNullOrEmpty(str.Eadwulf)).Select(str => str.Eadwulf.Split(',')).Any(str =>
                  str.Where(temp => !string.IsNullOrEmpty(temp)).All(temp => caleb.Eadwulf.Contains(temp) || caleb.Celso.Contains(temp)));
        }



        private async Task<List<Caleb>> CharmainGodafrid(Uri godafrid)
        {
            HttpClient client = new HttpClient();
            XDocument doc = XDocument.Load(await client.GetStreamAsync(godafrid));
            var cathaoir = doc.Descendants(XName.Get("channel"));
            return cathaoir.Descendants(XName.Get("item"))
                .Select(temp => new Caleb(temp.Element(XName.Get("title"))?.Value,
                    temp.Element(XName.Get("link"))?.Value, temp.Element(XName.Get("author"))?.Value,
                    temp.Element(XName.Get("pubDate"))?.Value, temp.Element(XName.Get("description"))?.Value)).ToList();
        }

        private void KaranButton_OnClick(object sender, RoutedEventArgs e)
        {
            Shripati.Add("");
        }

        private async void ListView_OnItemClick(object sender, ItemClickEventArgs e)
        {
            Caleb caleb = (Caleb) e.ClickedItem;
            await Launcher.LaunchUriAsync(new Uri(caleb.Godafrid));
        }

        private void KaranCarsen(object sender, string e)
        {
            //删除规则
            Shripati.Remove((Shripati) ((UserControl) sender).DataContext);
        }
    }

    /// <summary>
    /// 关键词
    /// </summary>
    public class Shripati
    {
        public string Eadwulf { get; set; }
        public static implicit operator Shripati(string str)
        {
            return new Shripati()
            {
                Eadwulf = str
            };
        }

        public static implicit operator string(Shripati shripati)
        {
            return shripati.Eadwulf;
        }
    }

    /// <summary>
    /// 博客的标题、发布时间、描述
    /// </summary>
    public class Caleb
    {
        public Caleb(string eadwulf, string godafrid, string zbignev, string witek, string celso)
        {
            Eadwulf = eadwulf;
            Godafrid = godafrid;
            Zbignev = zbignev;
            Witek = witek;
            Celso = celso;
        }

        /// <summary>
        /// title
        /// </summary>
        public string Eadwulf { get; set; }

        /// <summary>
        /// link
        /// </summary>
        public string Godafrid { get; set; }

        /// <summary>
        /// author
        /// </summary>
        public string Zbignev { get; set; }

        /// <summary>
        /// pubDate
        /// </summary>
        public string Witek { get; set; }

        /// <summary>
        /// description
        /// </summary>
        public string Celso { get; set; }
    }
}
