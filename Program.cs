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
            RomPlatformFetcher newFetcher = new RomPlatformFetcher(@"https://www.freeroms.com");
        }
    }
}