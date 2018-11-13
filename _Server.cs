using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using System.Threading;
using static ProjectCore._Common;

namespace ProjectCore
{
    class _Server
    {
        public string Endpoint { get; set; }
        public bool Stop { get; set; }
        private Dictionary<string, string> nodeList;
        private Dictionary<string, object> resultStore;
        private CodeLoader.Loader codeLoader;
        private int nodeCtr = 1;
        private int taskCtr = 1;
        public _Server()
        {
            Endpoint = "http://localhost:8080/";
            Stop = false;
            nodeList = new Dictionary<string, string>();
            resultStore = new Dictionary<string, object>();
            codeLoader = new CodeLoader.Loader(AppDomain.CurrentDomain.BaseDirectory + @"server.bin");
        }

        public async void Start()
        {
            HttpListener server = new HttpListener();
            server.Prefixes.Add(Endpoint);
            server.Start();
            while (!Stop)
            {
                var listCtxt = await server.GetContextAsync();
                Thread th = new Thread(ProcessRequest);
                th.Start(listCtxt);
            }
        }

        private void ProcessRequest(object ctx)
        {
            HttpListenerContext listCtxt = (HttpListenerContext)ctx;
            StreamReader streamReader = new StreamReader(listCtxt.Request.InputStream);
            RequestPacket requestPacket = _Common.GetRequestPacket(streamReader.ReadToEnd());
            switch (requestPacket.Code)
            {
                case REQUEST_CODES.REGISTER_CLIENT:
                    RegisterClient(listCtxt.Request, listCtxt.Response);
                    break;
                case REQUEST_CODES.REGISTER_ASSEMBLY:
                    RegisterAssembly(requestPacket, listCtxt.Response);
                    break;
                case REQUEST_CODES.TASK:
                    SubmitTask(requestPacket, listCtxt.Response);
                    break;
                case REQUEST_CODES.RESULT:
                    SendResult(requestPacket, listCtxt.Response);
                    break;
                case REQUEST_CODES.STATUS:
                    UpdateStatus(requestPacket, listCtxt.Response);
                    break;
                case REQUEST_CODES.UNREGISTER_CLIENT:
                    UnregisterClient(requestPacket, listCtxt.Response);
                    break;
                case REQUEST_CODES.UNREGISTER_ASSEMBLY:
                    UnregisterAssembly(requestPacket, listCtxt.Response);
                    break;
            }
            //DateTime dateTime = DateTime.Now;
            //Thread.Sleep(100);
            //DateTime dateTime2 = DateTime.Now;
            //string str = "<html><body>Received at " + dateTime.ToString() + " Sent at " + dateTime2.ToString() + "</body></html>";
            //listCtxt.Response.OutputStream.Write(Encoding.ASCII.GetBytes(str), 0, str.Length);
            //listCtxt.Response.Close();
        }

        private void UnregisterAssembly(RequestPacket requestPacket, HttpListenerResponse response)
        {
            throw new NotImplementedException();
        }

        private void UnregisterClient(RequestPacket requestPacket, HttpListenerResponse response)
        {
            throw new NotImplementedException();
        }

        private void UpdateStatus(RequestPacket requestPacket, HttpListenerResponse response)
        {
            throw new NotImplementedException();
        }

        private void SendResult(RequestPacket requestPacket, HttpListenerResponse response)
        {
            throw new NotImplementedException();
        }

        private void SubmitTask(RequestPacket requestPacket, HttpListenerResponse response)
        {
            throw new NotImplementedException();
        }

        private void RegisterAssembly(RequestPacket requestPacket, HttpListenerResponse response)
        {
            throw new NotImplementedException();
        }

        private void RegisterClient(HttpListenerRequest request, HttpListenerResponse response)
        {
            if (!nodeList.ContainsKey(request.RemoteEndPoint.ToString()))
            {
                string newNodeName = "Node_" + nodeCtr++;
                ResponsePacket resPacket = new ResponsePacket() { DateTimeStamp = DateTime.Now, Code = RESPONSE_CODES.REGISTER_CLIENT_ACK, Node = newNodeName };
                _Common.WriteResPacketToResponse(resPacket,response);
            }
        }
    } //test
}
