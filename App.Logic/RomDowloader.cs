using System;
using static System.Console;
using HtmlAgilityPack;
using System.Net;
using System.IO;
using System.Collections.Generic;
using System.Threading;
using System.Collections.Specialized;
using System.Text;

namespace RomScraper
{
    //Class to download either a whole platform or a single ROM
    public static class RomDownloader
    {
        //Static variable for the platform name
        public static string platform {get; set;}
        //Static variable for the platform directory and check the existance of it
        public static string platformDirectory {get; set;}
        //Static variable to track the number of downloads
        public static int totalRomsDownloaded = 0;

        public static void romDownloader(HtmlNode node){
            string uri = node.Attributes["href"].Value;
            var sessionData = UriContentFetcher.getContent(@"https://www.retrostic.com" + uri, "//input[@type='hidden']", true);
            var fileNameFetcher = UriContentFetcher.getContent(@"https://www.retrostic.com" + uri, "//td[contains(text(), '.zip') or contains(text(), '.bin')]", false);
            
            platformDirectory = $@"{DirectoryFetcher.currentDirectory}/roms/{platform}";

            if(checkFileInDirectory(node)){
                try{
                    string romDownloadPageContent = pullRequest(@"https://www.retrostic.com" + uri + "/download", sessionData[0].Attributes["value"].Value, sessionData[1].Attributes["value"].Value, sessionData[2].Attributes["value"].Value);
                    string romScript = getScript(romDownloadPageContent);
                    string urlDownload = getDownloadUrl(romScript);

                    using (WebClient wClient = new WebClient()){
                        Uri uriRom = new Uri(urlDownload);
                        string root = DirectoryFetcher.currentDirectory + "/roms/" + RomDownloader.platform + "/";
                        string fileName = @root + fileNameFetcher.InnerText;
                        
                        try{
                            wClient.DownloadFile(@uriRom, fileName);
                            Thread.Sleep(1000);
                            WriteLine($"Downloading roms for: {RomDownloader.platform}");
                            WriteLine($"--- {node.InnerText}");
                            WriteLine($"Downloaded!\n");
                            totalRomsDownloaded++;
                        }
                        catch (Exception e){
                            WriteLine($"Rom: {node.InnerText} - Download Failed // Error: {e.Message}\n");
                        }
                    }
                }
                catch{
                    WriteLine($"Rom '{node.InnerText}' not available\n");
                }
            }
            else{
                WriteLine($"{node.InnerText} exist already in your library\n");
            }
        }

        public static string pullRequest(string website, string param1, string param2, string param3){
            using (var client = new WebClient()){
        
                var values = new NameValueCollection();
                values["rom_url"] = param1;
                values["console_url"] = param2;
                values["session"] = param3;

                var response = client.UploadValues(website, values);
                Thread.Sleep(1000);
                return Encoding.Default.GetString(response);
            }
        }

        public static string getScript(string romContent){
            var romPage = new HtmlDocument();
            romPage.LoadHtml(romContent);
            var romLink = romPage.DocumentNode.SelectSingleNode("//script[@type='text/javascript']");
            return romLink.OuterHtml;
        }

        public static bool checkFileInDirectory(HtmlNode node){

            string[] files = Directory.GetFiles(platformDirectory);
            var filesNames = new List<string>();

            foreach(string file in files){
                filesNames.Add(Path.GetFileName(file));
            }

            if(!filesNames.Contains(node.InnerText + ".zip") || !filesNames.Contains(node.InnerText + ".bin")){
                return true;
            }
            return false;
        }

        public static string getDownloadUrl(string romScript){
            int lengthSubs;
            if(romScript.IndexOf(".zip")!=-1){
                lengthSubs = romScript.IndexOf(".zip") - romScript.IndexOf("https");
            }
            else{
                lengthSubs = romScript.IndexOf(".bin") - romScript.IndexOf("https");
            }
            return romScript.Substring(romScript.IndexOf("https"),lengthSubs+4);
        }
    }
}