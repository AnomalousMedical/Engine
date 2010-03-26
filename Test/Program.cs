using System;
using System.Collections.Generic;
using System.Text;
using Engine;
using Logging;
using System.Windows.Forms;
using Editor;
using Engine.ObjectManagement;
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
            using (VirtualFileSystem fileSystem = new VirtualFileSystem())
            {
                fileSystem.addArchive("S:/vfstest\\RootDir1");
                fileSystem.addArchive("S:/vfstest/RootDir2");
                fileSystem.addArchive("S:/vfstest/ZipDir.zip");

                Console.WriteLine("--List of all files--");
                foreach (String file in fileSystem.listFiles(true))
                {
                    Console.WriteLine(file);
                }

                Console.WriteLine("\n--List of all directories--");
                foreach (String file in fileSystem.listDirectories(true))
                {
                    Console.WriteLine(file);
                }


                printFileContents("Folder3/DuplicateTextDoc.txt", fileSystem);
                printFileContents("Folder1/TextDoc1Folder1.txt", fileSystem);
                printFileContents("Folder1/TextDoc2Folder1.txt", fileSystem);
                printFileContents("Folder2/Folder2TextDoc1.txt", fileSystem);

                Console.ReadLine();
            }
        }

        private static void printFileContents(String fileName, VirtualFileSystem fileSystem)
        {
            using (Stream stream = fileSystem.openStream(fileName, Engine.Resources.FileMode.Open))
            {
                Console.WriteLine("--------------Contents of {0}--------------", fileName);
                TextReader textReader = new StreamReader(stream);
                String line = textReader.ReadLine();
                while (line != null)
                {
                    Console.WriteLine(line);
                    line = textReader.ReadLine();
                }
                Console.WriteLine("------------------------------------------");
            }
        }
    }
}
