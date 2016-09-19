using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Anomalous.TilesetPlugin
{
    public class Tile
    {
        [JsonProperty]
        public float left;
        [JsonProperty]
        public float top;
        [JsonProperty]
        public float right;
        [JsonProperty]
        public float bottom;
    }
}
