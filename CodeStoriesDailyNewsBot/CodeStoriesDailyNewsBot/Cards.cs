// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System.Collections.Generic;
using System.IO;
using System.ServiceModel.Syndication;
using CodeStoriesDailyNewsBot.RSSFeeds;
using Microsoft.Bot.Schema;
using Newtonsoft.Json;
using System.Linq;

namespace Microsoft.BotBuilderSamples
{
    public static class Cards
    {    

        public static List<HeroCard> GetAnnouncements()
        {
            List<HeroCard> CardList = new List<HeroCard>();

           CardList = GetResults("https://azurecomcdn.azureedge.net/en-us/blog/topics/announcements/feed/");

            return CardList;
        }

        public static List<HeroCard> GetBigData()
        {
            List<HeroCard> CardList = new List<HeroCard>();

            CardList = GetResults("https://azurecomcdn.azureedge.net/en-us/blog/topics/big-data/feed/");

            return CardList;
        }

        public static List<HeroCard> GetCloudStrategy()
        {
            List<HeroCard> CardList = new List<HeroCard>();

            CardList = GetResults("https://azurecomcdn.azureedge.net/en-us/blog/topics/cloud-strategy/feed/");

            return CardList;
        }

        public static List<HeroCard> GetDeveloper()
        {
            List<HeroCard> CardList = new List<HeroCard>();

            CardList = GetResults("https://azurecomcdn.azureedge.net/en-us/blog/topics/developer/feed/");

            return CardList;
        }

        public static List<HeroCard> GetAll()
        {
            List<HeroCard> CardList = new List<HeroCard>();

            CardList = GetResults("https://azurecomcdn.azureedge.net/en-us/blog/feed/");

            return CardList;
        }

        public static List<HeroCard> GetResults(string url)
        {
            List<HeroCard> CardList = new List<HeroCard>();

            RSS rss = new RSS(url);
            SyndicationFeed Feed = rss.Get();

            if (Feed!=null)
            {
                foreach (var item in Feed.Items.Take(5))
                {
                    var heroCard = new HeroCard
                    {
                        Title = item.Title.Text,
                        Subtitle = item.Categories.FirstOrDefault().Name,
                        Text = item.Summary.Text,
                        Buttons = new List<CardAction> { new CardAction(ActionTypes.OpenUrl, "Link", value: item.Links.FirstOrDefault().Uri.ToString()) }
                    };
                    CardList.Add(heroCard);
                }
            }
                       
            return CardList;
        }



    }
}
