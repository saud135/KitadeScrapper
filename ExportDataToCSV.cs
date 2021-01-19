using System;
using System.Collections.Generic;
using System.Text;
using ScrapySharp.Extensions;
using ScrapySharp.Network;
using HtmlAgilityPack;
using System.IO;
using System.Globalization;
using CsvHelper;
using CsvHelper.Configuration;

namespace KitadeScrapper
{
    class ExportDataToCSV
    {
        public ExportDataToCSV(List<SchoolDetails> lstPageDetails)
        {
            string strFilePath = Environment.GetEnvironmentVariable("csvRecordsPath");//@"D:\Lenovo Imports\Disk D\VS19 Projects\Data.csv";

            // Do not include the header row if the file already exists
            CsvConfiguration csvConfig = new CsvConfiguration(CultureInfo.CurrentCulture)
            {
                HasHeaderRecord = !File.Exists(strFilePath)
            };

            // WARNING: This will throw an error if the file is open in Excel!
            using (FileStream fileStream = new FileStream(strFilePath, FileMode.Append, FileAccess.Write, FileShare.ReadWrite))
            {
                using (StreamWriter streamWriter = new StreamWriter(fileStream))
                {
                    using (var csv = new CsvWriter(streamWriter, csvConfig))
                    {
                        // Append records to csv
                        csv.WriteRecords(lstPageDetails);
                    }
                }
            }

            //using (var writer = new StreamWriter(strFilePath,true))
            //using (var csv = new CsvWriter(writer, CultureInfo.InvariantCulture))
            //csv.WriteRecordsAsync(lstPageDetails);

            //bool append = true;
            //var config = new CsvConfiguration(CultureInfo.InvariantCulture);
            //config.HasHeaderRecord = !append;

            //using (var writer = new StreamWriter(strFilePath, append))
            //{
            //    using (var csv = new CsvWriter(writer, config))
            //    {
            //        csv.WriteRecordsAsync(lstPageDetails);
            //    }
            //}

        }
    }
}
