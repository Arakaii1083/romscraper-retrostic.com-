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
    public static class RomDownloader
    {

        // public static HtmlNodeCollection romIndexList {set; get;}
        public static string platform;
        public static string platformDirectory = $@"{DirectoryFetcher.currentDirectory}/roms/{platform}";
        public static int totalRomsDownloaded = 0;
        public static void romDownloader(string uri){
            int count = 0;
            bool flag = true;
            var filesNames = new List<string>(Directory.GetFiles(platformDirectory));
            
            var listRomsLinks = UriContentFetcher.getContent(@"https://www.freeroms.com/"+uri, "//div[@class='rom-tr title']/a", true);

            foreach(var node in listRomsLinks){
                if(!filesNames.Contains(node.InnerText + ".zip")){

                    var romLink = UriContentFetcher.getContent(@"https://www.freeroms.com/"+node.Attributes["href"].Value, "//script[@language='javascript']", false);

                    string romScript = romLink.OuterHtml;
                    int lengthSubs = romScript.IndexOf(".zip") - romScript.IndexOf("http");
                    string urlDownload = romScript.Substring(romScript.IndexOf("http"),lengthSubs+4);

                    WebClient wClient = new WebClient();
                    Uri uriRom = new Uri(urlDownload);
                    string root = DirectoryFetcher.currentDirectory + "/roms/" + RomDownloader.platform + "/";
                    
                    try{
                        wClient.DownloadFile(uriRom, @root + node.InnerText + ".zip");
                        count++;
                        WriteLine($"--- {node.InnerText}");
                        WriteLine($"Downloaded {count} out of {listRomsLinks.Count}");
                        totalRomsDownloaded++;
                    }
                    catch{
                        WriteLine($"Rom: {node.InnerText} - Download Failed");
                    }
                    flag = false;
                }                
            }
            if(flag){
                WriteLine("You have the collection already");
            }
        }
    }
}