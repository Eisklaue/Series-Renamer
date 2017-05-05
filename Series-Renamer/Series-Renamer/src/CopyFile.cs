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
                copyFile(showPercentProgress);
                cleanUp();
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


        private void copyFile(Action<string, long, long> reportProgress, int blockSizeToRead = 4096)
        {
            string srcPath = ep.fullPath;
            string dstPath = ep.folder + "\\" + ep.episode + ".mkv";

            if (!File.Exists(srcPath))
            {
                Console.WriteLine("{0} not found.. skipping", srcPath);
                return;
            }
            else
            {
                FileInfo srcFileInfoTmp = new FileInfo(srcPath);
                if (srcFileInfoTmp.Length < 10)
                {
                    Console.WriteLine("{0} File is empty (maybe unzip in progress", srcPath);
                    return;
                }

            }

            FileInfo srcFileInfo = new FileInfo(srcPath);
            string message = string.Format("Copy:\t\t");

            if (File.Exists(dstPath))
            {
                dstPath = ep.folder + "\\" + ep.episode + ep.id + ".mkv";
            }

            byte[] buffer = new byte[blockSizeToRead];
            using (var dstfs = File.OpenWrite(dstPath))
            {
                using (var srcfs = File.OpenRead(srcPath))
                {
                    int bytesRead, totalBytesRead = 0;
                    Console.WriteLine("Move file:\t" + srcPath + "\n" + "to:\t\t" + dstPath);
                    while ((bytesRead = srcfs.Read(buffer,0,buffer.Length - 1)) > 0)
                    {
                        dstfs.Write(buffer, 0, bytesRead);
                        totalBytesRead += bytesRead;
                        if (reportProgress != null)
                        {
                            reportProgress(message, totalBytesRead, srcFileInfo.Length);
                        }
                    }
                }
            }

        }

        private void moveFile()
        {
            string srcPath = ep.fullPath;
            string dstPath = ep.folder + "\\" + ep.episode + ".mkv";


            if (!File.Exists(dstPath))
            {
                if( new FileInfo(srcPath).Length == 0)
                {
                    Console.WriteLine("Cannot copy file:\t" + srcPath + "\tfile is empty (maybe due to unzip)");
                }
                else
                {
                    Console.WriteLine("Move file:\t" + srcPath + "\n" + "to:\t\t" + dstPath);

                    File.Move(srcPath, dstPath);
                }
               
            }
            else
            {
                if (new FileInfo(srcPath).Length == 0)
                {
                    Console.WriteLine("Cannot copy file:\t" + srcPath + "\tfile is empty (maybe due to unzip)");
                }
                else
                {
                    Console.WriteLine("Move file:\t" + srcPath + "\n" + "to:\t\t" + ep.folder + "\\" + ep.episode + ep.id + ".mkv");
                    File.Move(srcPath, ep.folder + "\\" + ep.episode + ep.id + ".mkv");
                }
            }
        }

        private void cleanUp()
        {
            string srcPath = ep.path;

            if(string.IsNullOrEmpty(Directory.GetFiles(srcPath).OfType<string>().ToList().FirstOrDefault(s => s.Contains(".mkv"))))
            {
                try
                {
                    Console.WriteLine("Delete folder:\t" + srcPath + "\n-------------------------------------------\n");
                    Directory.Delete(srcPath, true);
                }catch(Exception e)
                {
                    Console.WriteLine(e.Message);
                }
            }else
            {
                try
                {
                    {
                        Console.WriteLine("Delete file:\t {0} \n-------------------------------------------\n", ep.fullPath);
                        File.Delete(ep.fullPath);
                    }
                }catch(Exception e)
                {
                    Console.WriteLine(e.Message);
                }
            }
        }

        private void showPercentProgress (string message, long processed, long total)
        {
            double percent = processed*100.0/total;
            Console.Write("\r{0}{1}% complete\t\t {2}/{3}", message, Math.Round(percent,1), processed, total);
            if (processed >= total - 1)
            {
                Console.WriteLine(Environment.NewLine);
            }
        }
    }
}
