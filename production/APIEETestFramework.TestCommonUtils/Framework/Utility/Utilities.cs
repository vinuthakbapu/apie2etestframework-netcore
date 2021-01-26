namespace TestCommonUtils
{
    using System;
    using System.IO;
    using System.Reflection;

    public class Utilities
    {
        public static string AppendPathToBinDirectory(string appendedPath)
        {
            Assembly ass = Assembly.GetExecutingAssembly();
            string path = Path.GetDirectoryName(ass.Location);

            if (path != null)
                return Path.GetFullPath(Path.Combine(path, appendedPath));

            throw new Exception("Bin directory path was null");
        }

        public static string StringToStrArray(string element, int index)
        {
            string[] ar = element.Split(' ');

            string k = ar[index];

            return k;
        }
    }
}