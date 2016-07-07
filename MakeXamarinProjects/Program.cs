using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace MakeXamarinProjects
{
    class Program
    {
        static void Main(string[] args)
        {
            convertToMac(args[0]);
            convertToAndroid(args[0]);
            convertToiOS(args[0]);
        }

        static void convertToAndroid(String file)
        {
            String output = Path.Combine(Path.GetDirectoryName(file), Path.GetFileNameWithoutExtension(file) + "Android" + Path.GetExtension(file));
            convertProject(file, output,
                "{EFBA0AD7-5A72-4C68-AF49-83D382785DCF};{FAE04EC0-301F-11D3-BF4B-00C04F79EFBC}",
                "$(MSBuildExtensionsPath)\\Xamarin\\Android\\Xamarin.Android.CSharp.targets",
                "6.0"
                );
        }

        static void convertToiOS(String file)
        {
            String output = Path.Combine(Path.GetDirectoryName(file), Path.GetFileNameWithoutExtension(file) + "iOS" + Path.GetExtension(file));
            convertProject(file, output,
                "{FEACFBD2-3405-455C-9665-78FE426C6842};{FAE04EC0-301F-11D3-BF4B-00C04F79EFBC}",
                "$(MSBuildExtensionsPath)\\Xamarin\\iOS\\Xamarin.iOS.CSharp.targets",
                "1.0"
                );
        }

        static void convertToMac(String file)
        {
            String output = Path.Combine(Path.GetDirectoryName(file), Path.GetFileNameWithoutExtension(file) + "Mac" + Path.GetExtension(file));
            convertProject(file, output,
                "{A3F8F2AB-B479-4A4A-A458-A89E7DC349F1};{FAE04EC0-301F-11D3-BF4B-00C04F79EFBC}",
                "$(MSBuildExtensionsPath)\\Xamarin\\Mac\\Xamarin.Mac.CSharp.targets",
                "1.0"
                );
        }

        static String VsXmlNamespace = "http://schemas.microsoft.com/developer/msbuild/2003";

        static void convertProject(String file, String output, String guids, String project, String targetFrameworkVersion)
        {
            XDocument xDoc = XDocument.Load(file);
            var root = xDoc.Root;
            foreach (var propGroup in root.Elements(XName.Get("PropertyGroup", VsXmlNamespace)))
            {
                foreach (var item in propGroup.Elements(XName.Get("TargetFrameworkVersion", VsXmlNamespace)))
                {
                    item.SetValue(targetFrameworkVersion);
                    propGroup.Add(new XElement(XName.Get("ProjectTypeGuids", VsXmlNamespace), guids));
                    break;
                }
            }
            foreach (var import in root.Elements(XName.Get("Import", VsXmlNamespace)))
            {
                var name = import.Attribute(XName.Get("Project"));
                name.SetValue(project);
            }
            xDoc.Save(output);
        }
    }
}
