﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;

namespace QIC.Farm.WebApi.Common
{
    public class HtmlReportHelper
    {
        public static string CreateHtmlReport(string filename, string htmlContent)
        {
            var fullName = "Report\\" + filename + ".html";

            string templatePath = System.IO.Path.Combine(System.AppDomain.CurrentDomain.BaseDirectory, "App_Data\\Template\\report.html");
            string templateFolder = templatePath.Substring(0, templatePath.LastIndexOf('\\'));
            if (!Directory.Exists(templateFolder))
            {
                Directory.CreateDirectory(templateFolder);
            }
            FileStream templateFileStream = File.Open(templatePath, FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.ReadWrite);
            StreamReader streamReader = new StreamReader(templateFileStream, Encoding.UTF8);
            var formatter = streamReader.ReadToEnd();
            //替换body
            var html = string.Format(formatter, htmlContent);
            streamReader.Close();
            templateFileStream.Close();

            string reportPath = System.IO.Path.Combine(System.AppDomain.CurrentDomain.BaseDirectory, fullName);
            string reportFolder = reportPath.Substring(0, reportPath.LastIndexOf('\\'));
            if (!Directory.Exists(reportFolder))
            {
                Directory.CreateDirectory(reportFolder);
            }
            FileStream reportFileStream = File.Open(reportPath, FileMode.OpenOrCreate, FileAccess.Write, FileShare.ReadWrite);
            StreamWriter streamWriter = new StreamWriter(reportFileStream, Encoding.UTF8);
            streamWriter.Write(html + "\r\n");
            streamWriter.Flush();
            streamWriter.Close();
            reportFileStream.Close();
            return fullName;
        }


        public static string ReaderHtmlReportContent(string filename)
        {
            var path = System.IO.Path.Combine(System.AppDomain.CurrentDomain.BaseDirectory, filename);
            FileStream fileStream = File.Open(path, FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.ReadWrite);
            StreamReader streamReader = new StreamReader(fileStream, Encoding.UTF8);
            var str = streamReader.ReadToEnd();
            streamReader.Close();
            fileStream.Close();

            var startIndex = str.IndexOf("<body>");
            var endIndex = str.IndexOf("</body>") - 6;

            return str.Substring(startIndex + 6, endIndex - startIndex).Trim();
        }
    }
}