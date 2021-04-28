using System;
using System.Net;
using System.Net.Sockets;
using System.Collections.Generic;
using System.Text;
using System.Net.Http;
using System.Net.Http.Headers;
using System.IO;
namespace gp20_2021_0426_rest_gameserver_Sopuffer
{
    class Program
    {
        static void Main(string[] args)
        {
            // HttpGetRequest("http://acme.com");
            Start();
            Console.ReadKey();
            
        }
        static void Start()
        {
            var tcpClient = new TcpClient("acme.com", 80);
            var stream = tcpClient.GetStream();
            var bytes = Encoding.ASCII.GetBytes("GET / HTTP/1.1\r\nHost: acme.com\r\n\r\n");
            stream.Write(bytes);

            string test = Encoding.ASCII.GetString(bytes);
            Console.WriteLine(test);

            byte[] resultBytes = new byte[124 * 124];
            var totalBytesReceived = 0;
            var bytesReceived = 1;
            while (bytesReceived != 0)
            {
                bytesReceived = stream.Read(resultBytes, totalBytesReceived, resultBytes.Length - totalBytesReceived);
                totalBytesReceived += bytesReceived;
            }
            string website = Encoding.ASCII.GetString(resultBytes, 0, totalBytesReceived);
            FindTitleOfWebsite(website, "<title>", "</title>");
           
        }

        public static void FindTitleOfWebsite(string text, string firstString, string lastString)
        {
            string content = text;
            string STRFirst = firstString;
            string STRLast = lastString;

            int Pos1 = content.IndexOf(STRFirst) + STRFirst.Length;
            int Pos2 = content.IndexOf(STRLast);
            string FinalString = content.Substring(Pos1, Pos2 - Pos1);
            Console.WriteLine("Title: " + STRFirst + FinalString + STRLast);
        }
        public static void FindHRef(string reference, string lastString, string text)
        {
            string content = text;
            string STRFirst = reference;
            string STRLast = lastString;

            int Pos1 = content.IndexOf(STRFirst) + STRFirst.Length;
            int Pos2 = content.IndexOf(STRLast);
          
        }
        //async static void HttpGetRequest(string website)
        //{
        //    var tcpClient = new TcpClient("acme.com", 80);
        //    var stream = tcpClient.GetStream();

        //    using (HttpClient httpClient = new HttpClient())
        //    {
        //        using (HttpResponseMessage responseMessage = await httpClient.GetAsync(website))
        //        {
        //            using (HttpContent content = responseMessage.Content)
        //            {
        //                string contentMessage = await content.ReadAsStringAsync();
        //                byte[] bytes = Encoding.ASCII.GetBytes(contentMessage);
        //                stream.Write(bytes);

        //                if(contentMessage.Contains("<title>") && contentMessage.Contains("</title>"))
        //                {
        //                    HttpGetTitle("<title>", "</title>", contentMessage);
        //                }

        //            }
        //        }
        //    }
        //}

        //async static void HttpGetTitle(string firstString, string lastString, string content)
        //{

        //    string text = content;
        //    string STRFirst = "<title>";
        //    string STRLast = "</title>";

        //    int Pos1 = text.IndexOf(STRFirst) + STRFirst.Length;
        //    int Pos2 = text.IndexOf(STRLast);
        //    string FinalString = text.Substring(Pos1, Pos2 - Pos1);
        //    Console.WriteLine("Title: " + STRFirst + FinalString + STRLast);
        //}


    }
}
