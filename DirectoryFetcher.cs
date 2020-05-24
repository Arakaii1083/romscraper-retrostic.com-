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
    static class DirectoryFetcher
    {
        private static string[] directories;
        private static string[] files;
        public static string currentDirectory = Directory.GetCurrentDirectory();

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

        public static void libraryFecther(){
            Console.WriteLine("Games Library:\n");
            if(Directory.Exists($@"{currentDirectory}/roms")){
                directories = Directory.GetDirectories($@"{currentDirectory}/roms");
                
                if(directories.Length>0){
                    foreach(string directory in directories){
                        string folderName = new DirectoryInfo(directory).Name;
                        files = Directory.GetFiles(directory);
                        if(files.Length>0){
                            Console.WriteLine($"Platform: {folderName} // Games: {files.Length}");
                        }                                       
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