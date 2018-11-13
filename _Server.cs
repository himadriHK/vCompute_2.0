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
        private Dictionary<string, _Common.Task> taskStore;
	     private Dictionary<string, double> nodeStatus;
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
                case REQUEST_CODES.SUBMIT_TASK:
                    SubmitTask(requestPacket, listCtxt.Response);
                    break;
                case REQUEST_CODES.GET_RESULT:
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
	        bool errored = false;
	        if (nodeList.ContainsValue(requestPacket.Node))
	        {
		        if (nodeStatus.ContainsKey(requestPacket.Node))
		        {
			        nodeStatus[requestPacket.Node] += ((requestPacket.Status.CpuTime + requestPacket.Status.RamUsage) / 2);
		        }
		        else
		        {
			        nodeStatus.Add(requestPacket.Node, ((requestPacket.Status.CpuTime + requestPacket.Status.RamUsage) / 2));
		        }
	        }
	        else
	        {
		        errored = true;
	        }

	        ResponsePacket resPacket = new ResponsePacket()
	        {
		        DateTimeStamp = DateTime.Now,
		        Code = RESPONSE_CODES.STATUS_ACK,
		        Errored = errored
	        };
	        _Common.WriteResPacketToResponse(resPacket, response);
		}

        private void SendResult(RequestPacket requestPacket, HttpListenerResponse response)
        {
	        bool errored = false;
	        object result = null;
	        if (resultStore.ContainsKey(requestPacket.Task.TaskId))
	        {
		        result = resultStore[requestPacket.Task.TaskId];
	        }
	        else
	        {
		        errored = true;
	        }
	        ResponsePacket resPacket = new ResponsePacket()
	        {
		        DateTimeStamp = DateTime.Now,
		        Code = RESPONSE_CODES.GET_RESULT_ACK,
		        Errored = errored,
				  Result = new Result() {ResultData = result,TaskId = requestPacket.Task.TaskId }
	        };
	        _Common.WriteResPacketToResponse(resPacket, response);
		}

        private void SubmitTask(RequestPacket requestPacket, HttpListenerResponse response)
        {
	        bool errored = false;
	        if (nodeList.ContainsKey(requestPacket.Node))
	        {
		        taskStore.Add(requestPacket.Node,requestPacket.Task);
	        }
	        else
	        {
		        errored = true;
	        }
	        ResponsePacket resPacket = new ResponsePacket()
	        {
		        DateTimeStamp = DateTime.Now,
		        Code = RESPONSE_CODES.SUBMIT_TASK_ACK,
		        Errored = errored
	        };
	        _Common.WriteResPacketToResponse(resPacket, response);
		}

        private void RegisterAssembly(RequestPacket requestPacket, HttpListenerResponse response)
        {
	        bool errored = !codeLoader.codeDictionary.WriteAssembly(requestPacket.AssemblyDetails.AssemblyName,
		        requestPacket.AssemblyDetails.AssemblyCode);

		        ResponsePacket resPacket = new ResponsePacket()
		        {
			        DateTimeStamp = DateTime.Now,
			        Code = RESPONSE_CODES.REGISTER_ASSEMBLY_ACK,
			        Errored = errored
		        };
			_Common.WriteResPacketToResponse(resPacket, response);
		}

        private void RegisterClient(HttpListenerRequest request, HttpListenerResponse response)
        {
	        ResponsePacket resPacket;

	        if (!nodeList.ContainsKey(request.RemoteEndPoint.ToString()))
	        {
		        string newNodeName = "Node_" + nodeCtr++;
		        resPacket = new ResponsePacket()
		        {
			        DateTimeStamp = DateTime.Now,
			        Code = RESPONSE_CODES.REGISTER_CLIENT_ACK,
			        Node = newNodeName
		        };
	        }
	        else
	        {
		        resPacket = new ResponsePacket()
		        {
			        DateTimeStamp = DateTime.Now,
			        Code = RESPONSE_CODES.REGISTER_CLIENT_ACK,
			        Errored = true
		        };
	        }
	        _Common.WriteResPacketToResponse(resPacket, response);
			}
        }
    }
}
