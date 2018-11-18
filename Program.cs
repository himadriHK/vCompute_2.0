using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectCore
{
    class Program
    {
        static void Main(string[] args)
        {
            _Server server = new _Server();
            _Client client = new _Client(string.Empty);
            string str = "No result";
            server.Start();
            client.RegisterClient().Wait();
            Console.WriteLine(client.NodeName);
            Console.Read();
        }
    }

}
