using System;
using System.Collections.Generic;
using System.Text;
using ScrapySharp.Extensions;
using ScrapySharp.Network;
using HtmlAgilityPack;
using System.IO;
using System.Globalization;
using CsvHelper;
using System.Text.RegularExpressions;
using System.Linq;

namespace KitadeScrapper
{
    class Scrapper
    {
        static ScrapingBrowser _browser = new ScrapingBrowser();
        static string mainUrl = ("https://www.kita.de/");
        public Scrapper()
        {
            var _mainLinks = GetMainPageLinks(mainUrl, "ol.states_germany li a");

            var _citiesLink = new List<string>();
            var _schoolLinks = new List<string>();
            var _schoolDetail = new List<SchoolDetails>();
            foreach (var link in _mainLinks)
            {
                var citiesInOtherPages = GetPages(link, "ol.pagination_char li a");

                foreach (var currentPage in citiesInOtherPages)
                {
                    var citiesLink = GetMainPageLinks(currentPage, "ol.cities li a");
                    _citiesLink.AddRange(citiesLink);
                    Console.WriteLine(link.ToString());
                }
            }

            foreach (var link in _citiesLink)
            {
                var citiesInOtherPages = GetPages(link, "ol.pagination li a");

                foreach (var currentPage in citiesInOtherPages)
                {
                    var schoolLink = GetMainPageLinks(link, "h3 a ");
                    _schoolLinks.AddRange(schoolLink);
                    var schoolDet = SchoolDetails.GetPageDetails(schoolLink);
                    _schoolDetail.AddRange(schoolDet);
                    Console.WriteLine(link.ToString());
                }
            }
            
            var exportDataToCSV = new ExportDataToCSV(_schoolDetail);
        }
        
        static List<String> GetPages(string url, string queryExpression)
        {
            HashSet<string> homePageLinks = new HashSet<string>();
            homePageLinks.Add(url);
            var html = GetHtml(url);
            var links = html.CssSelect(queryExpression); 

            foreach (var link in links)
            {
                //filter out links 
                if (link.Attributes["href"] != null)
                    homePageLinks.Add(mainUrl + link.Attributes["href"].Value);
            }
          
            return homePageLinks.ToList();
        }
        static List<string> GetMainPageLinks(string url, string queryExpression)
        {
            var homePageLinks = new List<string>(); // to store all of the urls from 1st page, should try to get the url from other pages from this as well
            var html = GetHtml(url); // get html from each link
            var links = html.CssSelect(queryExpression); // anchor tag, ?write expressions in json and get directly from them

            foreach (var link in links)
            {
                //filter out links 
                if (link.Attributes["href"].Value.Contains("/kita"))
                    homePageLinks.Add(mainUrl + link.Attributes["href"].Value);
            }
            return homePageLinks;
        }
        // get html node from static webpage
        public static HtmlNode GetHtml(string url)
        {
            WebPage webPage = _browser.NavigateToPage(new Uri(url));
            return webPage.Html;
        }

    }
}