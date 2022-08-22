using HtmlAgilityPack;
using Microsoft.Office.Interop.Excel;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("==========================GOOGLE SEARCH SCRAPER V1.0==========================");
            int totalpages = 0;
            string search = "";
            Console.WriteLine("What you are looking for?");
            search = Console.ReadLine();
            Console.WriteLine("Enter number of the page to scrape: ");
            totalpages = Convert.ToInt32(Console.ReadLine());



            var results = ScrapeSerp(search, totalpages);

            foreach (var result in results)
            {

            }
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

                List<string> lines = new List<string>();
                string dot = ".";
                int count = 0;
                int count2 = 0;
                int count3 = 0;
                string path = @"C:\Users\vllah\Desktop\Data.xlsx";
                Microsoft.Office.Interop.Excel.Application excel = new Microsoft.Office.Interop.Excel.Application();
                Workbook wb;
                Worksheet ws;

                object misValue = System.Reflection.Missing.Value;
                var excelFile = new Application();
                wb = excel.Workbooks.Open(path);
                wb.Close();
                wb = excel.Workbooks.Open(path);
                ws = wb.Worksheets[1];
                int last = ws.Cells.SpecialCells(XlCellType.xlCellTypeLastCell, Type.Missing).Row;

                Range rowrange = ws.get_Range("A1:A" + last);
                foreach (Range element in rowrange.Cells)
                {
                    if (element.Value2 != null)
                    {
                        count += 1;
                        count2 += 1;
                        count3 += 1;
                    }
                    else
                    {
                        break;
                    }
                }



                Workbook workbook = excelFile.Workbooks.Open(path);
                Worksheet worksheet = workbook.Worksheets[1];
                Range range = (Range)worksheet.Range[worksheet.Cells[1, 1], worksheet.Cells[1, 3]];
                var xlWorkbook = new ExcelPackage(new FileInfo(@"C:\Users\vllah\Desktop\Data.xlsx"));
                ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

                ExcelWorksheet sheet = new ExcelPackage().Workbook.Worksheets.Add("Sheet1");

                HtmlNodeCollection nodes = htmlDoc.DocumentNode.SelectNodes("//div[@class='rllt__details']"); //yuRUbf rllt__details
                HtmlNodeCollection nodes2 = htmlDoc.DocumentNode.SelectNodes("//span[@class='OSrXXb']");
                foreach (var tag in nodes)
                {

                    Range cellrange;
                    count += 1;
                    cellrange = ws.Range["B" + count + ":B" + count];
                    dot += dot;
                    Regex re = new Regex(@"\s*\+\d+ \d{3}-\d{3}-\d{4}"); //\w{8}, \w{2}, \w[a-zA]{5}
                    var matches = re.Matches(tag.InnerText);
                    foreach (Match m in matches)
                    {
                        string[] phone = new[] { m.Value };
                        cellrange.set_Value(XlRangeValueDataType.xlRangeValueDefault, phone);
                    }
                    if (matches.Count == 0)
                    {
                        cellrange.set_Value(XlRangeValueDataType.xlRangeValueDefault, "Sorry no phone number found!");
                    }

                    Console.WriteLine(dot);
                }
                lines.Clear();

                foreach (var tag2 in nodes2)
                {
                    Range cellrange3;

                    
                    count3 += 1; 
                    cellrange3 = ws.Range["A" + count3 + ":A" + count3];

                    string[] name = new[] { tag2.InnerText };
                    cellrange3.set_Value(XlRangeValueDataType.xlRangeValueDefault, name);
                }

                lines.Clear();
                foreach (var tag in nodes)
                {
                    Regex re = new Regex(@"\w{2}, \w[a-zA]{5}");
                    var matches = re.Matches(tag.InnerText);
                    Range cellrange2;
                    //cellrange2 = ws.Range["C:C"];
                    count2 += 1;
                    cellrange2 = ws.Range["C" + count2 + ":C" + count2];
                    foreach (Match m in matches)
                    {
                        string[] address = new[] { m.Value };
                        cellrange2.set_Value(XlRangeValueDataType.xlRangeValueDefault, address);
                    }
                    if (matches.Count == 0)
                    {
                        cellrange2.set_Value(XlRangeValueDataType.xlRangeValueDefault, "Sorry no address found!");
                    }
                }
                try
                {
                    //wb.SaveAs("C:\\Users\\vllah\\Desktop\\Data.xlsx");
                    wb.Save();
                    wb.Close();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    Console.WriteLine("To fix this problem open task manager, find Excel and make it  'END TASK' then try again to run the program.");
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
