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
            using (ZipFile zipFile = new ZipFile())
            {
                zipFile.open("Testinzipstuff.zip");
                ZipStream stream = zipFile.openFile("folder/file1.txt");
                StreamReader reader = new StreamReader(stream);
                while (!reader.EndOfStream)
                {
                    Console.WriteLine(reader.ReadLine());
                }
                reader.Close();
                zipFile.close();
            }
            Console.ReadLine();
        }
    }
}
