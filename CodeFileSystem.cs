using System;
using System.Collections.Generic;
using System.Linq;

namespace CodeLoader
{
    [Serializable]
	public class CodeFileSystem
	{
		private Dictionary<string, byte[]> codeDictionary;

		public CodeFileSystem()
		{
			codeDictionary = new Dictionary<string, byte[]>();
		}

        public bool WriteAssembly(string assemblyName, byte[] codeBytes)
        {
            if (!codeDictionary.ContainsKey(assemblyName))
            {
                codeDictionary.Add(assemblyName, codeBytes);
                return true;
            }

            return false;
        }

		public byte[] ReadAssembly(string assemblyName)
		{
            if (codeDictionary.ContainsKey(assemblyName))
                return codeDictionary[assemblyName];
            else
                return null;
		}

        public string[] GetAssemblyList()
		{
			return codeDictionary.Keys.ToArray<string>();
		}
	}
}
