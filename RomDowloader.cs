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
    public class RomDowloader
    {
        public static string platform;
        public static void romDownloader(string uri){
            int count = 0;
            bool flag = false;
            
            var html = @"https://www.freeroms.com/"+uri;
            HtmlWeb htmlweb = new HtmlWeb();
            var htmlIndex = htmlweb.Load(html);
            
            var listLinks = htmlIndex.DocumentNode.SelectNodes("//div[@class='rom-tr title']/a");
            RomPlatformFetcher.totalRomsDownloaded += listLinks.Count;            

            string[] files = Directory.GetFiles($@"{DirectoryFetcher.currentDirectory}/roms/{RomDowloader.platform}");
            var filesNames = new List<string>();

            foreach(string file in files){
                filesNames.Add(Path.GetFileName(file));
            }

            foreach(var node in listLinks){
                flag = true;
                if(!filesNames.Contains(node.InnerText + ".zip")){
                    var htmlRoms = @"https://www.freeroms.com/"+node.Attributes["href"].Value;
                    HtmlWeb htmlweb2 = new HtmlWeb();
                    var htmlRom = htmlweb2.Load(htmlRoms);

                    var romLink = htmlRom.DocumentNode.SelectSingleNode("//script[@language='javascript']");
                    string romScript = romLink.OuterHtml;
                    int lengthSubs = romScript.IndexOf(".zip") - romScript.IndexOf("http");
                    string urlDownload = romScript.Substring(romScript.IndexOf("http"),lengthSubs+4);

                    WebClient wClient = new WebClient();
                    Uri uriRom = new Uri(urlDownload);
                    string root = DirectoryFetcher.currentDirectory + "/roms/" + RomDowloader.platform + "/";
                    wClient.DownloadFile(uriRom, @root + node.InnerText + ".zip");
                    count++;
                    WriteLine($"Downloaded {count} out of {listLinks.Count}");

                    flag = false;
                }                
            }
            if(flag){
                WriteLine("You have the collection already");
            }
        }
    }
}