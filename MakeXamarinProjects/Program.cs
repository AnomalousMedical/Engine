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
        //Android and ios are supported with this, but they still work with shared projects,
        //so mac only for now, but if you get trouble later these should work (might have to tweak project file some)
        static void Main(string[] args)
        {
            var inFile = args[0];
            if (Directory.Exists(inFile))
            {
                foreach(var file in Directory.EnumerateFiles(inFile, "*" + Path.GetExtension(inFile), SearchOption.AllDirectories))
                {
                    convertToMac(file);
                    //convertToAndroid(file);
                    //convertToiOS(file);
                    convertToPortable(file);
                }
            }
            else
            {
                convertToMac(inFile);
                //convertToAndroid(inFile);
                //convertToiOS(inFile);
                convertToPortable(inFile);
            }
        }

        static void convertToAndroid(String file)
        {
            String output = Path.Combine(Path.GetDirectoryName(file), Path.GetFileNameWithoutExtension(file) + "Android" + Path.GetExtension(file));
            convertProject(file, output,
                "{EFBA0AD7-5A72-4C68-AF49-83D382785DCF};{FAE04EC0-301F-11D3-BF4B-00C04F79EFBC}",
                "$(MSBuildExtensionsPath)\\Xamarin\\Android\\Xamarin.Android.CSharp.targets",
                "6.0",
                new Tuple<String, String>[] { }
                );
        }

        static void convertToiOS(String file)
        {
            String output = Path.Combine(Path.GetDirectoryName(file), Path.GetFileNameWithoutExtension(file) + "iOS" + Path.GetExtension(file));
            convertProject(file, output,
                "{FEACFBD2-3405-455C-9665-78FE426C6842};{FAE04EC0-301F-11D3-BF4B-00C04F79EFBC}",
                "$(MSBuildExtensionsPath)\\Xamarin\\iOS\\Xamarin.iOS.CSharp.targets",
                "1.0",
                new Tuple<String, String>[] { }
                );
        }

        static void convertToMac(String file)
        {
            String output = Path.Combine(Path.GetDirectoryName(file), Path.GetFileNameWithoutExtension(file) + "Mac" + Path.GetExtension(file));
            convertProject(file, output,
                "{A3F8F2AB-B479-4A4A-A458-A89E7DC349F1};{FAE04EC0-301F-11D3-BF4B-00C04F79EFBC}",
                "$(MSBuildExtensionsPath)\\Xamarin\\Mac\\Xamarin.Mac.CSharp.targets",
                "2.0",
                new Tuple<String, String>[] {
                    Tuple.Create("TargetFrameworkIdentifier", "Xamarin.Mac")
                }
                );
        }

        static void convertToPortable(String file)
        {
            String output = Path.Combine(Path.GetDirectoryName(file), Path.GetFileNameWithoutExtension(file) + "Portable" + Path.GetExtension(file));
            convertProject(file, output,
                "{786C830F-07A1-408B-BD7F-6EE04809D6DB};{FAE04EC0-301F-11D3-BF4B-00C04F79EFBC}",
                "$(MSBuildExtensionsPath)\\Xamarin\\Mac\\Xamarin.Mac.CSharp.targets",
                "4.0",
                new Tuple<String, String>[] {
                    Tuple.Create("TargetFrameworkProfile", "Profile24")
                }
                );
        }

        static String VsXmlNamespace = "http://schemas.microsoft.com/developer/msbuild/2003";

        static void convertProject(String file, String output, String guids, String project, String targetFrameworkVersion, IEnumerable<Tuple<String, String>> additionalElements)
        {
            XDocument xDoc = XDocument.Load(file);
            var root = xDoc.Root;
            foreach (var propGroup in root.Elements(XName.Get("PropertyGroup", VsXmlNamespace)))
            {
                foreach (var item in propGroup.Elements(XName.Get("TargetFrameworkVersion", VsXmlNamespace)))
                {
                    item.SetValue(targetFrameworkVersion);
                    propGroup.Add(new XElement(XName.Get("ProjectTypeGuids", VsXmlNamespace), guids));
                    foreach(var additional in additionalElements)
                    {
                        propGroup.Add(new XElement(XName.Get(additional.Item1, VsXmlNamespace), additional.Item2));
                    }
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
