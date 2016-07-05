using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Series_Renamer.src
{
    class CopyFile
    {
        private Episode ep;

        public CopyFile(Episode ep)
        {
            this.ep = ep;
        }

        public void createFile(bool copy)
        {
            createFolder();
            if (copy)
            {
                copyFile();
            }
            else
            {
                moveFile();
                cleanUp();
            }

        }

        private void createFolder()
        {
            if (!Directory.Exists(ep.folder))
            {
                Console.WriteLine("Create Folder:\t" + ep.folder);
                Directory.CreateDirectory(ep.folder);
            }
        }


        private void copyFile()
        {
            if (!File.Exists(ep.folder + "\\" + ep.episode))
            {
                File.Create(ep.folder + "\\" + ep.episode);
            }
            else
            {
                File.Create(ep.folder + "\\" + ep.episode + ep.id);
            }
        }

        private void moveFile()
        {
            string srcPath = ep.fullPath;
            string dstPath = ep.folder + "\\" + ep.episode + ".mkv";


            if (!File.Exists(dstPath))
            {
                Console.WriteLine("Move file:\t" + srcPath + "\n" +"to:\t\t" + dstPath);

                File.Move(srcPath, dstPath);
            }
            else
            {
                Console.WriteLine("Move file:\t" + srcPath + "\n" + "to:\t\t" + ep.folder + "\\" + ep.episode + ep.id + ".mkv");
                File.Move(srcPath, ep.folder + "\\" + ep.episode + ep.id + ".mkv");
            }
        }

        private void cleanUp()
        {
            string srcPath = ep.path;

            if(string.IsNullOrEmpty(Directory.GetFiles(srcPath).OfType<string>().ToList().FirstOrDefault(s => s.Contains(".txt"))))
            {
                try
                {
                    Console.WriteLine("Delete folder:\t" + srcPath + "\n-------------------------------------------\n");
                    Directory.Delete(srcPath, true);
                }catch(Exception e)
                {
                    Console.WriteLine(e.Message);
                }
            }
        }
    }
}
