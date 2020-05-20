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
    public static class MenuStartUp
    {
        public static void startUp(){
            Console.Clear();
            menuHeader();
            // menuSelection();
        }

        private static void menuHeader(){
            SetWindowSize(91, 30);

            for(int i=1;i<91;i++){
                SetCursorPosition(i, 1);
                Write("═");
                SetCursorPosition(i, 7);
                Write("═");
            }
            for(int i=2;i<7;i++){
                SetCursorPosition(1, i);
                Write("║");
                SetCursorPosition(90, i);
                Write("║");
            }
            SetCursorPosition(1,1);
            Write("╔");
            SetCursorPosition(90,1);
            Write("╗");
            SetCursorPosition(1,7);
            Write("╚");
            SetCursorPosition(90,7);
            Write("╝");

            SetCursorPosition(3,3);
            DateTime fechaActual = DateTime.Now;
            Write($"Fecha: {fechaActual.ToString("dd/MM/yyy")}");

            SetCursorPosition(75, 3);
            Write($"Hora: {fechaActual.ToString("HH:mm:ss")}");

            SetCursorPosition(3,5);
            Write("Developer: Antonio Montes");
            SetCursorPosition(3,6);
            Write("Proyect: RomScraper");

            SetCursorPosition(0,9);
        }
    }
}