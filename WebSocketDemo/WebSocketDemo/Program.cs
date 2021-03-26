using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebSocketSharp;

namespace WebSocketDemo
{
    class Program
    {
        static void Main(string[] args)
        {

            var hellomessage = "Hello World";
            //Console.WriteLine("Hello World");
            displaystuff(hellomessage);
            TestWebSocketDemo();

            Console.ReadLine();

        }

        //as per steps in 
        //https://github.com/sta/websocket-sharp
        static void TestWebSocketDemo()
        {
            var connectionhost = "wss://echo.websocket.org";
            var ws = new WebSocket(connectionhost);
            ws.SslConfiguration.EnabledSslProtocols = System.Security.Authentication.SslProtocols.Tls12;


            ws.OnOpen += (sender, e) =>
            {
                displaystuff("connected to the web service");
            };

            ws.OnClose += (sender, e) => {
                displaystuff("DIS connected to the web service");
            };

            ws.OnMessage += (sender, e) => {
                displaystuff("------Messagae from Server------");
                if (e.IsText)
                {
                    // Do something with e.Data.
                    displaystuff(e.Data);

                    return;
                }

                if (e.IsBinary)
                {
                    // Do something with e.RawData.
                    displaystuff(e.RawData.ToString());

                    return;
                }
                displaystuff("------Messagae from Server------");
            };

            ws.Connect();

            var message1 = "That's what she said - Michael Scott";
            var message2 = "Hail to the King Baby - Ash Williams";
            ws.Send(message1);
            ws.Send(message2);

            ws.Close();
        }

        static void displaystuff(string somestring)
        {
            Console.WriteLine(somestring);
            Console.WriteLine("\n");
        }

    }
}
