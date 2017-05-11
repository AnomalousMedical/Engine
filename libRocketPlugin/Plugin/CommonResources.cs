using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;

namespace libRocketPlugin
{
    static class CommonResources
    {
        private const String NO_IMAGE = "libRocket.CommonResources.NoImage.png";
        private const String IMAGE_NOT_FOUND = "libRocket.CommonResources.ImageNotFound.png";

        private static Dictionary<String, String> resourceMap = new Dictionary<string, string>();

        static CommonResources()
        {
            resourceMap.Add(NO_IMAGE, "libRocketPlugin.Resources.NoImage.png");
            resourceMap.Add(IMAGE_NOT_FOUND, "libRocketPlugin.Resources.NoImageFound.png");
        }

        public static bool Exists(String resourceName)
        {
            return resourceMap.ContainsKey(resourceName);
        }

        public static Stream Open(String resourceName)
        {
            String assemblyResource;
            if (resourceMap.TryGetValue(resourceName, out assemblyResource))
            {
                return typeof(CommonResources).GetTypeInfo().Assembly.GetManifestResourceStream(assemblyResource);
            }
            return null;
        }
    }
}
