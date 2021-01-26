using System;
using System.IO;
using System.Reflection;
using System.Xml;

namespace TestCommonUtils
{
    public class XmlHelper
    {
        public static string GetKey(string key)
        {
            XmlDocument doc = null;
            XmlNode titleNode = null;
            try
            {
                doc = new XmlDocument();
                string currentFolder = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
                currentFolder = $"{currentFolder}/TestData/APTestData.xml";
                doc.Load(currentFolder);

                String envText = doc.SelectSingleNode("//Environment").InnerText;
                XmlNodeList itemNodes = doc.SelectNodes("//" + envText);
                foreach (XmlNode itemNode in itemNodes)
                {
                    titleNode = itemNode.SelectSingleNode(key);
                    if (titleNode == null)
                    {
                        titleNode = itemNode.SelectSingleNode("//" + key);
                    }
                    Console.WriteLine(titleNode.Name + ":\t" + titleNode.InnerText);
                }
            }catch(Exception e)
            {
                Console.WriteLine("XmlHelper Getkey Exception-->{0}" + e.StackTrace);
            }
            finally
            {
                doc = null;
                GC.Collect();
            }
            return titleNode.InnerText;
        }
        public static void SetKey(string key,string val)
        {
            XmlDocument doc = null;
            try
            {
                doc = new XmlDocument();

                string currentFolder = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
                currentFolder = $"{currentFolder}/TestData/APTestData.xml";
                doc.Load(currentFolder);

                XmlNode versionNode = doc.GetElementsByTagName(key)[0];
                versionNode.InnerText = val;
                doc.Save(currentFolder);
                Console.WriteLine("Updated the key {0} with value {1}", key, val);
            }
            catch (Exception e)
            {
                Console.WriteLine("XmlHelper SetKey Exception-->{0}" + e.StackTrace);
            }
            finally
            {
                doc = null;
                GC.Collect();
            }
        }
    }
}