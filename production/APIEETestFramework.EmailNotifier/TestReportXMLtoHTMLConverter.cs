namespace CustomTestReport
{
    using System;
    using System.IO;
    using System.Xml.Linq;
    internal class TestReportXmLtoHtmlConverter
    {
        public void XmLtoHtmlTestReport(string xmlFilePath, string reportForAttachment, string env)
        {
            XmLtoHtmlTestReportForAttachment(xmlFilePath, reportForAttachment, env);
        }
        public void XmLtoHtmlTestReportForAttachment(string xmlFilePath, string outputFilePath, string env)
        {
            try
            {

                Console.WriteLine("Arguments : Count12");
                if (!File.Exists(xmlFilePath)) return;
                var xmlFile = XDocument.Load(xmlFilePath);
                var rootElement = xmlFile.Root;
                if (rootElement == null) return;
                var result = true;
                const string sampleTemplate = "<!DOCTYPE html><html><head><meta http-equiv=\"content - type\" content=\"text/html; charset=UTF-8\" /><title>FCS E2E EOM API TestAutomation Execution Report</title><script type=\"text/javascript\" src=\"http://code.jquery.com/jquery-1.6.2.min.js\"></script><style type=\"text/css\">body{color: #000000;font-family: Calibri,Liberation Sans,DejaVu Sans,sans-serif;line-height: 130%;}h1 {font-family: Calibri,Liberation Sans,DejaVu Sans,sans-serif;font-size: 170%;font-weight: bold;} h2 {font-family: Calibri,Liberation Sans,DejaVu Sans,sans-serif;font-size: 130%;font-weight: bold;margin-bottom: 5px;} h3 {font-family: Calibri,Liberation Sans,DejaVu Sans,sans-serif;font-size: 120%;font-weight: bold;margin-bottom: 5px;}table.overall{border-collapse: collapse;}.overall th, td {border: 1px solid black;padding: 10px;text-align: left;} a.bar{text-decoration: none;display: block;line-height: 1px;}.description{font-style: italic;}.log {width: 600px;white-space: pre-wrap;display: block;margin: 0px;}.errorMessage {width: 600px;color: Red;font-weight: bold;}.stackTrace {width: 600px;white-space: pre-wrap;font-style: italic;color: Red;display: block;}1px solid black;}table.testEvents{border: solid 1px #e8eef4;border-collapse: collapse;}table.testEvents td{vertical-align: top;padding: 5px;border: solid 1px #e8eef4;}table.testEvents th{padding: 6px 5px;text-align: left;background-color: #e8eef4;border: solid 1px #e8eef4; }.comment{font-style: italic;font-size: smaller;}.startupBar{background-color: #EEEEEE;cursor: default;}.colorSucceeded{background-color: #90ED7B;}.colorIgnored{background-color: #FFFF85;}.colorPending{background-color: #D47BED;}.colorNothingToRun{background-color: #CCCCFF;}.colorSkipped{background-color: #CCCCFF;}.colorInconclusive{background-color: #7BEDED;}.colorCleanupFailed{background-color: #FFCCCC;}.colorRandomlyFailed{background-color: #EDB07B;}.colorFailed{background-color: #ED5F5F;}.colorInitializationFailed{background-color: #FF0000;}.colorFrameworkError{background-color: #FF0000;}ul.subNodeLinks{padding-left: 20px;margin: 0px;}ul.subNodeLinks li{list-style: none;}/* views general */div.scrollable{/*overflow: auto; - thshas to be set from js, because of an IE9 bug */}div.viewbox{position: relative;border: 3px solid #e8eef4;}div.viewbox table{border: 0px;}   /* testview */#testview{padding-top: 23px;}table.testview-items td{vertical-align: bottom;padding: 0px 1px 0px 1px;}td.right-padding, td.left-padding{width: 25px;min-width: 25px;}table.testview-items a.bar{width: 5px;}table.testview-items tr.testview-items-row{height: 60px;}/* scale */table.vertical-scale {position: absolute;top: 23px;left: 0px;width: 100%;z-index: -100;}table.vertical-scale td, tr.horizontal-scale td{font-size: 60%;line-height: normal;}table.vertical-scale tr.scale-max, table.vertical-scale tr.scale-mid {height: 30px;}tr.horizontal-scale, table.vertical-scale tr.scale-min {height: 12px;}td.scale-max-label, td.scale-mid-label, td.scale-min-label{border-top: solid 1px #E6E6E6;text-align: left;vertical-align: top;}td.scale-10-label{border-left: solid 1px #E6E6E6;text-align: left;vertical-align: bottom;padding-left: 1px;}tr.scale-mid td, tr.scale-min td, tr.scale-max td{border-top: solid 1px #E6E6E6;}/* bar-control */#bar-control{font-size: 60%;line-height: normal;position: absolute;right: 0px;top: 0px;}#bar-control label{font-weight: bold;vertical-align: middle;}#bar-control .option{vertical-align: middle;text-transform: lowercase;}#bar-control input[type=\"checkbox\"]{padding: 0 2px 0 3px;}#bar-control input{vertical-align: top;height: 12px;margin: 0px;padding: 0px;}#bar-control div{float: right;margin: 3px 5px 3px 5px;}/* timeline view */#timelineview{padding-top: 5px;}table.timelineview a{height: 20px;}table.timelineview td{vertical-align: bottom;padding: 0px 1px 0px 0px;border: 0px;}tr.thread-items-row{height: 25px;}tr.thread-items-row td{vertical-align: bottom;}td.thread-label{padding: 0px 6px 0px 6px;text-align: right;line-height: 18px;vertical-align: bottom;}th.thread-label{padding: 3px 6px 0px 6px;line-height: 18px;text-align: left;vertical-align: bottom;}</style></head><body><h2>FCS E2E EOM API Automation Execution Report</h2><br /><table class=\"testEvents\"><tr><th><b>Project:  </b></th><td>FCS E2E EOM</td></tr><tr><th><b>Environment:  </b></th><td><a href=#envlink >#envlink</a></td></tr><!--<tr><td><b>Test Assemblies:  </b></td><td>#Assembly</td></tr>--><tr><th><b>Start Time:  </b></th><td>#STime</td></tr><tr><th><b>End Time:  </b></th><td>#ETime</td></tr><tr><th><b>Duration:  </b></th><td>#Dur</td></tr><!--<tr><td><b>Test Execution link(JIRA):  </b></td><td></td></tr><tr><td><b>Productivity Savings:(Mins)  </b></td><td>#ProdSaving</td></tr><tr><td><b>Engine Version: </b> </td><td>#EngVer</td></tr><tr><td><b>Clr Version:  </b></td><td>#ClrVer</td></tr>--></table><br/><!--<div #StatusColor><h2>Result: #Result</h2></div>--><table class=\"testEvents\"><tr><th>Success rate</th><th>Tests</th><th>Succeeded</th><th>Failed</th><th>Inconclusive</th><th>Skipped</th></tr><tr><td>#successrate</td><td>#CumTotal</td><td>#CumPass</td><td>#CumFail</td><td>#CumIncon</td><td>#CumSkip</td></tr></table><br/><br/></body></html>";
                var objDetailsFinder = new DetailsFinder();
                var finalContent = CheckForError(objDetailsFinder.GetOverallSummaryDetails(rootElement, sampleTemplate,env), sampleTemplate, ref result);
                OutputReportWrite:
                if (File.Exists(outputFilePath))
                {
                    File.WriteAllText(outputFilePath, finalContent);
                    Console.WriteLine("From existing file Body");
                }
                else
                {
                    File.Create(outputFilePath).Close();
                    File.WriteAllText(outputFilePath, finalContent);
                    Console.WriteLine("From new file Body");
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                Console.ReadLine();
            }
        }
        public string CheckForError(string checkOutputOfDetailsFinderContent, string mainContent, ref bool result)
        {
            if (checkOutputOfDetailsFinderContent.Substring(0, 6) != "Error:")
            {
                result = true;
                return checkOutputOfDetailsFinderContent;
            }
            result = false;
            mainContent = mainContent.Replace("<table class=\"overall\" style=\"width: 100 %; display: none;\">", "<table class=\"overall\" style=\"width: 100 %; display: block;\">");
            mainContent = mainContent.Replace("#ErrorMessage", checkOutputOfDetailsFinderContent);
            return mainContent;
        }
    }
}
