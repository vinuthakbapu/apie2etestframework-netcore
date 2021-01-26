using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Web;
using System.Xml;
using System.Xml.Xsl;

namespace TrxerConsole
{
    class Program
    {
        private const string XSLT_FILE = "Trxer.xslt";
        private const string OUTPUT_FILE_EXT = ".html";
       
        public static void Main(string[] args)
        {
            string path1 = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            string[] filePaths = Directory.GetFiles($"{path1}/TestResults", "*.trx", SearchOption.TopDirectoryOnly);
            Console.WriteLine("filePaths-->{0}" + filePaths[0]);
            Transform(filePaths[0], PrepareXsl());
        }

        private static void Transform(string fileName, XmlDocument xsl)
        {
            OverrideSpecCharacters(fileName);
            XslCompiledTransform x = new XslCompiledTransform(true);
            x.Load(xsl, new XsltSettings(true, true), new XmlUrlResolver());
            Console.WriteLine("Transforming...");
            x.Transform(fileName, fileName + OUTPUT_FILE_EXT);
            Console.WriteLine("Done transforming xml into html");
        }

        private static void OverrideSpecCharacters(string fileName)
        {
            string text = File.ReadAllText(fileName);
            string textWithDuration = Regex.Replace(text, "queuing=\"(.*)\" start", $"queuing=\"{GetTestsDuration(text)}\" start", RegexOptions.ExplicitCapture);
            string decodedText = HttpUtility.HtmlDecode(textWithDuration);
            File.WriteAllText(fileName, decodedText);
            text = text.Replace(" & ", "&amp;");
            text = text.Replace("<>", "&lt;&gt;");
            File.WriteAllText(fileName, text);
        }

        private static string GetTestsDuration(string text)
        {
            string start = new Regex("start=\"(?<x>.*)\" finish").Match(text).Groups["x"].Value;
            string end = new Regex("finish=\"(?<x>.*)\" />").Match(text).Groups["x"].Value;
            TimeSpan timeSpan = DateTime.Parse(end)-DateTime.Parse(start);
            return timeSpan.ToString("hh':'mm':'ss'.'fff");
        }

        private static XmlDocument PrepareXsl()
        {
            XmlDocument xslDoc = new XmlDocument();
            Console.WriteLine("Loading xslt template...");
            xslDoc.Load(ResourceReader.StreamFromResource(XSLT_FILE));
            MergeCss(xslDoc);
            MergeJavaScript(xslDoc);
            return xslDoc;
        }

        private static void MergeJavaScript(XmlDocument xslDoc)
        {
            Console.WriteLine("Loading javascript...");
            XmlNode scriptEl = xslDoc.GetElementsByTagName("script")[0];
            XmlAttribute scriptSrc = scriptEl.Attributes["src"];
            string script = ResourceReader.LoadTextFromResource(scriptSrc.Value);
            scriptEl.Attributes.Remove(scriptSrc);
            scriptEl.InnerText = script;
        }

        private static void MergeCss(XmlDocument xslDoc)
        {
            Console.WriteLine("Loading css...");
            XmlNode headNode = xslDoc.GetElementsByTagName("head")[0];
            XmlNodeList linkNodes = xslDoc.GetElementsByTagName("link");
            List<XmlNode> toChangeList = linkNodes.Cast<XmlNode>().ToList();

            foreach (XmlNode xmlElement in toChangeList)
            {
                XmlElement styleEl = xslDoc.CreateElement("style");
                styleEl.InnerText = ResourceReader.LoadTextFromResource(xmlElement.Attributes["href"].Value);
                headNode.ReplaceChild(styleEl, xmlElement);
            }
        }
    }
}
