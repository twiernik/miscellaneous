using System.IO;
using System.Reflection;

namespace UPackage
{
	public static class EmbadedResourceHelper
	{
		public static Stream GetResouceStream(Assembly assembly, string resourceName)
		{
			return assembly.GetManifestResourceStream(resourceName);
		}

		public static byte[] GetResourceBytes(Assembly assembly, string resouceName)
		{
			using (var stream = GetResouceStream(assembly, resouceName))
				return ReadFully(stream);
		}

		/// <summary>
		/// Thanks John!
		/// </summary>
		/// <param name="input"></param>
		/// <returns></returns>
		public static byte[] ReadFully(Stream input)
		{
			var buffer = new byte[16 * 1024];
			using (var ms = new MemoryStream())
			{
				int read;

				while ((read = input.Read(buffer, 0, buffer.Length)) > 0)
					ms.Write(buffer, 0, read);

				return ms.ToArray();
			}
		}

	}
}
