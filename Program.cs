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
    class Program
    {
        static void Main(string[] args)
        {
            Menu.startUp();

            try{
                RomPlatformFetcher newFetcher = new RomPlatformFetcher(@"https://www.freeroms.com");
            }
            catch(Exception e){
                WriteLine($"Exception: {e.GetType()}");
            }
            
            int optMain = menuSelection();
            
            while(optMain>0 && optMain <4){
                switch(optMain){
                case 1:
                case 2:
                    Menu.menuHeader();
                    menuPlatforms();
                    optMain = menuSelection();
                    break;
                case 3:
                    Menu.menuHeader();
                    DirectoryFetcher.libraryFecther();
                    optMain = menuSelection();
                    break;
                default:
                    break;
                }
            }

            int menuSelection(){
                WriteLine("\n(1)Add game\t\t(2)Add platform\t\t(3)See library\t\t(Other)Exit");
                WriteLine("Please, enter a number...");
                try{
                    return Convert.ToInt16(ReadLine());
                }
                catch{
                    WriteLine("Shutting down...");
                    return 5;
                }
            }

            void menuPlatforms(){
                var links = RomPlatformFetcher.listMenuLinks;   
                int inc = 0;
                int opt;

                WriteLine("Platforms:");
                foreach(var link in links){
                    if(link.InnerText!="Links" && link.InnerText!="Flash Games"){
                        WriteLine($"({inc}): {link.InnerText}");
                        inc++;
                    }
                }
                WriteLine($"({inc}): Exit selection");

                opt = Convert.ToInt16(ReadLine());

                if(opt>=0 && opt <links.Count){
                    RomDownloader.platform = links[opt].InnerText;
                    DirectoryFetcher.checkPlatformDirectory(links[opt].InnerText);
                    RomPlatformFetcher.romIndexesFetcher(links[opt]);
                    
                    foreach(var node in RomPlatformFetcher.listIndexes){
                        WriteLine(node.InnerText + ": ");
                        RomDownloader.romDownloader(node.Attributes["href"].Value);
                    }
                    WriteLine($"Total downloaded: {RomDownloader.totalRomsDownloaded}");
                    RomDownloader.totalRomsDownloaded = 0;
                    }
                else{
                    WriteLine("ERROR!");
                }
            }
        }
    }
}