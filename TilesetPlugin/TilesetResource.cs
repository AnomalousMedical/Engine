using Engine;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TilesetPlugin
{
    class TilesetResource
    {
        private List<Tileset> tilesets = new List<Tileset>();
        private String locName;
        private bool recursive;

        public TilesetResource(String locName, bool recursive)
        {
            this.locName = locName;
            this.recursive = recursive;
        }

        public void load(TilesetManager manager)
        {
            VirtualFileSystem vfs = VirtualFileSystem.Instance;
            foreach(var file in vfs.listFiles(locName, "*.tiles.json", recursive))
            {
                using(var reader = new StreamReader(vfs.openStream(file, Engine.Resources.FileMode.Open, Engine.Resources.FileAccess.Read)))
                {
                    var tileset = JsonConvert.DeserializeObject<Tileset>(reader.ReadToEnd());
                    manager.addTileset(tileset);
                    tilesets.Add(tileset);
                }
            }
        }

        public void unload(TilesetManager manager)
        {
            foreach(var tileset in tilesets)
            {
                manager.removeTileset(tileset);
            }
        }
    }
}
