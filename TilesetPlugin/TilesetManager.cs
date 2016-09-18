using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TilesetPlugin
{
    public class TilesetManager
    {
        private Dictionary<String, Tileset> tilesets = new Dictionary<string, Tileset>();

        internal void addTileset(Tileset tileset)
        {
            tilesets[tileset.Name] = tileset;
        }

        internal void removeTileset(Tileset tileset)
        {
            tilesets.Remove(tileset.Name);
        }

        public Tileset getTileset(String name)
        {
            Tileset tileset;
            tilesets.TryGetValue(name, out tileset);
            return tileset;
        }

        public bool tryGetTileset(String name, out Tileset tileset)
        {
            return tilesets.TryGetValue(name, out tileset);
        }
    }
}
