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
            _Client client = new _Client();
            string str = "No result";
            Task.Run(async () =>
            {
                server.Start();
                str = await client.SendRequest();
            }).GetAwaiter().GetResult();
            
            Console.WriteLine(str);
            Console.Read();
        }
    }

}
