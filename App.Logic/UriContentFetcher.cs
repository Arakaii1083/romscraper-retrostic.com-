using HtmlAgilityPack;

namespace RomScraper
{
    //Class to extract the different content (node or collection of nodes) we will use for the downloading service
    public static class UriContentFetcher{
        //Dynamic function to select node or collection of nodes depending of the boolean parameter "mode".
        public static dynamic getContent(string uri, string xpath, bool mode){
            HtmlWeb htmlweb = new HtmlWeb();
            var htmlDocument = htmlweb.Load(uri);

            if(mode){
                return htmlDocument.DocumentNode.SelectNodes(xpath);
            }
            else{
                return htmlDocument.DocumentNode.SelectSingleNode(xpath);
            }
        }
    }
}