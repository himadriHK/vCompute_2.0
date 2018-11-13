using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace ProjectCore
{
    static class _Common
    {
        const string SUCCESS = "Success";
        const string FAILURE = "Failure";

        public enum REQUEST_CODES
        {
            STATUS,
            REGISTER_CLIENT,
            REGISTER_ASSEMBLY,
            UNREGISTER_CLIENT,
            UNREGISTER_ASSEMBLY,
            GET_RESULT,
            SUBMIT_TASK,
            SUBMIT_RESULT,
            GET_TASK
        };

        public enum RESPONSE_CODES
        {
            STATUS_ACK,
            REGISTER_CLIENT_ACK,
            REGISTER_ASSEMBLY_ACK,
            UNREGISTER_CLIENT_ACK,
            UNREGISTER_ASSEMBLY_ACK,
            GET_RESULT_ACK,
            SUBMIT_TASK_ACK,
            SUBMIT_RESULT_ACK,
            GET_TASK_ACK
        };

        public struct STATUS
        {
            public double CpuTime;
            public double RamUsage;
        }

        public enum CODE_LANG
        {
            CSharp,
            VisualBasic,
            JScript
        };

        public struct Task
        {
            public string TaskId;
            public string RequestingNode;
            public Assembly AssemblyDetails;
            public object Params;
        }

        public struct Assembly
        {
            public string AssemblyName;
            public byte[] AssemblyCode;
            public CODE_LANG CodeLang;
        }

        public struct RequestPacket
        {
            public DateTime DateTimeStamp;
            public string Node;
            public Assembly AssemblyDetails;
            public REQUEST_CODES Code;
            public Task Task;
            public Result Result;
            public STATUS Status;
        }

        public struct Result
        {
            public string TaskId;
            public object ResultData;
        }

        public struct ResponsePacket
        {
            public DateTime DateTimeStamp;
            public string Node;
            public RESPONSE_CODES Code;
            public bool Errored;
            public string Response;
            public Task Task;
            public Result Result;
        }

        //public ResponsePacket GenerateResponsePacket(string NodeName,RESPONSE_CODES resCode,bool isErrored, string response, )

        public static void WriteResPacketToResponse(ResponsePacket resPacket,HttpListenerResponse response)
        {
            string output = ConvertData(resPacket);
            response.OutputStream.Write(Encoding.ASCII.GetBytes(output), 0, output.Length);
            response.Close();
        }

        public static string ConvertData(RequestPacket reqPacket)
        {
            MemoryStream ms = new MemoryStream();
            StreamWriter streamWriter = new StreamWriter(ms);
            Newtonsoft.Json.JsonSerializer.Create().Serialize(streamWriter, reqPacket);
            streamWriter.Flush();
            ms.Seek(0, SeekOrigin.Begin);
            StreamReader streamReader = new StreamReader(ms);
            string result= streamReader.ReadToEnd();
            streamWriter.Close();
            streamReader.Close();
            return result;
        }

        public static string ConvertData(ResponsePacket resPacket)
        {
            MemoryStream ms = new MemoryStream();
            StreamWriter streamWriter = new StreamWriter(ms);
            Newtonsoft.Json.JsonSerializer.Create().Serialize(streamWriter, resPacket);
            streamWriter.Flush();
            ms.Seek(0, SeekOrigin.Begin);
            StreamReader streamReader = new StreamReader(ms);
            string result = streamReader.ReadToEnd();
            streamReader.Close();
            streamWriter.Close();
            return result;
        }

        public static RequestPacket GetRequestPacket(string reqPacket)
        {
            return (RequestPacket)Newtonsoft.Json.JsonSerializer.Create().Deserialize(new StringReader(reqPacket), typeof(RequestPacket));
        }

        public static ResponsePacket GetResponsePacket(string resPacket)
        {
            return (ResponsePacket)Newtonsoft.Json.JsonSerializer.Create().Deserialize(new StringReader(resPacket), typeof(RequestPacket));
        }
    }
}
