using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel.Syndication;
using System.Threading.Tasks;
using System.Xml;

namespace CodeStoriesDailyNewsBot.RSSFeeds
{
    public class RSS
    {
        string _url;
        public RSS(string URL)
        {
            _url = URL;
        }

        public SyndicationFeed Get()
        {
            try
            {           
                XmlReader reader = XmlReader.Create(_url);
                SyndicationFeed feed = SyndicationFeed.Load(reader);
                reader.Close();
                return feed;
            }
            catch (Exception)
            {
                return null;
            }
        }

    }
}
