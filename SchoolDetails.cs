﻿using System;
using System.Collections.Generic;
using System.Text;
using ScrapySharp.Extensions;
using ScrapySharp.Network;
using HtmlAgilityPack;
using System.IO;
using System.Globalization;
using CsvHelper;
using System.Text.RegularExpressions;

namespace KitadeScrapper
{
    class SchoolDetails
    {
        public string Name { get; set; } 
        public string Address { get; set; } 
        public string PostalCode { get; set; } 
        public string City { get; set; } 
        public string Teilort { get; set; } 
        public string NumberOfPlaces { get; set; } 
        public string Type { get; set; }
        public string SchoolCommunity { get; set; } 
        public string Url { get; set; } 
        public string OpeningHours { get; set; }

        public static List<SchoolDetails> GetPageDetails(List<string> urls)
        {
            var lstPageDetails = new List<SchoolDetails>();
            bool isError = false;
            foreach (var url in urls)
            {
                try
                {
                    var htmlNode = Scrapper.GetHtml(url);
                    var pageDetails = new KitadeScrapper.SchoolDetails();
                    //
                    pageDetails.Name = ((HtmlAgilityPack.HtmlNode[])htmlNode.OwnerDocument.GetElementbyId("profile").CssSelect("h1"))[0].InnerText.ToString(); //A
                    string addressBlock = ((HtmlAgilityPack.HtmlNode[])htmlNode.OwnerDocument.GetElementbyId("profile").CssSelect("p.address"))[0].InnerHtml.ToString(); //B, C, D, E, 

                    string[] arg1 = Regex.Split(addressBlock, @"<br>");
                    string[] arg2 = Regex.Split(arg1[1], @"\ ");
                    if (arg1[2].Contains("\\"))
                    {
                        string[] arg3 = Regex.Split(arg1[2], @"\-");
                        pageDetails.Teilort = arg3[1];
                    }
                    else
                        pageDetails.Teilort = arg1[2];

                    pageDetails.Address = arg1[0].ToString();
                    pageDetails.PostalCode = arg2[0];
                    pageDetails.City = arg2[1];

                    pageDetails.NumberOfPlaces = ((HtmlAgilityPack.HtmlNode[])htmlNode.OwnerDocument.GetElementbyId("tab-informationen-content").CssSelect("td"))[2].InnerText.ToString().Replace( "/ ?",""); //F
                    //pageDetails.NumberOfPlaces =((HtmlAgilityPack.HtmlNode[])htmlNode.OwnerDocument.GetElementbyId("tab-informationen-content").CssSelect("td div.progress-bar div.percentage"))[0].InnerText.ToString().Replace(" / ?", "")


                    pageDetails.Type = ((HtmlAgilityPack.HtmlNode[])htmlNode.OwnerDocument.GetElementbyId("profile").CssSelect("h2"))[0].InnerHtml.ToString(); //G

                    if (((HtmlAgilityPack.HtmlNode[])htmlNode.OwnerDocument.GetElementbyId("tab-informationen-content").CssSelect("td.traeger")).Length > 0)
                        pageDetails.SchoolCommunity = ((HtmlAgilityPack.HtmlNode[])htmlNode.OwnerDocument.GetElementbyId("tab-informationen-content").CssSelect("td.traeger"))[0].InnerText.ToString();
                   // pageDetails.SchoolCommunity = ((HtmlAgilityPack.HtmlNode[])htmlNode.OwnerDocument.GetElementbyId("tab-informationen-content").CssSelect("td"))[0].InnerText.ToString(); //H

                    if (((HtmlAgilityPack.HtmlNode[])htmlNode.OwnerDocument.GetElementbyId("profile").CssSelect("p.www a")).Length > 0)//(htmlNode.OwnerDocument.GetElementbyId("profile").CssSelect("p.www") != null)//(htmlNode.OwnerDocument.GetElementbyId("profile").HasClass("www"))
                        pageDetails.Url = ((HtmlAgilityPack.HtmlNode[])htmlNode.OwnerDocument.GetElementbyId("profile").CssSelect("p.www a"))[0].Attributes["href"].Value;

                    // pageDetails.Url = htmlNode.OwnerDocument.GetElementbyId("profile")

                    if (htmlNode.OwnerDocument.GetElementbyId("profile_text") != null) //htmlNode.OwnerDocument.GetElementbyId("tab-informationen-content").HasClass("www"))
                        pageDetails.OpeningHours = ((HtmlAgilityPack.HtmlNode[])htmlNode.OwnerDocument.GetElementbyId("tab-informationen-content").CssSelect("p"))[0].InnerText.ToString().Replace("\r", ""); //J
                    else
                        pageDetails.OpeningHours = "Not Available";

                    lstPageDetails.Add(pageDetails);
                }
                catch(Exception e)
                {
                    isError = true;
                    Console.WriteLine(e.ToString());
                }
            }
            return lstPageDetails;
        }

    }
}
