using System;
using System.Net.Http;
using static System.Console;
using System.Threading.Tasks;
using HtmlAgilityPack;
using System.Net;
using System.IO;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;

namespace RomScraper
{
    public class RomPlatformFetcher
    {
        public static HtmlDocument uriContent {get; set;}
        public static HtmlNodeCollection listMenuLinks {get; set;}
        public static HtmlNodeCollection listIndexes {get; set;}

        

        public RomPlatformFetcher(string uri){
            listMenuLinks = UriContentFetcher.getContent(uri, "//ul[@class='desktop-menu']/li/a", true);
        }

        public static void romIndexesFetcher(HtmlNode platformUri){
            listIndexes = UriContentFetcher.getContent(@"https://www.freeroms.com/" + platformUri.Attributes["href"].Value, "//div[@class='page']/a", true);
 
        }
    }
}