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
    public static class RomDownloader{
        //Static variable for the platform name
        public static string platform {get; set;}
        //Static variable for the platform directory and check the existance of it
        public static string platformDirectory {get; set;}
        //Static variable to track the number of downloads
        public static int totalRomsDownloaded = 0;

        //Function to get a HTML site and start downloading the resource (a batch of ROMs or a single one)
        public static void romDownloader(HtmlNode node){
            //Variable to contain the URL of the ROM to be downloaded
            string uri = node.Attributes["href"].Value;
            //Variable to contain the data to make a request to the server and get access to the download page
            var sessionData = UriContentFetcher.getContent(@"https://www.retrostic.com" + uri, "//input[@type='hidden']", true);
            //Variable to contain the name of the file as it is downloaded
            var fileNameFetcher = UriContentFetcher.getContent(@"https://www.retrostic.com" + uri, "//td[contains(text(), '.zip') or contains(text(), '.bin')]", false);
            //Update class attribute with the root of the platform
            platformDirectory = $@"{DirectoryFetcher.currentDirectory}/roms/{platform}";
            
            //We check if the ROM is already in such directory (library)
            if(checkFileInDirectory(node)){
                try{
                    //Variable that contains the final site to extract the URL. We should pull a request with data gathered previously and contained in sessionData variable (nodes)
                    string romDownloadPageContent = pullRequest(@"https://www.retrostic.com" + uri + "/download", sessionData[0].Attributes["value"].Value, sessionData[1].Attributes["value"].Value, sessionData[2].Attributes["value"].Value);
                    //Variable that contains the whole script that we will use to extract the URL to download the ROM
                    string romScript = getScript(romDownloadPageContent);
                    //Variable that contains the URL to download the ROM
                    string urlDownload = getDownloadUrl(romScript);

                    //Once we have the URL we create a temp WebClient object to download in sync mode
                    //NOTE: To avoid scraping alert from their server, I rather prefer to download the ROMs in sync mode
                    using (var wClient = new WebClient()){
                        Uri uriRom = new Uri(urlDownload);
                        string root = DirectoryFetcher.currentDirectory + "/roms/" + RomDownloader.platform + "/";
                        string fileName = @root + fileNameFetcher.InnerText;
                        
                        try{
                            wClient.DownloadFile(@uriRom, fileName);
                            Thread.Sleep(1000);
                            WriteLine($"Downloading roms for: {RomDownloader.platform}");
                            WriteLine($"--- {node.InnerText}");
                            WriteLine($"Downloaded!\n");
                            //Update of the variable to track the number of downloads
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

        //Function to request the server with certain data extracted from the HTML page
        public static string pullRequest(string website, string param1, string param2, string param3){
            //Creation of a temp WebClient object to make a server request
            using (var client = new WebClient()){
        
                var values = new NameValueCollection();
                values["rom_url"] = param1;
                values["console_url"] = param2;
                values["session"] = param3;
                //Making a request and assigning content to variable
                var response = client.UploadValues(website, values);
                Thread.Sleep(1000);
                //Return the HTML content as string
                return Encoding.Default.GetString(response);
            }
        }

        //Function to extract the script from the HTML content
        public static string getScript(string romContent){
            var romPage = new HtmlDocument();
            romPage.LoadHtml(romContent);
            var romLink = romPage.DocumentNode.SelectSingleNode("//script[@type='text/javascript']");
            return romLink.OuterHtml;
        }

        //Function to check if the ROM already exists in the library
        public static bool checkFileInDirectory(HtmlNode node){
            //Variable to keep the list of files
            string[] files = Directory.GetFiles(platformDirectory);
            var filesNames = new List<string>();

            //As the list of files is a file path collection, we want to keep only the name of the file and also be able
            //to use the function Contains()
            foreach(string file in files){
                filesNames.Add(Path.GetFileName(file));
            }

            if(!filesNames.Contains(node.InnerText + ".zip") || !filesNames.Contains(node.InnerText + ".bin")){
                return true;
            }
            return false;
        }

        //Function to extrac the URL from the script
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