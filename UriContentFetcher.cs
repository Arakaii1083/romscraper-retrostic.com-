using HtmlAgilityPack;

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