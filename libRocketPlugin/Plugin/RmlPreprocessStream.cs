using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace libRocketPlugin
{
    public class RmlPreprocessStream : ManagedFileInterface
    {
        static RmlPreprocessStream()
        {
            ScaleFactor = 1.0f;
        }

        private VirtualFileSystemFileInterface virtualFileInterface;

        public RmlPreprocessStream()
        {
            virtualFileInterface = new VirtualFileSystemFileInterface();
        }

        public override Stream Open(string path)
        {
            Stream realStream = virtualFileInterface.Open(path);
            Stream returnStream;
            switch(Path.GetExtension(path).ToLowerInvariant())
            {
                case ".rcss":
                    returnStream = getAdjustedStream(realStream);
                    break;
                case ".rml":
                    returnStream = getAdjustedStream(realStream);
                    break;
                case ".trml":
                    returnStream = getAdjustedStream(realStream);
                    break;
                default:
                    returnStream = realStream;
                    break;
            }
            return returnStream;
        }

        private const char Colin = ':';
        private const String Pf = "pf;";
        private const String PfReplace = ":{0}px;";

        private static Stream getAdjustedStream(Stream realStream)
        {
            String realDocument;
            using (StreamReader streamReader = new StreamReader(realStream))
            {
                realDocument = streamReader.ReadToEnd();
            }
            StringBuilder sb = getAdjustedString(realDocument);
            MemoryStream ms = new MemoryStream(Encoding.UTF8.GetBytes(sb.ToString()));
            return ms;
        }

        public static StringBuilder getAdjustedString(String realDocument)
        {
            StringBuilder sb = new StringBuilder(realDocument.Length);
            int i = 0;
            int colin;
            colin = realDocument.IndexOf(Colin, i);
            if (colin == -1)
            {
                //Give up
                sb.Append(realDocument);
            }
            else
            {
                for (; i < realDocument.Length; )
                {
                    colin = realDocument.IndexOf(Colin, i);
                    if (colin == -1)
                    {
                        //Give up
                        sb.Append(realDocument.Substring(i));
                        i = realDocument.Length;
                    }
                    else
                    {
                        sb.Append(realDocument.Substring(i, colin - i));
                        i = colin + 1;
                        switch (findCloser(realDocument, ref i))
                        {
                            case CloserType.Eof:
                                //Give up
                                sb.Append(realDocument.Substring(colin));
                                break;
                            case CloserType.Px:
                                int num;
                                if (int.TryParse(realDocument.Substring(colin + 1, i - colin - Pf.Length), out num))
                                {
                                    sb.AppendFormat(PfReplace, num * ScaleFactor);
                                }
                                else
                                {
                                    sb.Append(realDocument.Substring(colin, i - colin));
                                }
                                ++i;
                                break;
                            case CloserType.Colin:
                                sb.Append(realDocument.Substring(colin, i - colin));
                                break;
                        }
                    }
                }
            }
            return sb;
        }

        enum CloserType
        {
            Colin,
            Px,
            Eof
        }

        private static CloserType findCloser(String realString, ref int i)
        {
            int pxMatch = 0;
            for (; i < realString.Length; ++i)
            {
                if (realString[i] == Colin)
                {
                    return CloserType.Colin;
                }
                if (realString[i] == Pf[pxMatch])
                {
                    ++pxMatch;
                    if(pxMatch == Pf.Length)
                    {
                        return CloserType.Px;
                    }
                }
                else
                {
                    pxMatch = 0;
                }
            }
            return CloserType.Eof;
        }

        public override void addExtension(RocketFileSystemExtension extension)
        {
            virtualFileInterface.addExtension(extension);
        }

        public override void removeExtension(RocketFileSystemExtension extension)
        {
            virtualFileInterface.removeExtension(extension);
        }

        public static float ScaleFactor { get; set; }
    }
}
