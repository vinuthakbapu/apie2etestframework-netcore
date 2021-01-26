using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading;

namespace TestCommonUtils
{
    public class TempEmail
    {
        private CookieContainer Cookies { get; set; }

        [JsonProperty("total")]
        private string Total { get; set; }

        [JsonProperty("msgs")]
        private List<InboxEmail> Emails { get; set; }

        private int EmailCount { get; set; }

        private const string InboxURL = "https://getnada.com/api/v1/inboxes/";

        private const string Messages = "https://getnada.com/api/v1/messages/";

        /// <summary>
        /// This method will generate unique emailId with random nada domain
        /// </summary>
        /// <returns>Return unique email id</returns>
        public string GenerateEmailId()
        {
            try
            {
                var chars = "abcdefghijklmnopqrstuvwxyz0123456789";
                var stringChars = new char[10];
                var random = new Random();
                var emailDomains = new List<string> {
                    "@getnada.com",
                    "@abyssmail.com",
                    "@boximail.com",
                    "@clrmail.com",
                    "@dropjar.com",
                    "@getairmail.com",
                    "@givmail.com",
                    "@inboxbear.com",
                    "@robot-mail.com",
                    "@tafmail.com",
                    "@vomoto.com",
                    "@wmail.club",
                    "@zetmail.com",
                    "@banit.me",
                    "@nada.email",
                    "@nada.ltd",
                    "@banit.club"
                    };
                
                for (int i = 0; i < stringChars.Length; i++)
                {
                    stringChars[i] = chars[random.Next(chars.Length)];
                }

                var emailId = new String(stringChars) + "@vomoto.com";//emailDomains[random.Next(emailDomains.Count)];
                return emailId;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return null;
            }
        }

        public TempEmail GetInbox(string id)
        {
            try
            {
                Console.WriteLine("Started GetInbox");
                CookieContainer cookies = new CookieContainer();
                string url = InboxURL + id;
                Console.WriteLine("Started Response");
                Response response = HttpUtil.Get(url, cookies);
                Console.WriteLine("Started Dederialize");
                TempEmail tempmail = JsonConvert.DeserializeObject<TempEmail>(response.Json);
                this.Cookies = response.Cookies;
                this.EmailCount = tempmail.Emails.Count;
                this.Emails = tempmail.Emails;
                return tempmail;
            }
            catch (Exception ex)
            {
                Console.WriteLine("GetInbox {0}", ex.Message);
                return null;
            }

        }

        public Email GetMessageById(string uid)
        {
            CookieContainer cookies = new CookieContainer();
            string url = Messages + uid;
            Response response = HttpUtil.Get(url, cookies);
            this.Cookies = response.Cookies;
            Email emailMessage = JsonConvert.DeserializeObject<Email>(response.Json);
            return emailMessage;
        }

        public List<String> GetUserDetailsById(string uid)
        {
            List<String> userDetails = new List<String>();
            Email message = GetMessageById(uid);
            var msg = message.Text.Split(new string[] { " \n\n" }, StringSplitOptions.None);
            userDetails.Add(msg[0]);
            for (int i = 0; i < msg.Length; i++)
            {
                if (msg[i].Contains("USERNAME"))
                {
                    userDetails.Add(msg[i + 1]);
                }
                else if (msg[i].Contains("PASSWORD"))
                {
                    userDetails.Add(msg[i + 1]);
                }
            }

            return userDetails;
        }

        public List<String> GetCredentialsFromLastEmail(string id)
        {
            try
            {
                CookieContainer cookies = new CookieContainer();
                string url = InboxURL + id;
                Response response = HttpUtil.Get(url, cookies);
                TempEmail tempmail = JsonConvert.DeserializeObject<TempEmail>(response.Json);
                var userCred = GetUserDetailsById(tempmail.Emails.LastOrDefault().Uid);
                return userCred;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return null;
            }

        }

        public List<string> SearchMailBySubjectAndGetUserDetails(string id, string subject, int count)
        {        
            int counter = 0;
            TempEmail mailInbox = GetInbox(id);
            int EmailCount = mailInbox.Emails.Count;
            Console.WriteLine("Started while");
            while (EmailCount <= count && counter < 60)
            {
                //if(counter > 60)
                //{
                //    Console.WriteLine("Missing Creadentials Inbox");
                //    break;
                //}
                Thread.Sleep(TimeSpan.FromSeconds(10));
                var mailInboxNew = GetInbox(id);
                EmailCount = mailInboxNew.Emails.Count;
                Console.WriteLine("Email Count-->{0}" + EmailCount);
                Thread.Sleep(TimeSpan.FromSeconds(10));
                counter++;
            }
            Thread.Sleep(TimeSpan.FromSeconds(10));
            if (EmailCount > count) 
            {
                Console.WriteLine("Started EmailCount greater");
                var inbox = GetInbox(id);
                foreach (var item in inbox.Emails)
                {
                    if (item.Subject.Contains(subject))
                    {
                        var userCred = GetUserDetailsById(item.Uid);
                        return userCred;
                    }
                }
            }
            return null;
        }

        public int GetEmailCount(string id)
        {
            TempEmail tempmail = GetInbox(id);
            return tempmail.Emails.Count;
        }

        public string GetVerifyEmailAddressUrl(string id)
        {
            int counter = 0;
            TempEmail mailInbox = GetInbox(id);
            int EmailCount = mailInbox.Emails.Count;
            while (EmailCount <= 0 && counter < 60)
            {
                var mailInboxNew = GetInbox(id);
                EmailCount = mailInboxNew.Emails.Count;
                Thread.Sleep(TimeSpan.FromSeconds(5));
                counter++;
            }
            Thread.Sleep(TimeSpan.FromSeconds(10));
            if (EmailCount > 0)
            {
                var inbox = GetInbox(id);
                foreach (var item in inbox.Emails)
                {
                    if (item.Subject.Contains(XmlHelper.GetKey("VerifyEmailAddressSubject")))
                    {
                        Email message = GetMessageById(item.Uid);
                        var msg = message.Text.Split(new string[] { " \r\n\r\n" }, StringSplitOptions.None);
                        for (int i = 0; i < msg.Length; i++)
                        {
                            if (msg[i].Contains("copy and paste"))
                            {
                                return msg[i + 1];
                            }                         
                        }
                    }
                }
            }
            return null;
        }
    }
}
