using System;
using System.Collections.Generic;
using System.Text;
using ScrapySharp.Extensions;
using ScrapySharp.Network;
using HtmlAgilityPack;
using System.IO;
using System.Globalization;
using CsvHelper;

namespace KitadeScrapper
{
    class ExportDataToCSV
    {
        public ExportDataToCSV(List<SchoolDetails> lstPageDetails)
        {
            string strFilePath = @"D:\Lenovo Imports\Disk D\VS19 Projects\Data.csv";

            using (var writer = new StreamWriter(strFilePath))
            using (var csv = new CsvWriter(writer, CultureInfo.InvariantCulture))
            csv.WriteRecords(lstPageDetails);
        }
    }
}
