using System;
using System.Collections.Generic;
using System.Text;
using Engine;
using Logging;
using System.Windows.Forms;
using Editor;
using Engine.ObjectManagement;
using PhysXWrapper;
using System.Xml;
using Engine.Saving.XMLSaver;
using System.IO;
using Engine.Renderer;
using Engine.Platform;
using Engine.Resources;
using ZipAccess;

namespace Test
{
    class Program
    {
        [STAThread]
        static void Main(string[] args)
        {
            try
            {
                //ZipFile zipFile = new ZipFile())
                using (ZipFile zipFile = new ZipFile("TestZip.zip"))
                {
                    List<ZipFileInfo> files;
                    Console.WriteLine("Root folder recursive");
                    files = zipFile.listFiles("", true);
                    foreach (ZipFileInfo file in files)
                    {
                        Console.WriteLine(file.FullName);
                    }
                    Console.WriteLine("");

                    Console.WriteLine("Root folder nonrecursive");
                    files = zipFile.listFiles("", false);
                    foreach (ZipFileInfo file in files)
                    {
                        Console.WriteLine(file.FullName);
                    }
                    Console.WriteLine("");

                    Console.WriteLine("Folder folder recursive");
                    files = zipFile.listFiles("folder", true);
                    foreach (ZipFileInfo file in files)
                    {
                        Console.WriteLine(file.FullName);
                    }
                    Console.WriteLine("");

                    Console.WriteLine("Folder folder nonrecursive");
                    files = zipFile.listFiles("folder", false);
                    foreach (ZipFileInfo file in files)
                    {
                        Console.WriteLine(file.FullName);
                    }
                    Console.WriteLine("");

                    Console.WriteLine("Reading folder/file1.txt");
                    using (ZipStream stream = zipFile.openFile("folder/file1.txt"))
                    {
                        Console.WriteLine("Root folder recursive");
                        StreamReader reader = new StreamReader(stream);
                        while (!reader.EndOfStream)
                        {
                            Console.WriteLine(reader.ReadLine());
                        }
                        reader.Close();
                    }

                    Console.WriteLine("Attemping to read bad file got {0}.", zipFile.openFile("null"));
                }
                Console.ReadLine();
            }
            catch(ZipIOException)
            {

            }
        }
    }
}
