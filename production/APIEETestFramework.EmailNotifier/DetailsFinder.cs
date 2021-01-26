namespace CustomTestReport
{
    using System;
    using System.Xml.Linq;
    internal class DetailsFinder
    {
        public string GetOverallSummaryDetails(XElement element, string fileContent, string env)
        {
            try
            {
                var childElementFilter = element.Element("filter");
                var childElementTestSuite = element.Element("test-suite");
                ReplaceContentInReportTemplate("#Env", childElementFilter?.Element("cat")?.Value, ref fileContent);
                if (childElementTestSuite != null)
                {                    
                    ReplaceContentInReportTemplate("#ProjectName", childElementTestSuite.Attribute("name")?.Value.Split('.')[0], ref fileContent);
                    ReplaceContentInReportTemplate("#Assembly", childElementTestSuite.Attribute("fullname")?.Value, ref fileContent);
                    ReplaceContentInReportTemplate("#Result", childElementTestSuite.Attribute("result")?.Value, ref fileContent);                    
                }

                var duration = Math.Round(Convert.ToDecimal(element.Attribute("duration")?.Value), 2) / 60;
                var hour = System.Math.Floor(duration / 60);
                Console.WriteLine("t.Hours-->{0}", hour);
                var min = System.Math.Floor(duration % 60);
                Console.WriteLine("t.Min-->{0}", min);
                ReplaceContentInReportTemplate("#Dur", hour + "hr " + min + "mins", ref fileContent);

                Console.WriteLine("envlink-->{0}", env);
                ReplaceContentInReportTemplate("#envlink", env, ref fileContent);

                var startTime = element.Attribute("start-time")?.Value;
                var endTime = element.Attribute("end-time")?.Value;
                ReplaceContentInReportTemplate("#STime", startTime, ref fileContent);
                ReplaceContentInReportTemplate("#ETime", endTime, ref fileContent);
                ReplaceContentInReportTemplate("#CumTotal", element.Attribute("total")?.Value, ref fileContent);
                ReplaceContentInReportTemplate("#CumPass", element.Attribute("passed")?.Value, ref fileContent);
                ReplaceContentInReportTemplate("#CumFail", element.Attribute("failed")?.Value, ref fileContent);
                ReplaceContentInReportTemplate("#CumIncon", element.Attribute("inconclusive")?.Value, ref fileContent);
                ReplaceContentInReportTemplate("#CumSkip", element.Attribute("skipped")?.Value, ref fileContent);
                var successrate = Convert.ToDouble(element.Attribute("passed")?.Value) * 100 /Convert.ToDouble(element.Attribute("total")?.Value);
                ReplaceContentInReportTemplate("#successrate", Math.Round(successrate) + "%", ref fileContent);
                var successWidthPercentage = ((200 * successrate / 100));
                var failureWidthPercentage = ((200 * (100 - successrate) / 100));
                var addSuccessColor = successWidthPercentage > 0? "background-color: #90ED7B;": "";
                var addFailureColor = failureWidthPercentage > 0? "background-color: #ED5F5F;": "";                               
                var successratebar = "<td style=\"" + addSuccessColor + "line-height: 1px; width: " + successWidthPercentage + "px;\"><a style=\"text-decoration: none; display:block;width: " + successWidthPercentage + "px;" + addSuccessColor + "\" title=\"" + element.Attribute("passed")?.Value + " succeeded\"></a></td><td style=\"width: " + failureWidthPercentage + "px;" + addFailureColor + "line - height: 1px;\"><a style=\"width: " + failureWidthPercentage + "px;" + addFailureColor + "text - decoration: none; display:block;\" title=\"" + element.Attribute("failed")?.Value + " failed\" href=\"#error_summary\" /></td>";
                ReplaceContentInReportTemplate("#successbar", successratebar, ref fileContent);
                ReplaceContentInReportTemplate("#EngVer", element.Attribute("engine-version")?.Value, ref fileContent);
                ReplaceContentInReportTemplate("#ClrVer", element.Attribute("clr-version")?.Value, ref fileContent);
                ReplaceContentInReportTemplate("#StatusColor", element.Attribute("passed")?.Value == element.Attribute("total")?.Value ? "style=\"color: #90ED7B;\"" : "style=\"color: #ED5F5F;\"", ref fileContent);
            }
            catch (Exception e)
            {
                fileContent = "Error: " + e.Message;
                Console.WriteLine("Error-->{0}", fileContent);
            }
            return fileContent;
        }

        private void ReplaceContentInReportTemplate(string original, string replaceContent, ref string template)
        {
            template = template.Replace(original, replaceContent);
        }       
        public string ConvertToIst(string date)
        {
            var len = date.Length;

            var time = DateTime.Parse(date.Substring(0, len - 1));
            var clientZone = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");
            var indianTime = TimeZoneInfo.ConvertTimeFromUtc(time, clientZone);
            Console.WriteLine(indianTime.ToString());
            return indianTime.ToString();
        }
    }
}
