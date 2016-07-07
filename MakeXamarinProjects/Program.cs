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
            String output = Path.GetFileNameWithoutExtension(file) + "Android" + Path.GetExtension(file);
            convertProject(file, output,
                "<ProjectTypeGuids>{EFBA0AD7-5A72-4C68-AF49-83D382785DCF};{FAE04EC0-301F-11D3-BF4B-00C04F79EFBC}</ProjectTypeGuids>",
                "$(MSBuildExtensionsPath)\\Xamarin\\Android\\Xamarin.Android.CSharp.targets",
                "6.0"
                );
        }

        static void convertToiOS(String file)
        {
            String output = Path.GetFileNameWithoutExtension(file) + "iOS" + Path.GetExtension(file);
            convertProject(file, output,
                "<ProjectTypeGuids>{FEACFBD2-3405-455C-9665-78FE426C6842};{FAE04EC0-301F-11D3-BF4B-00C04F79EFBC}</ProjectTypeGuids>",
                "$(MSBuildExtensionsPath)\\Xamarin\\iOS\\Xamarin.iOS.CSharp.targets",
                "1.0"
                );
        }

        static void convertToMac(String file)
        {
            String output = Path.GetFileNameWithoutExtension(file) + "Mac" + Path.GetExtension(file);
            convertProject(file, output,
                "<ProjectTypeGuids>{A3F8F2AB-B479-4A4A-A458-A89E7DC349F1};{FAE04EC0-301F-11D3-BF4B-00C04F79EFBC}</ProjectTypeGuids>",
                "$(MSBuildExtensionsPath)\\Xamarin\\Mac\\Xamarin.Mac.CSharp.targets",
                "1.0"
                );
        }

        static void convertProject(String file, String output, String guids, String import, String targetFrameworkVersion)
        {
            XElement xelement = XElement.Load(file);
            foreach (var propGroupOrImport in xelement.Elements())
            {
                if (propGroupOrImport.Name == "PropertyGroup")
                {
                    foreach (var item in propGroupOrImport.Elements())
                    {
                        if (item.Name == "TargetFrameworkVersion")
                        {
                            item.Value = targetFrameworkVersion;
                            propGroupOrImport.Add(guids);
                        }
                    }
                }
                else if (propGroupOrImport.Name == "Import")
                {
                    var name = propGroupOrImport.Attribute(XName.Get("Project"));
                    name.Value = import;
                }
            }
            xelement.Save(output);
        }
    }
}
