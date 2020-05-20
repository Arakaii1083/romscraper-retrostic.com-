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
        public static int totalRomsDownloaded;

        public RomPlatformFetcher(string uri){
            HtmlWeb htmlweb = new HtmlWeb();
            var htmlIndex = htmlweb.Load(uri);
            uriContent = htmlIndex;
        }

        public static HtmlNodeCollection platformLinks(){
            var listLinks = uriContent.DocumentNode.SelectNodes("//ul[@class='desktop-menu']/li/a");
            return listLinks;
        }
        public static void romIndexesFetcher(HtmlNode platformUri){
            string uri = @"https://www.freeroms.com/" + platformUri.Attributes["href"].Value;
            HtmlWeb htmlweb = new HtmlWeb();
            var htmlIndex = htmlweb.Load(uri);
            uriContent = htmlIndex;

            var listLinks = uriContent.DocumentNode.SelectNodes("//div[@class='page']/a");

            foreach(var node in listLinks){
                WriteLine(node.InnerText + ": ");
                RomDowloader.romDownloader(node.Attributes["href"].Value);
            }
            WriteLine($"Total downloaded: {totalRomsDownloaded}");
        }
    }
}