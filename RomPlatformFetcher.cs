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
        public static HtmlNodeCollection listRoms {get; set;}

        public RomPlatformFetcher(string uri){
            listMenuLinks = UriContentFetcher.getContent(uri,
             "//td[@class='d-block d-sm-none text-center']/a[contains(@href,'/roms/')]",
              true);
        }

        public static void romIndexesFetcher(HtmlNode platformUri, int page){
            listRoms = UriContentFetcher.getContent(@"https://www.retrostic.com" + platformUri.Attributes["href"].Value + "/page/" + page,
            "//td[@class='d-block d-sm-none text-center']/a[contains(@href,'/roms/')]",
            true);
            page++;
        }

        public static void romSearchListFetcher(string uri){
            listRoms = UriContentFetcher.getContent(uri,
            "//td[@class='d-block d-sm-none text-center']/a[contains(@href,'/roms/')]",
            true);
        }
    }
}