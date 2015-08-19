using NuGet;
using System.IO;
using System.Linq;
using System.Xml.Linq;
using System;
using System.Reflection;

namespace UPackage
{
	class Program
	{
		static void Main(string[] args)
		{
			AppDomain.CurrentDomain.AssemblyResolve += CurrentDomain_AssemblyResolve;

			new Program().Run(args);
		}

		private void Run(string[] args)
		{
			var items = GetDirectory(args)
				.GetDirectories()
				.SelectMany(d => d.GetFiles("*nupkg"))
				.Select(file => new ZipPackage(file.FullName))
				.Select(zp => new { zp.Id, zp.Version })
				.Select(x => new XElement("package",
					new XAttribute("id", x.Id),
					new XAttribute("version", x.Version)))
				.ToList();

			new XDocument(new XElement("packages", items)).Save(Path.Combine(GetDirectory(args).FullName, "_packages.config"));
		}

		private static Assembly CurrentDomain_AssemblyResolve(object sender, ResolveEventArgs args)
		{
			if (args.Name.StartsWith("NuGet.Core"))
			{
				return Assembly.Load(EmbadedResourceHelper.GetResourceBytes(Assembly.GetExecutingAssembly(), "UPackage.libs.NuGet.Core.dll"));
			}
			else if (args.Name.StartsWith("Microsoft.Web.XmlTransform"))
			{
				return Assembly.Load(EmbadedResourceHelper.GetResourceBytes(Assembly.GetExecutingAssembly(), "UPackage.libs.Microsoft.Web.XmlTransform.dll"));
			}
			return null;
		}

		private DirectoryInfo GetDirectory(string[] args)
		{
			if (args.Length != 0)
				return new DirectoryInfo(args[0]);
			else
				return new DirectoryInfo(".");
		}
	}
}
