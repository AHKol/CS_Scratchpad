using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;
using HtmlAgilityPack;
using System.IO;

//Collects all avalable IMDB movie names and urls

namespace Scraper
{
    class Program
    {
        public struct Entery
        {
            public String URL;
            public String Name;
        }
        static void Main(string[] args)
        {
            //https://www.imdb.com/title/tt#######/?ref_=adv_li_tt format style

            //Todo: Continue from CSV
            for (int i = 1; i < 9999999; i++)
            {
                using(StreamWriter sw = File.AppendText(@"C:\Users\Adam\source\repos\Scraper\Scraper\IMDB_URL.csv"))
                {
                    try
                    {
                        Entery result = GetHtmlAsync(i).Result;
                        String line = i + "," + result.URL + "," + result.Name;
                        sw.WriteLine(line);

                        Console.WriteLine(i + ": " + result.Name);
                    }
                    catch {
                        Console.WriteLine(i + ": " + "Failed");
                    }
                }
            }

            Console.ReadLine();

        }

        private static async Task<Entery> GetHtmlAsync(int urlNum)
        {
            var url = "https://www.imdb.com/title/tt" + urlNum.ToString("0000000") + "/?ref_=adv_li_tt";
            var httpClient = new HttpClient();

            String html;

            //todo: Catch 404 and throw fail
            html = await httpClient.GetStringAsync(url);
            

            var doc = new HtmlDocument();
            doc.LoadHtml(html);

            //Get and format name in html
            var name = doc.DocumentNode.SelectSingleNode("//div[contains(@class,'title_wrapper')]/h1/text()");
            var nameString = name.InnerText;
            nameString = nameString.Remove(nameString.Length - 6, 6);

            //Prepare data for output
            Entery output;
            output.URL = url;
            output.Name = nameString;

            return output;
        }
    }   
}

