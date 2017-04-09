﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Entities;
using Domain.Interfaces;
using HtmlAgilityPack;

namespace Robot.GooglePlay.GetApp
{
    class GetApp : IGetApp
    {
        public App Get(string urlApp, string country)
        {
            HtmlWeb web = new HtmlWeb();
            HtmlDocument html = web.Load(urlApp);
            
            string divClass = "details-wrapper apps square-cover id-track-partial-impression id-deep-link-item";
            var divs = GetNode(html.DocumentNode, "div", "class", divClass);

            App app = new App()
            {
                Name = GetNode(divs, "div", "class", "id-app-title").InnerText,
                SubTitle = GetNode(divs, "span", "itemprop", "name").InnerText,
                Description = GetNode(divs, "div", "jsname", "C4s9Ed").InnerText,
                Icon = GetNode(divs, "img", "class", "cover-image")
                    .GetAttributeValue("src", string.Empty),                
                Category = GetNode(divs, "a", "class", "document-subtitle category")
                    .GetAttributeValue("href", string.Empty).Split('/')[3],
                Screenshots = GetNode(divs, "div", "class", "thumbnails")
                    .Descendants("img")
                    .Select(ts => ts.GetAttributeValue("src", string.Empty))
                    .ToArray()
            };
            
            return app;
        }

        private static HtmlNode GetNode(HtmlNode node,
           string targetObject, string attribute, string comparationValue)
        {
            return node.Descendants(targetObject)
                .FirstOrDefault(ts => ts.GetAttributeValue(attribute, string.Empty) == comparationValue);
        }
    }
}