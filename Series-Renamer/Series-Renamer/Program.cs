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
        static int Main(string[] args)
        {
            if ( args.Length != 1)
            {
                Console.WriteLine("Usage:\tSeries-Renamer.exe <file extension>\n\nExample:\tSeries-Renamer.exe mkv");
                return 1;
            }

            string fileExtension = "." + args[0];
            string folder = ConfigurationManager.AppSettings["JdownloadFolder"];
            int i = 0;
            List<Episode> files = new List<Episode>();
            try
            {
                foreach (string file in Directory.EnumerateFiles(folder, "*" + fileExtension, SearchOption.AllDirectories))
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
                    cp.createFile(true, fileExtension);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

            return 0;
        }


    }
}
