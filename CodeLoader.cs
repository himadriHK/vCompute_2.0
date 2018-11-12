using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;

namespace CodeLoader
{
	public class Loader
	{
		public CodeFileSystem codeDictionary { get; set; }
		string codeFilePath;
		FileStream fs;
		public Loader(string path)
		{
			codeFilePath = path;
			reloadAssemblies();
		}

		public void reloadAssemblies()
		{
			IFormatter binaryFormatter = new BinaryFormatter();
			if(File.Exists(codeFilePath))
				fs = new FileStream(codeFilePath, FileMode.Open,FileAccess.Read);
			else
				fs = new FileStream(codeFilePath, FileMode.Create, FileAccess.Write);

			codeDictionary = fs.Length == 0 ? new CodeFileSystem() : (CodeFileSystem)binaryFormatter.Deserialize(fs);
            fs.Close();
		}

		public void saveCodeDictionary()
		{
			IFormatter binaryFormatter = new BinaryFormatter();
			fs = new FileStream(codeFilePath, FileMode.Create, FileAccess.Write);
			binaryFormatter.Serialize(fs, codeDictionary);
			fs.Close();
		}
	}
}
