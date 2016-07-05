using System;
using System.Configuration;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Series_Renamer.src;

namespace Series_Renamer
{
    class Program
    {
        static void Main(string[] args)
        {
            string folder = ConfigurationManager.AppSettings["JdownloadFolder"];
            int i = 0;
            List<Episode> files = new List<Episode>();
            try
            {
                foreach (string file in Directory.EnumerateFiles(folder, "*.mkv", SearchOption.AllDirectories))
                {
                    files.Add(new Episode(file, i));
                    i++;

                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

            try
            {
                foreach (Episode ep in files)
                {
                    CopyFile cp = new CopyFile(ep);
                    cp.createFile(false);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }


    }
}
