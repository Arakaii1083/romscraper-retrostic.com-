using HtmlAgilityPack;

namespace RomScraper
{
    //Class to get the different from URL we will use for the downloading service
    public class RomPlatformFetcher{
        //Variable to storage the Collection of nodes with the different platforms links
        public static HtmlNodeCollection listMenuLinks {get; set;}
        //Variable to storage the Collection of nodes with the different roms displayed on each page (1,2...n) for each platform
        public static HtmlNodeCollection listRoms {get; set;}

        //Function to get a collection of HTML nodes with the different platforms links
        public RomPlatformFetcher(string uri){
            listMenuLinks = UriContentFetcher.getContent(uri,
             "//td[@class='d-block d-sm-none text-center']/a[contains(@href,'/roms/')]",
              true);
        }

        //Function to get a collection of HTML nodes with the different pages indexes
        public static void romIndexesFetcher(HtmlNode platformUri, int page){
            listRoms = UriContentFetcher.getContent(@"https://www.retrostic.com" + platformUri.Attributes["href"].Value + "/page/" + page,
            "//td[@class='d-block d-sm-none text-center']/a[contains(@href,'/roms/')]",
            true);
            page++;
        }
        
        //Function to get a collection of HTML nodes with the different ROMs searching results
        public static void romSearchListFetcher(string uri){
            listRoms = UriContentFetcher.getContent(uri,
            "//td[@class='d-block d-sm-none text-center']/a[contains(@href,'/roms/')]",
            true);
        }
    }
}