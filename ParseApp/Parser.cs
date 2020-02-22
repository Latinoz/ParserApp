using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml.Linq;

namespace ParseApp
{
    public class Parser
    {
        string urlHttp = "http://****/cigardb/";

        Dictionary<string,string> brands = new Dictionary<string, string>();

        List<Parameters> param = new List<Parameters>();

        public void DoWork()
        {
            Console.WriteLine("Start working!");

            //Первый шаг: Получения url страниц с 1-ой по 69-ю
            DoStep_First();
            
            Console.WriteLine("Done!");
        }
       
        
        void DoStep_First()
        {
            string url = "******";
            int i = 1;
            string urlStep;

            //while(i <= 2)
            while (i <= 69)            
            {
                string iStr;
                iStr = i.ToString();
                urlStep = url + iStr;
                                
                HtmlDocument tempDoc = ProxyUse(urlStep);     /* Работа через прокси, или без */

                //Второй шаг: Получение названий брендов
                DoStep_Second(tempDoc);
                Console.WriteLine("Page: " + i + " complete!");

                i++;
                
            }                
            
        }

        void DoStep_Second(HtmlDocument _tempDoc)
        {
            string _brandName;

            string _subUrl;

            var doc = _tempDoc;

            var table_node = doc.DocumentNode.SelectNodes("//table[last()][contains(@class, 'bbstable')]//a[@href]");

            foreach (var t in table_node)
            {
                brands.Add(t.InnerText, t.OuterHtml);                
            }

            foreach(var u in brands)
            {
                _brandName = u.Key;
                _subUrl = u.Value;
                string trimURL = FindHrefs(_subUrl);

                //Третий шаг: Перебор внутри бренда
                DoStep_Third(urlHttp + trimURL);

            }

            //Очистка словаря brands после каждой страницы
            brands.Clear();
      

        }

        void DoStep_Third(string _url)
        {
            HtmlDocument tempDoc = ProxyUse(_url);     /* Работа через прокси, или без */

            var doc = tempDoc;             

            var a8 = doc.DocumentNode.SelectNodes("//td[last()-8][contains(@class, 'messagecellbody')]");

            var a7 = doc.DocumentNode.SelectNodes("//td[last()-7][contains(@class, 'messagecellbody')]");

            var a6 = doc.DocumentNode.SelectNodes("//td[last()-6][contains(@class, 'messagecellbody')]");

            var a5 = doc.DocumentNode.SelectNodes("//td[last()-5][contains(@class, 'messagecellbody')]");

            var a4 = doc.DocumentNode.SelectNodes("//td[last()-4][contains(@class, 'messagecellbody')]");

            var a3 = doc.DocumentNode.SelectNodes("//td[last()-3][contains(@class, 'messagecellbody')]");

            var a2 = doc.DocumentNode.SelectNodes("//td[last()-2][contains(@class, 'messagecellbody')]");

            var a1 = doc.DocumentNode.SelectNodes("//td[last()-1][contains(@class, 'messagecellbody')]");

            var a0 = doc.DocumentNode.SelectNodes("//td[last()][contains(@class, 'messagecellbody')]");

            
            param.Add(new Parameters { Cigar = a8.Select(x => x.InnerText).ToArray(),
                                       Length = a7.Select(x => x.InnerText).ToArray(),
                                       Ring = a6.Select(x => x.InnerText).ToArray(),
                                       Country = a5.Select(x => x.InnerText).ToArray(),
                                       Filler = a4.Select(x => x.InnerText).ToArray(),
                                       Wrapper = a3.Select(x => x.InnerText).ToArray(),
                                       Color = a2.Select(x => x.InnerText).ToArray(),
                                       Strength = a1.Select(x => x.InnerText).ToArray()                                       
            });

            //....//
            DoStep_Fourth(param);
        }

        void DoStep_Fourth(List<Parameters> _item)
        {
            var open = new MangoDBConnect();
            //open.Connect();
            open.Insert(_item);
        }

        HtmlDocument WithOutProxy(string _url)
        {
            string url = _url;

            // From Web            
            var _web = new HtmlWeb();
            var _doc = _web.Load(url);

            return _doc;
        }

        HtmlDocument ProxyUse(string _url)
        {
            string url = _url;

            WebProxy proxy = new WebProxy("srv-squid.gnivc.msk", 8080);     /* Настройки прокси сервера */
            proxy.Credentials = CredentialCache.DefaultCredentials;

            WebClient client = new WebClient();
            client.Proxy = proxy;

            string baseHtml = "";

            byte[] pageContent = client.DownloadData(url);

            UTF8Encoding utf = new UTF8Encoding();
            baseHtml = utf.GetString(pageContent);

            HtmlDocument pageHtml = new HtmlDocument();
            pageHtml.LoadHtml(baseHtml);

            return pageHtml;
        }

        private static string FindHrefs(string input)
        {
            Match m;
            string HRefPattern = @"href\s*=\s*(?:[""'](?<1>[^""']*)[""']|(?<1>\S+))";

            string result = null;

            try
            {
                m = Regex.Match(input, HRefPattern,
                                RegexOptions.IgnoreCase | RegexOptions.Compiled,
                                TimeSpan.FromSeconds(1));

                return result = m.Groups[1].ToString();
                
            }
            catch (RegexMatchTimeoutException)
            {
                Console.WriteLine("The matching operation timed out.");
                return null;
            }            

        }
    }
}
