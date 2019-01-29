using Konsole;
using System;
using System.Threading;

namespace Freeblox_Roblox_Username_Scraper
{
    internal class Program
    {
        #region Private Methods

        [STAThread]
        private static void Main(string[] args)
        {
            Console.CursorVisible = false;
            DrawLogo();
            {
                DrawOptionsMainMenu();
                {
                    UpdateTitle();
                }
            }
            while (true)
            {
                ConsoleKeyInfo keyInfo = Console.ReadKey(true);
                switch (keyInfo.Key)
                {
                    case ConsoleKey.D1:
                        System.Windows.Forms.Clipboard.SetText("1PiXhZSkrPLUdnme9EGKV4jbXn7oCTJkLG");
                        Console.Clear();
                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.WriteLine("Bitcoin Address Copied to Clipboard!");
                        Console.ForegroundColor = ConsoleColor.White;
                        Thread.Sleep(3000);
                        Console.Clear();
                        break;

                    case ConsoleKey.D2:
                        System.Windows.Forms.Clipboard.SetText("0x1C511201496385F09c35F089411fD0EDfa6C7dd0");
                        Console.Clear();
                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.WriteLine("Ether Address Copied to Clipboard!");
                        Console.ForegroundColor = ConsoleColor.White;
                        Thread.Sleep(3000);
                        Console.Clear();
                        break;

                    case ConsoleKey.D3:
                        Console.Clear();
                        Scrape.StartScrape(300, 1000);
                        break;

                    case ConsoleKey.Escape:
                        Environment.Exit(-1);
                        break;
                }
            }
        }

        #endregion Private Methods

        #region Public Methods

        public static void DrawInterface()
        {
            Console.Clear();
        }

        public static void DrawLogo()
        {
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine($"Roblox Username Scraper made by CemsSlave\n");
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"Hopefully you can restrain from posting my tool on other websites like people did on 'ComboXD'.");
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"This tool is a freeware. But please, it will always bring my mood up to create more and better scrapers.");
            Console.WriteLine("\n\n");
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine($"## Press 1 for BTC: 1PiXhZSkrPLUdnme9EGKV4jbXn7oCTJkLG");
            Console.ForegroundColor = ConsoleColor.DarkYellow;
            Console.WriteLine($"## Press 2 for ETH: 0x1C511201496385F09c35F089411fD0EDfa6C7dd0");
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"## Press 3 To Start Scraping.");
            Console.ForegroundColor = ConsoleColor.White;
        }

        public static void DrawOptionsMainMenu()
        {
            Console.WriteLine("\n");

            Console.WriteLine("\nAmount of threads are scaled dynamically.", System.Drawing.Color.White);
        }

        public static void UpdateProgress(ProgressBar pb, int progress)
        {
            pb.Refresh(progress, $"Scraping.. {progress}% / {100}%");
        }

        public static void UpdateTitle()
        {
            Console.Title = $"Freeblox Username Scraper | Scraped {Scrape.users.Count} | Unavailable Profiles {Scrape.unavailableUsers}";
        }

        #endregion Public Methods
    }
}