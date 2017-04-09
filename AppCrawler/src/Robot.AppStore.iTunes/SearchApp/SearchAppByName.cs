﻿using System;
using System.Collections.Generic;
using Domain.Entities;
using Domain.Interfaces;
using Domain;

namespace Robot.AppStore.iTunes.SearchApp
{
    public class SearchAppByName : BaseSearchAppByName
    {
        public SearchAppByName(ISearchApp searchApp) : base(searchApp)
        {            
        }

        protected override string getSearchUrl(string q, string country)
        {
            return $"http://www.apple.com/{country}/search/{q}?src=serp";
        }
    }
}
