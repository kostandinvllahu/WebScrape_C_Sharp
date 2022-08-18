using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebScrape
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("====WELCOME TO GOOGLE SERP SCRAPER====");
            var results = ScrapeSerp("accountants near me", 1);
            foreach (var result in results)
            {
                Console.WriteLine(result.Title);
                Console.WriteLine(result.Url);
            }
            Console.WriteLine("Press ANY key...");
            Console.ReadKey();
        }

        public static List<serpResult> ScrapeSerp(string query, int n_pages)
        {
            var serpResults = new List<serpResult>();
            for(var i = 1; i <= n_pages; i++)
            {
                var url = "https://www.google.com/search?q=" + query + "&num=50&start=" + ((i - 1) * 10).ToString();
                HtmlWeb web = new HtmlWeb();
                web.UserAgent = "user-agent=Mozilla/5.0 " +
                    "(Windows NT 10.0; Win64; x64)" +
                    "AppleWebKit/537.36 (KHTML, like Gecko)" +
                    "Chrome/74.0.3729.169 Safari/537.36";
                var htmlDoc = web.Load(url);
                HtmlNodeCollection nodes =
                    htmlDoc.DocumentNode.SelectNodes("//div[@class='rllt__details']"); //yuRUbf rllt__details
                foreach (var tag in nodes)
                {
                    // var result = new serpResult();
                    Console.WriteLine(tag.InnerText);
                    // result.Url = tag.Descendants("a").FirstOrDefault().Attributes["href"].Value; result.Title = 
                   // var name = htmlDoc.DocumentNode?.SelectNodes("//div[@class=\"VkpGBb\"]//span[@class=\"OSrXXb\"]")[0].InnerText;
                    //serpResults.Add(result);
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
