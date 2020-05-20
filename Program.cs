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
            MenuStartUp.startUp();
            try{
                RomPlatformFetcher newFetcher = new RomPlatformFetcher(@"https://www.freeroms.com");
            }
            catch(Exception e){
                WriteLine($"Exception: {e.GetType()}");
            }
            

            int opt = MenuStartUp.menuSelection();
            switch(opt){
                case 1:
                case 2:
                    MenuStartUp.menuHeader();
                    MenuStartUp.menuPlatforms();
                    opt = MenuStartUp.menuSelection();
                    break;
                case 3:
                    MenuStartUp.menuHeader();
                    DirectoryFetcher.libraryFecther();
                    opt = MenuStartUp.menuSelection();
                    break;
                default:
                    break;
            }
        }
    }
}