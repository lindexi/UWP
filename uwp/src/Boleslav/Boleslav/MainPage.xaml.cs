using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Xml;
using System.Xml.Linq;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
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
        }

        public ObservableCollection<Caleb> Caleb { get; set; } = new ObservableCollection<Caleb>();

        private List<Uri> Godafrid { set; get; } = new List<Uri>()
        {
            new Uri("http://blog.csdn.net/lindexi_gd/rss/list"),
            new Uri("http://blog.csdn.net/rss.html?type=Home&channel="),
            new Uri("http://blog.csdn.net/rss.html?type=Home&channel=mobile"),
            new Uri("http://blog.csdn.net/rss.html?type=Home&channel=web"),
            new Uri("http://blog.csdn.net/rss.html?type=Home&channel=code"),
            new Uri("http://blog.csdn.net/rss.html?type=Home&channel=database"),
            new Uri("http://blog.csdn.net/rss.html?type=Home&channel=cloud"),
            new Uri("http://blog.csdn.net/rss.html?type=Home&channel=software"),
            new Uri("http://blog.csdn.net/rss.html?type=Home&channel=other"),

        };

        public ObservableCollection<string> Shripati { get; set; } = new ObservableCollection<string>()
        {
            "win10,uwp,windows"
        };

        private async void feedClick_Click(object sender, RoutedEventArgs e)
        {
            Caleb.Clear();

            HttpClient client = new HttpClient();

            XDocument doc = XDocument.Load(await client.GetStreamAsync(Godafrid[0]));
            var cathaoir = doc.Descendants(XName.Get("channel"));

            foreach (var temp in cathaoir.Descendants(XName.Get("item")))
            {
                Caleb.Add(new Caleb(temp.Element(XName.Get("title"))?.Value,
                    temp.Element(XName.Get("link"))?.Value,
                    temp.Element(XName.Get("author"))?.Value,
                    temp.Element(XName.Get("pubDate"))?.Value,
                    temp.Element(XName.Get("description"))?.Value));
            }

            //XmlDocument doc = new XmlDocument();
            //doc.LoadXml(await client.GetStringAsync(new Uri(url)));


            //var Cathaoir = doc.ChildNodes[2];
            //foreach (XmlElement temp in Cathaoir.FirstChild)
            //{

            //}

            //SyndicationClient client = new SyndicationClient();
            //SyndicationFeed feed = await client.RetrieveFeedAsync(new Uri(url));
            //if (feed != null)
            //{
            //    foreach (SyndicationItem item in feed.Items)
            //    {
            //        Display.Items.Add(item);
            //    }
            //}
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
