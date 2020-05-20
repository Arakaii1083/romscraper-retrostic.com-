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

namespace romscraper
{
    public class RomPlatformFetcher
    {
        private string uriContent {get; set;}
        public static int totalRomsDownloaded;

        public RomPlatformFetcher(string uri){
            HtmlWeb htmlweb = new HtmlWeb();
            var htmlIndex = htmlweb.Load(uri);
        }

        public void platformLinks(){
            var listLinks = this.uriContent.DocumentNode.SelectNodes("//ul[@class='desktop-menu']/li/a");
            // foreach(var node in listLinks){
            //     if(node.InnerText!="Links" || node.InnerText!="Flash Games"){
            //         // Console.WriteLine(node.Attributes["href"].Value);
            //     }
            // }
        }
        public void romIndexesFetcher(HtmlDocument platformUri){
            var listLinks = platformUri.DocumentNode.SelectNodes("//div[@class='page']/a");
            foreach(var node in listLinks){
                WriteLine(node.InnerText + ": ");
                RomDowloader.romDownloader(node.Attributes["href"].Value);
            }
            WriteLine($"Total downloaded: {totalRomsDownloaded}");
        }
    }
}