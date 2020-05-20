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
    class DirectoryFetcher
    {
        private static string[] directories;
        private static string[] files;
        private static List<string> filesName = new List<string>();
        public static string currentDirectory = Directory.GetCurrentDirectory();
        private string platform {set; get;}

        public DirectoryFetcher(){}
        public DirectoryFetcher(string platform){
            this.platform = platform;
        }

        public static void checkPlatformDirectory(string dir){
            string path = DirectoryFetcher.currentDirectory + "/roms/" + dir;
            if(!Directory.Exists(path)){
                Directory.CreateDirectory($@"{path}");
            }
        }

        public static void libraryFecther(){
            Console.WriteLine("Games Library:\n");
            if(Directory.Exists($@"{currentDirectory}/roms")){
                directories = Directory.GetDirectories($@"{currentDirectory}/roms");
                
                if(directories.Length>0){
                    foreach(string directory in directories){
                        string folderName = new DirectoryInfo(directory).Name;
                        files = Directory.GetFiles(directory);

                        Console.WriteLine($"Platform: {folderName} // Games: {files.Length}");                                       
                    }
                }
                else{
                    Console.WriteLine("Empty Library! Get yourself some roms!");
                }
            }
            else{
                Directory.CreateDirectory($@"{currentDirectory}/roms");
                Console.WriteLine("Empty Library! Get yourself some roms!");
            }
        }
    }
}