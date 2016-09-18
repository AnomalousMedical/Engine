using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TilesetPlugin
{
    class Tileset
    {
        [JsonProperty]
        private String name;
        [JsonProperty]
        private String material;
        [JsonProperty]
        private Dictionary<String, Tile> tiles = new Dictionary<string, Tile>();

        public Tileset()
        {

        }

        public Tileset(String name, String material)
        {

        }

        public String Name
        {
            get
            {
                return name;
            }
        }

        public String Material
        {
            get
            {
                return material;
            }
        }

        public Tile this[String key]
        {
            get
            {
                return tiles[key];
            }
        }
    }
}
