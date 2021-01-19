using System;
using System.Collections.Generic;
using ScrapySharp.Extensions;
using ScrapySharp.Network;
using HtmlAgilityPack;
using System.IO;
using System.Globalization;
using CsvHelper;

namespace KitaDeScrapper
{
    class Program
    {
        static void Main(string[] args)
        {
            string csvRecordsPath = "";//{  @"D:\SomeFolder\" + "KitaDeSchoolRecords.csv"  };

            if (!Directory.Exists(csvRecordsPath)) //bin as default csvRecords path
                Environment.SetEnvironmentVariable("csvRecordsPath", Path.GetFullPath(Path.Combine(Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().GetName().CodeBase)?.Replace("file:\\", ""), @"..\..\")) + "KitaDeSchoolRecords.csv");
            else
                Environment.SetEnvironmentVariable("csvRecordsPath", csvRecordsPath);
        


            KitadeScrapper.Scrapper scrapper = new KitadeScrapper.Scrapper();
        }
    }
}
