using Engine.Saving;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Anomalous.TextureCompiler
{
    class CompiledTextureInfo : Saveable
    {
        private Dictionary<String, byte[]> hashes = new Dictionary<string, byte[]>();

        public CompiledTextureInfo()
        {

        }

        public bool isChanged(String key, String path)
        {
            byte[] newHash = computeHash(path);
            byte[] oldHash;
            if(hashes.TryGetValue(key, out oldHash))
            {
                bool match = newHash.Length == oldHash.Length;
                for (int i = 0; match && i < oldHash.Length; ++i)
                {
                    match = newHash[i] == oldHash[i];
                }
                if(!match)
                {
                    //update hash
                    hashes[key] = newHash;
                }
                return !match;
            }
            else
            {
                //No hash yet
                hashes.Add(key, newHash);
                return true;
            }
        }

        private byte[] computeHash(String path)
        {
            using (var algo = SHA1.Create())
            {
                using (var stream = File.OpenRead(path))
                {
                    return algo.ComputeHash(stream);
                }
            }
        }

        protected CompiledTextureInfo(LoadInfo info)
        {
            info.RebuildDictionary("hash", hashes);
        }

        public void getInfo(SaveInfo info)
        {
            info.ExtractDictionary("hash", hashes);
        }
    }
}
