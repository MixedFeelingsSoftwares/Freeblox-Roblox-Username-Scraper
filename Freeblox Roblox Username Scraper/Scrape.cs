using HtmlAgilityPack;
using Konsole;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading;
using System.Threading.Tasks;

namespace Freeblox_Roblox_Username_Scraper
{
    public class Scrape
    {
        #region Public Fields

        public static List<User> users = new List<User>();

        #endregion Public Fields

        #region Public Properties

        public static int unavailableUsers { get; set; } = 0;

        #endregion Public Properties

        #region Public Methods

        public static int cProg = -1;

        public static int minProg = -1;

        public static int progID = -1;

        private static List<BackgroundWorker> Workers { get; set; } = new List<BackgroundWorker>();

        public static async void createList(List<User> usr)
        {
            using (System.IO.StreamWriter sw = System.IO.File.CreateText(Environment.CurrentDirectory + $@"\Scraped_Users_{DateTime.Now.ToShortDateString().Replace('/', '-')}.txt"))
            {
                sw.AutoFlush = true;
                for (int i = 0; i < usr.Count; i++)
                {
                    await sw.WriteLineAsync(usr[i].Username);
                }
            }
        }

        public static void RenderUsers()
        {
            Console.Clear();

            // let's open a window with a box around it by using Window.Open
            var credits = Window.Open(1, 4, 40, 10, "Credits");
            credits.WriteLine("");
            credits.WriteLine(" Made by CemsSlave - Nulled.To");

            var names = Window.Open(50, 4, 40, 10, "Stats");
            names.WriteLine("");
            names.WriteLine($" Total Users Scraped: {Scrape.users.Count}");

            Console.SetCursorPosition(0, 0);

            ConsoleKeyInfo key = Console.ReadKey(true);
            switch (key.Key)
            {
                case ConsoleKey.D1:
                    createList(users);
                    Console.Clear();
                    Console.WriteLine("## Successfully saved to file!");
                    Console.WriteLine("## Path: " + Environment.CurrentDirectory + $@"\Scraped_Users_{DateTime.Now.ToShortDateString().Replace('/', '-')}.txt");
                    break;

                case ConsoleKey.D2:

                    break;
            }
        }

        public static async Task<User> Start(CountdownEvent finishedSignal)
        {
            User usr = null;
            if (cProg > minProg)
            {
                int p = cProg;
                cProg--;
                string url = $"https://api.roblox.com/users/{p}";
                HtmlDocument doc = await new HtmlWeb().LoadFromWebAsync(url);
                usr = JsonConvert.DeserializeObject<User>(doc.DocumentNode.InnerText);
                if (usr != null)
                {
                    if (!users.Exists(x => x.Username == usr.Username))
                    {
                        users.Add(usr);
                    }
                }
                else
                {
                    usr = null;
                    unavailableUsers++;
                }

                if (cProg <= minProg)
                {
                    usr = null;
                }
            }
            finishedSignal.Signal();
            Program.UpdateTitle();
            return usr;
        }

        public static void StartScrape(int From, int To)
        {
            using (var finishedSignal = new CountdownEvent(1))
            {
                progID = To;
                minProg = From;
                cProg = To;
                for (int i = 0; i < progID && cProg > minProg; i++)
                {
                    if (cProg > minProg)
                    {
                        finishedSignal.AddCount();
                        Task.Factory.StartNew(async () => await Start(finishedSignal));

                        //BackgroundWorker worker = new BackgroundWorker();
                        //worker.DoWork += async (s, g) =>
                        //{
                        //    await Start();
                        //};
                        //worker.RunWorkerCompleted += (s, g) =>
                        //{
                        //    Console.WriteLine("completed" + Workers.Count);
                        //};
                        //Workers.Add(worker);
                        //worker
                        //worker.RunWorkerAsync();
                    }
                }

                finishedSignal.Signal();
                Console.WriteLine("Waiting for Scraper to complete.");
                finishedSignal.Wait();
            }
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("Finished waiting for all tasks to complete.");
            Console.ForegroundColor = ConsoleColor.White;
            RenderUsers();
        }

        #endregion Public Methods

        #region Public Classes

        public class Progress
        {
        }

        public class User
        {
            #region Public Properties

            public bool AvatarFinal { get; set; }

            public object AvatarUri { get; set; }

            public int Id { get; set; }

            public bool IsOnline { get; set; }

            public string Username { get; set; }

            #endregion Public Properties
        }

        #endregion Public Classes
    }
}