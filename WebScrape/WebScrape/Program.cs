using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace WebScrape
{
    class Program
    {
        static void Main(string[] args)
        {

            Console.WriteLine("====WELCOME TO GOOGLE SERP SCRAPER====");
            int totalpages = 0;
            string search = "";
            Console.WriteLine("What you are looking for?");
            search = Console.ReadLine();
            Console.WriteLine("Enter number of pages to scrape: ");
            totalpages = Convert.ToInt32(Console.ReadLine());
            
            
         //   for (int i = 1; i < totalpages; i++)
           // {
                var results = ScrapeSerp(search, totalpages);

                foreach (var result in results)
                {
                    //Console.WriteLine(".....");
                    // Console.WriteLine(result.Url);
                }
            //}
            Console.WriteLine("Scrapping is finished successfully!");
            Console.WriteLine("Press ANY key to close the terminal...");
            Console.ReadKey();
        }


        public static List<serpResult> ScrapeSerp(string query, int n_pages)
        {
            var serpResults = new List<serpResult>();
            for (var i = 1; i <= n_pages; i++)
            {
                var url = "https://www.google.com/search?tbs=lf:1,lf_ui:14&tbm=lcl&sxsrf=ALiCzsa3y5padl5QdKju9qK5nUc_UgOU9g:1660837363043&q=" + query + "&num=50&start=" + ((i - 1) * 10).ToString();
                HtmlWeb web = new HtmlWeb();
                web.UserAgent = "user-agent=Mozilla/5.0 " +
                    "(Windows NT 10.0; Win64; x64)" +
                    "AppleWebKit/537.36 (KHTML, like Gecko)" +
                    "Chrome/74.0.3729.169 Safari/537.36";
                var htmlDoc = web.Load(url);
                string filepath = @"C:\Users\vllah\Desktop\Phone.txt";
                string filepath2 = @"C:\Users\vllah\Desktop\Name.txt";
                string filepath3 = @"C:\Users\vllah\Desktop\Address.txt";
                List<string> lines = new List<string>();
                string dot = ".";
                int count = 0;
                int count2 = 0;
                HtmlNodeCollection nodes = htmlDoc.DocumentNode.SelectNodes("//div[@class='rllt__details']"); //yuRUbf rllt__details
                HtmlNodeCollection nodes2 = htmlDoc.DocumentNode.SelectNodes("//span[@class='OSrXXb']");
                foreach (var tag in nodes)
                {

                    dot += dot;
                    count += 1;
                    Regex re = new Regex(@"\s*\+\d+ \d{3}-\d{3}-\d{4}"); //\w{8}, \w{2}, \w[a-zA]{5}
                    var matches = re.Matches(tag.InnerText);
                    foreach (Match m in matches)
                    {
                        lines.Add(count + ") " + m.Value + "\n");
                        File.WriteAllLines(filepath, lines);
                    }
                    if(matches.Count == 0)
                    {
                        lines.Add(count + ") " + "Sorry no phone number found!" + "\n");
                        File.WriteAllLines(filepath, lines);
                    }

                    //Regex re2 = new Regex(@"\w{8}, \w{2}, \w[a-zA]{5}");
                    //lines.Add(count + ") " + tag.InnerText + "\n");
                    //File.WriteAllLines(filepath3, lines);
                    Console.WriteLine(dot);
                }

                lines.Clear();
                //  Console.WriteLine(dot);
                foreach (var tag2 in nodes2)
                {
                    count2 += 1;
                    lines.Add(count2 + ") " + tag2.InnerText + "\n");
                    File.WriteAllLines(filepath2, lines);
                }
            }

            return serpResults;
        }

        public class serpResult
        {
            public string Url { get; set; }
            public string Title { get; set; }
            public string PhoneNum { get; set; }
            public string Address { get; set; }
            public string Rate { get; set; }
            public string OpeningHours { get; set; }
        }
    }
}
