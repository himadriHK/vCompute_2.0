using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.Remoting;
using System.Security;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using CodeLoader;

namespace ProjectCore
{
	class SandboxExecute
	{
		public object executeAssembly(string assemblyName, Loader codeLoader,object param)
		{
			Evidence e = new Evidence();
			e.AddHostEvidence(new Zone(SecurityZone.Intranet));
			PermissionSet permSet = SecurityManager.GetStandardSandbox(e);

			AppDomainSetup ads = new AppDomainSetup();
			ads.ApplicationBase = AppDomain.CurrentDomain.BaseDirectory;
			AppDomain tempDomain = AppDomain.CreateDomain(assemblyName + new Random().NextDouble(), e, ads, permSet, null);

			tempDomain.Load("CodeLoader");

			ObjectHandle handle = Activator.CreateInstanceFrom(tempDomain, typeof(Sandboxer).Assembly.ManifestModule.FullyQualifiedName, typeof(Sandboxer).FullName);

			Sandboxer newDomainInstance = (Sandboxer)handle.Unwrap();

			byte[] assemblyBinary = codeLoader.codeDictionary.ReadAssembly(assemblyName);

			object output = newDomainInstance.ExecuteNewAssembly(assemblyBinary, param);

			return output;
		}
	}

	class Sandboxer : MarshalByRefObject
	{
		public object ExecuteNewAssembly(byte[] rawAssm, object param)
		{
			Assembly asm = Assembly.Load(rawAssm);
			CodeInterface assemblyObj = null;

			foreach (Type t in asm.GetTypes())
				if (t.GetInterface("CodeInterface") != null)
					assemblyObj = (CodeInterface)Activator.CreateInstance(t);

			object output = string.Empty;

			try
			{
				output = assemblyObj.DoWork(param);
			}
			catch (Exception ex)
			{
				output = ex.Message;
			}
			return output;
		}
	}
}
