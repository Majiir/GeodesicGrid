using System;
using System.IO;
using System.Reflection;

namespace GeodesicGrid
{
    internal static class LicenseSentinel
    {
        public static void Run()
        {
            var assembly = Assembly.GetExecutingAssembly();
            var path = assembly.Location;
            if (!Path.GetFileName(path).Equals("GeodesicGrid.dll", StringComparison.OrdinalIgnoreCase))
            {
                path += "-GeodesicGrid";
            }
            else
            {
                path = Path.ChangeExtension(path, null);
            }
            var text = new StreamReader(assembly.GetManifestResourceStream("GeodesicGrid.GeodesicGrid-LICENSE.txt")).ReadToEnd();
            File.WriteAllText(path + "-LICENSE.txt", text);
        }
    }
}
