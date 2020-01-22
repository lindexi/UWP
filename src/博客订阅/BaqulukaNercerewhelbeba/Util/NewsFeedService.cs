using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;
using Microsoft.SyndicationFeed;
using Microsoft.SyndicationFeed.Atom;
using Microsoft.SyndicationFeed.Rss;

namespace BaqulukaNercerewhelbeba.Util
{
    public class NewsFeedService
    {
        private readonly string _feedUri;

        public NewsFeedService(string feedUri)
        {
            _feedUri = feedUri;
        }

        private XmlFeedReader GetXmlFeedReader(string xml, XmlReader xmlReader)
        {
            var xDocument = XDocument.Load(new StringReader(xml));
            var rootName = xDocument.Root.Name;
            if (rootName.Namespace.NamespaceName.Contains("Atom", StringComparison.OrdinalIgnoreCase))
            {
                return new AtomFeedReader(xmlReader);
            }

            if (rootName.LocalName.Contains("feed", StringComparison.OrdinalIgnoreCase))
            {
                return new AtomFeedReader(xmlReader);
            }

            if (rootName.ToString().Contains("rss", StringComparison.OrdinalIgnoreCase))
            {
                return new RssFeedReader(xmlReader);
            }

            return new AtomFeedReader(xmlReader);
        }

        public async Task<List<ISyndicationItem>> GetNewsFeed()
        {
            var rssNewsItems = new List<ISyndicationItem>();

            var httpClient = new HttpClient()
            {
                Timeout = TimeSpan.FromMinutes(10)
            };
            var xml = await httpClient.GetStringAsync(_feedUri);
            Console.WriteLine($"Get {_feedUri}");

            using (var xmlReader = XmlReader.Create(new StringReader(xml)))
            {
                XmlFeedReader feedReader = GetXmlFeedReader(xml, xmlReader);
                Console.WriteLine("Read");
                while (await feedReader.Read())
                {
                    try
                    {
                        if (feedReader.ElementType == SyndicationElementType.Item)
                        {
                            ISyndicationItem item = await feedReader.ReadItem();
                            rssNewsItems.Add(item);
                        }
                    }
                    catch (Exception e)
                    {
                    }
                }
            }

            return rssNewsItems.OrderByDescending(p => p.LastUpdated).ToList();
        }
    }
}