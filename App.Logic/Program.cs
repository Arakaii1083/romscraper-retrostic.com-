using System;
using static System.Console;

namespace RomScraper
{
    class Program{
        static void Main(string[] args){
            //Clearing the screen and loading the header
            Menu.startUp();

            //Instance of an object type RomPlatformFetcher to load the site content
            try{
                RomPlatformFetcher newFetcher = new RomPlatformFetcher(@"https://www.retrostic.com/roms");
                WriteLine($"Connected to retrostic.com");
            }
            catch(Exception e){
                WriteLine($"Exception: {e.GetType()}");
            }
            
            /*****************************
            **********MAIN MENU***********
            *****************************/
            int optMain = menuSelection();
            
            while(optMain>0 && optMain <4){
                switch(optMain){
                case 1:
                    Menu.startUp();
                    menuPlatforms();
                    optMain = menuSelection();
                    break;
                case 2:
                    Menu.startUp();
                    DirectoryFetcher.libraryFecther();
                    optMain = menuSelection();
                    break;
                case 3:
                    Menu.startUp();
                    searchGame();
                    optMain = menuSelection();
                    break;
                default:
                    break;
                }
            }
            //Method to return menu selection. In case of a string, catch exception and return 4 to close the app
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
            /*****************************
            ********PLATFORM MENU*********
            *****************************/
            void menuPlatforms(){
                //Previous object instance of RomPlatformFetcher gets the different platform links
                var links = RomPlatformFetcher.listMenuLinks;
                
                int countList = 1;
                int opt;

                //Showing the list of platforms
                WriteLine("Platforms:");
                foreach(var link in links){
                    WriteLine($"({countList}): {link.InnerText}");
                    countList++;
                }
                //Selecting platform to download
                Write("Select a platform by its number: ");
                try{
                    opt = Convert.ToInt32(ReadLine());
                }
                catch {return;}
                WriteLine("\n");

                if(opt>=0 && opt<links.Count+1){
                    //Variable for control purposes
                    bool flag = true;
                    //Variable to go through whole platform pages result
                    int page = 1;
                    //Get the platform name for directory purposes and calling a method to check if such 
                    //directory already exists in our library
                    RomDownloader.platform = links[opt-1].InnerText;
                    DirectoryFetcher.checkPlatformDirectory(links[opt-1].InnerText);
                    
                    //Loop to fetch all the roms through the different pages (1,2,3...) till fails and catch
                    //the exception, then return false and end the loop
                    while(flag==true){
                        try{
                            //Call a method to catch of the rom links for the current page
                            RomPlatformFetcher.romIndexesFetcher(links[opt-1], page);
                            //Loop to go through all the links collected by the class RomPlatformFetcher
                            foreach(var node in RomPlatformFetcher.listRoms){
                                //Call a method to dowload the rom. Parameters: uri and currect HtmlNode
                                RomDownloader.romDownloader(node);
                            }
                            //While fetching content...
                            page++;
                        }
                        //After the last page (1,2,3...N), done and showing the total amount of downloads
                        catch{
                            WriteLine("Done!");
                            WriteLine($"\n\nTotal downloaded: {RomDownloader.totalRomsDownloaded}\n\n");
                            RomDownloader.totalRomsDownloaded = 0;
                            flag = false;
                        }
                    }
                }
                else{
                    //In case of bad selection in the menu
                    WriteLine("Platform does not exist!\n");
                }
            }
            /*****************************
            ********SEARCHING MENU********
            *****************************/
            void searchGame(){
                //Variables for selection purposes
                int countList = 1;
                int opt;

                Write("Game to search: ");
                string search = ReadLine();
                //To fit the URL header, replace the space with +
                search.Replace(" ", "+");
                WriteLine();

                //Same concept like the Platform menu and selection
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
                        RomDownloader.romDownloader(RomPlatformFetcher.listRoms[opt-1]);
                    }
                }
                catch{
                    WriteLine($"Searching error. Please, try again!");
                }
            }
        }
    }
}