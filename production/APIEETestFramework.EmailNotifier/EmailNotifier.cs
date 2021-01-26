using CustomTestReport;
using Microsoft.Extensions.Configuration;
using System;
using System.IO;
using System.Net.Mail;
using System.Reflection;
using System.Text;

namespace EmailNotifier
{
    class Program
    {
        public static IConfiguration Configuration { get; } =
         new ConfigurationBuilder()
           .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
           .AddEnvironmentVariables()
           .Build();

        public static void Main(string[] args)
        {
            //---------------------------------Email body----------------------------------

            var objXmLtoHtmlReportGenerator = new TestReportXmLtoHtmlConverter();
            Console.WriteLine("Arguments : Count1 " + string.Join(",", args));
            string path = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            objXmLtoHtmlReportGenerator.XmLtoHtmlTestReport($"{path}/results.xml", $"{path}/TestResults/Body.html", Configuration["EmailNotification:Env"]);

            //---------------------------------Email script with attachment----------------------------------

            MailMessage mail = new MailMessage();

            SmtpClient SmtpServer = new SmtpClient("smtp.gmail.com", 587);
            mail.From = new MailAddress("SystemTeam <SystemTestAutomation@gmail.com>", "SystemTestTeam", Encoding.UTF8);
            mail.To.Add(Configuration["EmailNotification:ToList"]);
            mail.CC.Add(Configuration["EmailNotification:CcList"]);
            mail.Subject = "API E2E Automation Execution Report";
            var summaryFilePath = $"{path}/TestResults/Body.html";
            var bodyContent = string.Empty;

            if (File.Exists(summaryFilePath))
            {
                bodyContent = File.ReadAllText(summaryFilePath);
            }
            else
            {
                bodyContent = "";
            }

            mail.Body = bodyContent;
            mail.IsBodyHtml = true;
            mail.BodyEncoding = Encoding.UTF8;
            AlternateView htmlView = AlternateView.CreateAlternateViewFromString(bodyContent, Encoding.UTF8, "text/html");

            mail.AlternateViews.Add(htmlView);

            //------------Remove navigations--------------

            string path1 = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            string[] filePaths = Directory.GetFiles($"{path1}/TestResults", "_*.html", SearchOption.TopDirectoryOnly);
            Console.WriteLine("filePaths-->{0}" + filePaths[0]);
            System.Net.Mail.Attachment attachment;
            attachment = new System.Net.Mail.Attachment(filePaths[0]);
            mail.Attachments.Add(attachment);

            SmtpServer.Credentials = new System.Net.NetworkCredential("", "");
            SmtpServer.EnableSsl = true;

            SmtpServer.Send(mail);
        }
    }
}