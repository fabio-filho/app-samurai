﻿using System.Collections.Generic;
using Domain.Entities;
using Domain.Interfaces;
using HtmlAgilityPack;
using System.Linq;
using System;
using Robot.GooglePlay.Helpers;

namespace Robot.GooglePlay.SearchApp
{
    public class SearchAppByLink : ISearchApp
    {
        public ISearchApp SearchApp { get; }

        public SearchAppByLink(ISearchApp searchApp)
        {
            SearchApp = searchApp;
        }

        private static HtmlNode[] GetDescendents(HtmlNode cardDiv,
            string targetObject, string attribute, string comparationValue)
        {

            return cardDiv.Descendants(targetObject)
                    .Where(ts => ts.GetAttributeValue(attribute, string.Empty) == comparationValue)
                    .ToArray();
        }

        public IEnumerable<App> Search(string q, string country)
        {
            if (IsALink(q) == false)
                return SearchApp?.Search(q, country);
                        
            return MapHtmlToApps(q);
        }

        public IEnumerable<App> MapHtmlToApps(string q)
        {
            IList<App> apps = new List<App>();

            HtmlWeb web = new HtmlWeb();
            HtmlDocument html = web.Load(q);

            string divClass = "card-content id-track-click id-track-impression";
            var cardDivs = html.DocumentNode.Descendants("div")
                .Where(ts => ts.GetAttributeValue("class", string.Empty) == divClass);

            foreach (var cardDiv in cardDivs)
            {
                var names = GetDescendents(cardDiv, "a", "class", "title");
                var subtitles = GetDescendents(cardDiv, "a", "class", "subtitle");
                var prices = GetDescendents(cardDiv, "span", "class", "display-price");
                var descriptions = GetDescendents(cardDiv, "div", "class", "description");
                var ratings = GetDescendents(cardDiv, "div", "class", "current-rating");
                var icon = GetDescendents(cardDiv, "img", "class", "cover-image");

                for (int index = 0; index < names.Length; index++)
                {
                    string rating = "0";
                    if (ratings.Length > 0)
                        rating = GooglePlayUtils.GetRating(
                                ratings[index].GetAttributeValue("style", string.Empty)
                        );

                    App app = new App()
                    {
                        Name = names[index].InnerText,
                        SubTitle = subtitles[index].InnerText,
                        Price = prices.Length != 0 ? prices[index].InnerText : string.Empty,
                        Description = descriptions[index].InnerText,
                        Package = names[index].GetAttributeValue("href", string.Empty).Split('=')[1],
                        Rating = rating,
                        Icon = icon[index].GetAttributeValue("src", string.Empty)
                    };
                    apps.Add(app);
                }
            }
            return apps;
        }

        private bool IsALink(string q)
        {
            return Uri.IsWellFormedUriString(q, UriKind.Absolute);
        }
    }
}
