using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Mail;
using System.Threading;
namespace Twitter_Scrapper
{
    class Program
    {
        static void Main(string[] args)
        {
            for(; ; )
            {
                Downloader();
                Thread.Sleep(60000);
            }
        }
        private static void Downloader()
        {
            try
            {
                var links = new Dictionary<string, string>() {

                {"Ripple.html","https://twitter.com/Ripple" } };
               
                //pobieranie postu
                foreach (var link in links)
                {
                    var website = new WebClient();
                    var html = website.DownloadString(link.Value);
                    File.WriteAllText($"C:\\Users\\user\\Desktop\\twitter_scrapper\\Twitter_Scrapper\\bin\\Debug\\net5.0\\{link.Key}", html);
                    Thread.Sleep(1500);
                }
                //koniec pobierania postu

                //sprawdzanie czy jest post
                foreach (var link in links)
                {
                    Thread.Sleep(5000);
                    var html_ = $"C:\\Users\\user\\Desktop\\twitter_scrapper\\Twitter_Scrapper\\bin\\Debug\\net5.0\\{link.Key}";
                    var web = new HtmlWeb();
                    var xd = web.Load(html_);
                    var node = xd.DocumentNode.SelectNodes("//*[contains(@class,'_timestamp js-short-timestamp js-relative-timestamp')]");
                    if (node != null)
                    //koniec sprawdzania czy jest post
                    {
                        //porównanie czy post jest młodszy niż 2 min
                        foreach (var x in node)
                        {
                            var dateNow = DateTime.Now;
                            var date = (new DateTime(1970, 1, 1)).AddMilliseconds(double.Parse(x.Attributes["data-time-ms"].Value));
                            var time = dateNow - date;
                            var timePars = time.Minutes;
                            if (timePars < 2)
                            //koniec porównania
                            {
                                var maile = new List<string>()
            {
            //adresy mail do wysłania
                "MailTo",
                "AnotherMail"
            };
                                foreach (var _mail in maile)
                                {
                                    //wysyłanie maila
                                    try
                                    {
                                        using (MailMessage mail = new MailMessage())
                                        using (SmtpClient smtpClient = new SmtpClient())
                                        {
                                            smtpClient.Host = "smtp-mail.outlook.com";
                                            smtpClient.Port = 587;
                                            smtpClient.Credentials = new NetworkCredential("Sender", "Sender Password");
                                            smtpClient.EnableSsl = true;
                                            mail.IsBodyHtml = true;
                                            mail.Attachments.Add(new Attachment($"C:\\Users\\user\\Desktop\\twitter_scrapper\\Twitter_Scrapper\\bin\\Debug\\net5.0\\{link.Key}"));
                                            mail.From = new MailAddress("Sender", "Title");
                                            mail.To.Add(_mail);
                                            mail.Subject = $"{link.Key}";
                                            smtpClient.Send(mail);
                                            Console.WriteLine("Wysłane, czekam..");
                                            Thread.Sleep(5000);
                                        }
                                    }
                                    catch (Exception error) { Console.WriteLine(error); }
                                }
                            }
                        }
                    }
                    else Console.WriteLine("Nic tu nie ma..");
                }
            }
            catch (Exception error)
            {
                Console.WriteLine(error.Message);
            }
        }
    }
}
