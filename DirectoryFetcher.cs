using System;
using static System.Console;
using System.IO;

namespace RomScraper
{
    static class DirectoryFetcher
    {
        //Array containing all the platform directories
        private static string[] directories;
        //Array containing all the files for counting
        private static string[] files;
        //Static variable to get the path of the current directory of the running app
        public static string currentDirectory = Directory.GetCurrentDirectory();

        //Function to check the existance of ROMs directory. In case of directory not found, this one would be created
        public static void checkRomsDirectory(){
            if(!Directory.Exists($@"{currentDirectory}/roms")){
                try{
                    WriteLine($"Creating root directory (roms): {currentDirectory}/roms");
                    Directory.CreateDirectory($@"{currentDirectory}/roms");
                }
                catch{
                    WriteLine("Folder creation failed.");
                }
            }
        }

        //Function to check the existance of the specific platoform we are downloading. In case of directory not found, this one would be created
        public static void checkPlatformDirectory(string dir){
            string path = DirectoryFetcher.currentDirectory + "/roms/" + dir;
            if(!Directory.Exists(path)){
                try{
                    WriteLine($"Creating directory: {path}");
                    Directory.CreateDirectory($@"{path}");
                }
                catch{
                    WriteLine("Folder creation failed.");
                }
            }
        }

        //Function to show the current library with all the platforms and number of games
        public static void libraryFecther(){
            WriteLine("Games Library:\n");
            if(Directory.Exists($@"{currentDirectory}/roms")){
                directories = Directory.GetDirectories($@"{currentDirectory}/roms");
                
                if(directories.Length>0){
                    foreach(string directory in directories){
                        string folderName = new DirectoryInfo(directory).Name;
                        files = Directory.GetFiles(directory);
                        if(files.Length>0){
                            WriteLine($"Platform: {folderName} // Games: {files.Length}");
                        }                                       
                    }
                }
                else{
                    WriteLine("Empty Library! Get yourself some roms!");
                }
            }
            else{
                Directory.CreateDirectory($@"{currentDirectory}/roms");
                WriteLine("Empty Library! Get yourself some roms!");
            }
        }
    }
}