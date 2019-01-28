using HtmlAgilityPack;
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
                    await sw.WriteLineAsync(usr[i].username);
                }
            }
        }

        public static void RenderUsers()
        {
            Console.Clear();
            Console.WriteLine($"######################## Stats ########################");
            Console.WriteLine($"## Total Users Scraped: {users.Count}                  ");
            Console.WriteLine($"## Total Profiles Unavailable: {unavailableUsers}      ");
            Console.WriteLine($"#######################################################");
            Console.WriteLine($"## Press 1 To save as list");
            Console.WriteLine($"## Press 2 To Exit");
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
                string url = $"https://www.roblox.com/users/{p}/profile";
                HtmlDocument doc = await new HtmlWeb().LoadFromWebAsync(url);
                HtmlNode nameNode = doc.DocumentNode.SelectSingleNode("//*[@id='wrap']/div[4]/div[2]/div[2]/div/div[1]/div/div[2]/div[2]/div[1]/h2");
                if (nameNode != null)
                {
                    if (!users.Exists(x => x.username == nameNode.InnerText))
                    {
                        usr = new User()
                        {
                            username = nameNode.InnerText,
                        };
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
            Program.UpdateProgress();
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

            public string username { get; set; }

            #endregion Public Properties
        }

        #endregion Public Classes
    }
}