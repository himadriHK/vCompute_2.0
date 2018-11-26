using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Net.Http;
using System.Security.Policy;
using System.Threading;
using System.Threading.Tasks;
using CodeLoader;
using static ProjectCore._Common;

namespace ProjectCore
{
	public class _Client
	{
		private string baseUrl = "http://localhost:8080/";
		public string NodeName = string.Empty;
		private CodeLoader.Loader codeLoader;
        private Queue<_Common.Task> taskQueue;
        private bool _availableForTasks;
        private TimeSpan _maxAwaitTime;
        public _Client(string baseUrl)
		{
            if(baseUrl != string.Empty)
			    this.baseUrl = baseUrl;
			codeLoader = new Loader(AppDomain.CurrentDomain.BaseDirectory + @"client.bin");
            taskQueue = new Queue<_Common.Task>();
            _availableForTasks = true;
            _maxAwaitTime = new TimeSpan(0, 0, 30);

            SyncAssemblies();

            Thread sendStatus = new Thread(SendStatus);
            sendStatus.Start();
            Thread processTasks = new Thread(ProcessTasks);
            processTasks.Start();
        }

		private async Task<ResponsePacket> SendRequesPacketAsync(RequestPacket request)
		{
			HttpClient client = new HttpClient();
			string data = _Common.ConvertData(request);
			StringContent content = new StringContent(data);
			HttpResponseMessage response = await client.PostAsync(baseUrl, content);
			string str = await response.Content.ReadAsStringAsync();
			ResponsePacket result = GetResponsePacket(str);
			return result;
		}

		public async Task<string> RegisterClient()
		{
			RequestPacket requestPacket =
				new RequestPacket() { DateTimeStamp = DateTime.Now, Code = REQUEST_CODES.REGISTER_CLIENT };

			ResponsePacket responsePacket = await SendRequesPacketAsync(requestPacket);

			NodeName = responsePacket.Node;
            return NodeName;
		}

		private async void SendStatus()
		{
            while (true)
            {
                double[] perfCounter = getPerfCounter();
                RequestPacket requestPacket =
                    new RequestPacket() { DateTimeStamp = DateTime.Now,Node=NodeName, Code = REQUEST_CODES.STATUS, Status = new STATUS() { CpuTime = perfCounter[0], RamUsage = perfCounter[1] } };

                await SendRequesPacketAsync(requestPacket);
                Thread.Sleep(2000);
            }
		}

		private async void SyncAssemblies()
		{
			HashSet<string> assemblyList= new HashSet<string>(codeLoader.codeDictionary.GetAssemblyList());
			RequestPacket requestPacket =
				new RequestPacket() { DateTimeStamp = DateTime.Now, Code = REQUEST_CODES.ASSEMBLY_TRACK, Result = new Result(){ResultData = assemblyList}};

			await SendRequesPacketAsync(requestPacket);
		}

        private async Task<bool> GetTasks()
        {
            RequestPacket requestPacket = new RequestPacket() { DateTimeStamp = DateTime.Now, Code = REQUEST_CODES.GET_TASK, Node = NodeName };
            ResponsePacket responsePacket = await SendRequesPacketAsync(requestPacket);
            if(!responsePacket.Errored)
            {
                lock (taskQueue)
                {
                    taskQueue.Enqueue(responsePacket.Task);
                }
                return true;
            }

            return false;
        }

        public async void ProcessTasks()
        {
            _Common.Task task;
            while (_availableForTasks)
            {
                await GetTasks();
                if (taskQueue.Count > 0)
                {
                    lock (taskQueue)
                    {
                        task = taskQueue.Dequeue();
                    }
                    codeLoader.codeDictionary.WriteAssembly(task.AssemblyDetails.AssemblyName, task.AssemblyDetails.AssemblyCode);
                    SandboxExecute sandboxExecute = new SandboxExecute();
                    object result = sandboxExecute.executeAssembly(task.AssemblyDetails.AssemblyName, codeLoader, task.Params);
                    RequestPacket requestPacket = new RequestPacket()
                    {
                        DateTimeStamp = DateTime.Now,
                        Code = REQUEST_CODES.SUBMIT_RESULT,
                        Node = NodeName,
                        Result = new Result()
                        {
                            ResultData = result,
                            TaskId = task.TaskId
                        }
                    };
                    await SendRequesPacketAsync(requestPacket);
                }

                Thread.Sleep(500);
            }
        }

        public async Task<object> GetResult(string taskId)
        {
            RequestPacket requestPacket = new RequestPacket()
            {
                DateTimeStamp = DateTime.Now,
                Node = NodeName,
                Code = REQUEST_CODES.GET_RESULT,
                Task = new _Common.Task() { TaskId = taskId }
            };

            ResponsePacket responsePacket;
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();

            do
            {
                responsePacket = await SendRequesPacketAsync(requestPacket);
            }
            while (responsePacket.Errored || stopwatch.Elapsed >= _maxAwaitTime);
            stopwatch.Stop();
            return responsePacket.Result.ResultData;
        }

        public async Task<string> SubmitTask(string assemblyName, byte[] assemblyBytes, object param)
        {
            if (assemblyBytes == null)
                assemblyBytes = codeLoader.codeDictionary.ReadAssembly(assemblyName);

            if (assemblyBytes == null)
                return string.Empty;
            else
                codeLoader.codeDictionary.WriteAssembly(assemblyName, assemblyBytes);

            RequestPacket request = new RequestPacket()
            {
                DateTimeStamp = DateTime.Now,
                Node = NodeName,
                Code = REQUEST_CODES.SUBMIT_TASK,
                Task = new _Common.Task() { AssemblyDetails = new Assembly() { AssemblyName = assemblyName, AssemblyCode = assemblyBytes, },Params=param,RequestingNode=NodeName }
            };

            ResponsePacket response = await SendRequesPacketAsync(request);
            string taskId = (response.Errored) ? string.Empty : response.Task.TaskId;
            return taskId;
        }

        public byte[] complieAssembly(string sourceFile,out string error)
        {

            var provider = CodeDomProvider.CreateProvider("CSharp");
            CompilerParameters cp = new CompilerParameters();
            cp.GenerateExecutable = false;
            cp.GenerateInMemory = false;
            cp.ReferencedAssemblies.Add("CodeInterface.dll");

            error = string.Empty;
            CompilerResults result = provider.CompileAssemblyFromSource(cp, sourceFile);
            if (result.Errors.HasErrors)
            {
                error = result.Errors[0].ErrorText;
                return null;
            }
            else
                return File.ReadAllBytes(result.PathToAssembly);
        }

        private double[] getPerfCounter()
		{

			PerformanceCounter cpuCounter = new PerformanceCounter();
			cpuCounter.CategoryName = "Processor Information";
			cpuCounter.CounterName = "% Processor Time";
			cpuCounter.InstanceName = "_Total";

			PerformanceCounter ramCounter = new PerformanceCounter("Memory", "% Committed Bytes In Use");
            
			ramCounter.NextValue();
            cpuCounter.NextValue();
			System.Threading.Thread.Sleep(500);

			double cpusecondValue = Math.Round(cpuCounter.NextValue(),2);
			double ramsecondValue = Math.Round(ramCounter.NextValue(),2);

			return new double[] { cpusecondValue, ramsecondValue };

		}

	}
}
