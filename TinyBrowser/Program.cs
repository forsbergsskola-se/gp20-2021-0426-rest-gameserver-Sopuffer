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
            HttpGetRequest("http://acme.com");
            //Start();
            Console.ReadKey();


        }

        async static void HttpGetRequest(string website)
        {
            var tcpClient = new TcpClient("acme.com", 80);
            var stream = tcpClient.GetStream();

            using (HttpClient httpClient = new HttpClient())
            {
                using (HttpResponseMessage responseMessage = await httpClient.GetAsync(website))
                {
                    using (HttpContent content = responseMessage.Content)
                    {
                        string contentMessage = await content.ReadAsStringAsync();

                        Console.WriteLine(contentMessage);

                        byte[] bytes = Encoding.ASCII.GetBytes(contentMessage);
                        stream.Write(bytes);
                    }
                }
            }
        }
        //static void Start()
        //{
        //    var tcpClient = new TcpClient("acme.com", 80);
        //    var stream = tcpClient.GetStream();
        //    // Get the bytes for our HTTP Request
        //    var bytes = Encoding.ASCII.GetBytes("GET / HTTP/1.1\r\nHost: acme.com\r\n\r\n");
        //    // Send them over TCP
        //    stream.Write(bytes);
        //}


    }
}
