using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TilesetPlugin
{
    class Tile
    {
        [JsonProperty]
        private float left;
        [JsonProperty]
        private float top;
        [JsonProperty]
        private float right;
        [JsonProperty]
        private float bottom;


    }
}
