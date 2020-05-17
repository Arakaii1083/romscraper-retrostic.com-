using System;
using System.Net.Http;
using static System.Console;
using System.Threading.Tasks;
using HtmlAgilityPack;
using System.Net;
using System.IO;
using System.Collections.Generic;

namespace romscraper
{
    class Program
    {
        static void Main(string[] args)
        {
            /******URI FETCHER******/
            // HttpClient client = new HttpClient();
            // var response = await client.GetAsync("https://www.freeroms.com/");
            // var pageContents = await response.Content.ReadAsStringAsync();

            /******LIST FETCHER******/
            // var web = @"source\index2.html";

            // var pageDocument = new HtmlDocument();
            // pageDocument.Load(web);
            
            // var listLinks = pageDocument.DocumentNode.SelectNodes("//ul[@class='desktop-menu']/li/a");
            // foreach(var node in listLinks){
            //     if(node.InnerText!="Links" || node.InnerText!="Flash Games"){
            //         Console.WriteLine(node.Attributes["href"].Value);
            //     }
            // }

            /******INDEX FETCHER******/
            // var web2 = @"source/index_amiga.html";

            // var pageDocumentPlatform = new HtmlDocument();
            // pageDocumentPlatform.Load(web2);

            // var listLinks = pageDocumentPlatform.DocumentNode.SelectNodes("//div[@class='page']/a");
            // foreach(var node in listLinks){
            //     Write(node.InnerText + ": ");
            //     Console.WriteLine(node.Attributes["href"].Value);
            // }

            /******ROM/URL FETCHER******/
            int count = 0;
            var web3 = @"source/index_amiga_a.html";

            var pageDocumentPlatformIndex = new HtmlDocument();
            pageDocumentPlatformIndex.Load(web3);
            
            var listLinks = pageDocumentPlatformIndex.DocumentNode.SelectNodes("//div[@class='rom-tr title']/a");

            string[] files = Directory.GetFiles(@"C:\Users\MSI\Google Drive\Projects\romscraper\prueba");
            var filesNames = new List<string>();

            foreach(string file in files){
                filesNames.Add(Path.GetFileName(file));
            }

            foreach(var node in listLinks){
                if(!filesNames.Contains(node.InnerText + ".zip")){
                    var html = @"https://www.freeroms.com/"+node.Attributes["href"].Value;
                    HtmlWeb htmlweb = new HtmlWeb();
                    var htmlRom = htmlweb.Load(html);

                    var romLink = htmlRom.DocumentNode.SelectSingleNode("//script[@language='javascript']");
                    string romScript = romLink.OuterHtml;
                    int lengthSubs = romScript.IndexOf(".zip") - romScript.IndexOf("http");
                    string urlDownload = romScript.Substring(romScript.IndexOf("http"),lengthSubs+4);

                    WebClient wClient = new WebClient();
                    wClient.DownloadFile(urlDownload, @"prueba/" + node.InnerText + ".zip");
                    count++;
                    WriteLine($"Downloaded {count} out of {listLinks.Count - filesNames.Count}");
                }
                // else{
                //     WriteLine($"{node.InnerText}: Game already downloaded");
                // }                    
            }
                // Write(node.InnerText + ": ");
                // Console.WriteLine(node.Attributes["href"].Value);
        }

            /******ROM/SCRIPT/URL EXTRACTOR******/
            // var web4 = @"source/index_rom_test.html";

            // var pageDocumentPlatformIndex = new HtmlDocument();
            // pageDocumentPlatformIndex.Load(web4);

            // var romLink = pageDocumentPlatformIndex.DocumentNode.SelectSingleNode("//script[@language='javascript']");
            // string romScript = romLink.OuterHtml;

            // int lengthSubs = romScript.IndexOf(".zip") - romScript.IndexOf("http");
            // string urlDownload = romScript.Substring(romScript.IndexOf("http"),lengthSubs+4);

            // /******DOWNLOADER******/

            // WebClient wClient = new WebClient();
            // wClient.DownloadFile(urlDownload, @"prueba/rom.zip");
        
    }
}