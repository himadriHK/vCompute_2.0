using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Http;
using System.Security.Policy;
using System.Threading.Tasks;
using CodeLoader;
using static ProjectCore._Common;

namespace ProjectCore
{
	class _Client
	{
		private string baseUrl = "http://localhost:8080/";
		private string NodeName = string.Empty;
		private CodeLoader.Loader codeLoader;
		public _Client(string baseUrl)
		{
			this.baseUrl = baseUrl;
			codeLoader = new Loader(AppDomain.CurrentDomain.BaseDirectory + @"client.bin");
		}

		private async Task<ResponsePacket> SendRequesPacketAsync(RequestPacket request)
		{
			HttpClient client = new HttpClient();
			string data = _Common.ConvertData(request);
			StringContent content = new StringContent(data);
			HttpResponseMessage response = await client.PostAsync("http://localhost:8080/", content);
			string str = await response.Content.ReadAsStringAsync();
			ResponsePacket result = GetResponsePacket(str);
			return result;
		}

		public async void RegisterClient()
		{
			RequestPacket requestPacket =
				new RequestPacket() { DateTimeStamp = DateTime.Now, Code = REQUEST_CODES.REGISTER_CLIENT };

			ResponsePacket responsePacket = await SendRequesPacketAsync(requestPacket);

			NodeName = responsePacket.Node;
		}

		private async void SendStatus()
		{
			float[] perfCounter = getPerfCounter();
			RequestPacket requestPacket =
				new RequestPacket() { DateTimeStamp = DateTime.Now, Code = REQUEST_CODES.STATUS,Status = new STATUS(){CpuTime = perfCounter[0], RamUsage = perfCounter[1] } };

			await SendRequesPacketAsync(requestPacket);
		}

		private async void SyncAssemblies()
		{
			HashSet<string> assemblyList= new HashSet<string>(codeLoader.codeDictionary.GetAssemblyList());
			RequestPacket requestPacket =
				new RequestPacket() { DateTimeStamp = DateTime.Now, Code = REQUEST_CODES.ASSEMBLY_TRACK, Result = new Result(){ResultData = assemblyList}};

			await SendRequesPacketAsync(requestPacket);
		}

		public async bool SubmitTask(string assemblyName, byte[] assemblyBytes, object param)
		{
			if(assemblyBytes == null)
			assemblyBytes = codeLoader.codeDictionary.ReadAssembly(assemblyName);

			if (assemblyBytes == null)
				return false;


		}
		private float[] getPerfCounter()
		{

			PerformanceCounter cpuCounter = new PerformanceCounter();
			cpuCounter.CategoryName = "Processor Information";
			cpuCounter.CounterName = "% Processor Time";
			cpuCounter.InstanceName = "_Total";

			PerformanceCounter ramCounter = new PerformanceCounter("Memory", "% Committed Bytes In Use");


			dynamic ramfirstValue = ramCounter.NextValue();


			dynamic cpufirstValue = cpuCounter.NextValue();
			System.Threading.Thread.Sleep(50);

			dynamic cpusecondValue = cpuCounter.NextValue();
			dynamic ramsecondValue = ramCounter.NextValue();

			return new float[] { cpusecondValue, ramsecondValue };

		}

	}
}
