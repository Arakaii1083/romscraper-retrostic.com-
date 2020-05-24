using System;
using System.Net.Http;
using static System.Console;
using System.Threading.Tasks;
using System.Threading;
using HtmlAgilityPack;
using System.Net;
using System.IO;
using System.Collections.Generic;
using System.Diagnostics;

namespace RomScraper
{
    class Program
    {
        static void Main(string[] args)
        {
            Menu.startUp();

            try{
                RomPlatformFetcher newFetcher = new RomPlatformFetcher(@"https://www.retrostic.com/roms");
                WriteLine($"Connected to retrostic.com");
            }
            catch(Exception e){
                WriteLine($"Exception: {e.GetType()}");
            }
            
            int optMain = menuSelection();
            
            while(optMain>0 && optMain <4){
                switch(optMain){
                case 1:
                    Menu.menuHeader();
                    menuPlatforms();
                    optMain = menuSelection();
                    break;
                case 2:
                    Menu.menuHeader();
                    DirectoryFetcher.libraryFecther();
                    optMain = menuSelection();
                    break;
                case 3:
                    Menu.menuHeader();
                    searchGame();
                    optMain = menuSelection();
                    break;
                default:
                    break;
                }
            }

            int menuSelection(){
                WriteLine("\n(1)Add platform\t\t(2)See library\t\t(3)Search for a game\t\t(Other)Exit");
                WriteLine("Please, enter a number...");
                try{
                    return Convert.ToInt16(ReadLine());
                }
                catch{
                    WriteLine("Shutting down...");
                    return 4;
                }
            }

            void menuPlatforms(){
                var links = RomPlatformFetcher.listMenuLinks;   
                int countList = 1;
                int opt;

                WriteLine("Platforms:");
                foreach(var link in links){
                    WriteLine($"({countList}): {link.InnerText}");
                    countList++;
                }
                Write("Select a platform by its number: ");
                opt = Convert.ToInt32(ReadLine());
                WriteLine("\n");

                if(opt>=0 && opt<links.Count){
                    bool flag = true;
                    int page = 1;
                    RomDownloader.platform = links[opt-1].InnerText;
                    DirectoryFetcher.checkPlatformDirectory(links[opt-1].InnerText);

                    while(flag==true){
                        try{
                            RomPlatformFetcher.romIndexesFetcher(links[opt-1], page);

                            foreach(var node in RomPlatformFetcher.listRoms){
                                RomDownloader.romDownloader(node.Attributes["href"].Value, node);
                            }
                            page++;
                        }
                        catch{
                            WriteLine("Done!");
                            WriteLine($"\n\nTotal downloaded: {RomDownloader.totalRomsDownloaded}\n\n");
                            RomDownloader.totalRomsDownloaded = 0;
                            flag = false;
                        }
                    }
                }
                else{
                    WriteLine("Platform does not exist!\n");
                }
            }

            void searchGame(){
                int countList = 1;
                int opt;

                Write("Game to search: ");
                string search = ReadLine();
                search.Replace(" ", "+");
                WriteLine();

                try{
                    RomPlatformFetcher.romSearchListFetcher($@"https://www.retrostic.com/search?search_term_string={search}");

                    foreach(var node in RomPlatformFetcher.listRoms){
                        WriteLine($"{countList}: {node.InnerText}");
                        countList++;
                    }
                    Write("Select a game by its number: ");
                    opt = Convert.ToInt32(ReadLine());
                    WriteLine("\n");

                    if(opt>=0 && opt<RomPlatformFetcher.listRoms.Count+1){
                        DirectoryFetcher.checkRomsDirectory();
                        RomDownloader.romDownloader(RomPlatformFetcher.listRoms[opt-1].Attributes["href"].Value, RomPlatformFetcher.listRoms[opt-1]);
                    }
                }
                catch (Exception e){
                    WriteLine($"Searching error. Please, try again!");
                }
            }
        }
    }
}