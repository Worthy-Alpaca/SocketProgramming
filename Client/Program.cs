using Newtonsoft.Json.Linq;
using Shared;
using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace Client
{

    public class MyMessage
    {
        public string StringProperty { get; set; }
        public int IntProp { get; set; }
    }




    class Program
    {
        static async Task Main(string[] args)
        {
            Console.WriteLine("Press Enter To Connect");
            Console.ReadLine();

            var endPoint = new IPEndPoint(IPAddress.Loopback, 9000);

            //var channel = new ClientChannel<JSONMessageProtocol, JObject>();
            var channel = new ClientChannel<XmlMessageProtocol, XDocument>();

            channel.OnMessage(OnMessage);

            await channel.ConnectAsync(endPoint).ConfigureAwait(false);

            var myMessage = new MyMessage
            {
                IntProp = 404,
                StringProperty = "Hello World"
            };

            Console.WriteLine("Sending");
            Print(myMessage);

            await channel.SendAsync(myMessage).ConfigureAwait(false);

            Console.ReadLine();
        }

        static Task OnMessage(JObject jObject)
        {
            Console.WriteLine("Received JObject Message");
            Print(Convert(jObject));
            return Task.CompletedTask;
        }

        static Task OnMessage(XDocument xDocument)
        {
            Console.WriteLine("Received XDoc Message");
            Print(Convert(xDocument));
            return Task.CompletedTask;
        }

        static MyMessage Convert(JObject jObject)
            => jObject.ToObject(typeof(MyMessage)) as MyMessage;

        static MyMessage Convert(XDocument xmlDocument)
            => new XmlSerializer(typeof(MyMessage)).Deserialize(new StringReader(xmlDocument.ToString())) as MyMessage;

        static void Print(MyMessage m) => Console.WriteLine($"MyMessage.IntProp = {m.IntProp}, MyMessage.StringProp = {m.StringProperty}");
    }
}
