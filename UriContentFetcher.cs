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
    public static class UriContentFetcher
    {
        public static dynamic getContent(string uri, string xpath, bool mode){
            HtmlWeb htmlweb = new HtmlWeb();
            var htmlDocument = htmlweb.Load(uri);

            if (mode){
                return htmlDocument.DocumentNode.SelectNodes(xpath);
            }
            else {
                return htmlDocument.DocumentNode.SelectSingleNode(xpath);
            }
        }
    }
}