using BulletPlugin;
using Engine;
using Engine.Editing;
using OgrePlugin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Anomalous.TilesetPlugin;

namespace Anomalous.SidescrollerCore
{
    class TileBrush : BehaviorInterface
    {
        [Editable]
        public Vector3 Dimensions { get; set; }

        [Editable]
        public String TilesetName { get; set; }

        [Editable]
        public String TileName { get; set; }

        [Editable]
        public String NodeName { get; set; } = "Node";

        [Editable]
        public String ManualObjectName { get; set; } = "ManualObject";

        [Editable]
        public String RigidBodyName { get; set; } = "RigidBody";

        [Editable]
        public Vector3 TileSize { get; set; }

        private ManualObject manualObject;

        private ReshapeableRigidBody rigidBody;

        protected override void link()
        {
            var ts = getService<Anomalous.TilesetPlugin.TilesetInterface>();

            var node = Owner.getElement(NodeName) as SceneNodeElement;
            if (node == null)
            {
                blacklist($"Cannot find SceneNodeElement {NodeName}");
            }

            manualObject = node.getNodeObject(ManualObjectName) as ManualObject;
            if (manualObject == null)
            {
                blacklist($"Cannot find ManualObject {ManualObjectName} on SceneNodeElement {NodeName}");
            }

            drawTile();

            rigidBody = Owner.getElement(RigidBodyName) as ReshapeableRigidBody;
            if(rigidBody != null)
            {
                rigidBody.beginUpdates();
                var section = rigidBody.createSection("Tile", Vector3.Zero, Quaternion.Identity, Vector3.ScaleIdentity);
                rigidBody.finishUpdates();
                rigidBody.setLocalScaling(Dimensions * TileSize);
            }

            base.link();
        }

        protected override Behavior createStaticStandIn()
        {
            return new TileBrush()
            {
                NodeName = NodeName,
                ManualObjectName = ManualObjectName,
                TileSize = TileSize,
                Dimensions = Dimensions,
                TilesetName = TilesetName,
                TileName = TileName
            };
        }

        protected void drawTile()
        {
            var tileManager = getService<TilesetManager>();
            if (tileManager == null)
            {
                blacklist("No tile manager service");
            }

            Tileset tileset;
            if (!tileManager.tryGetTileset(TilesetName, out tileset))
            {
                blacklist($"Cannot file tileset {TilesetName}");
            }

            Tile tile;
            if (!tileset.tryGetTile(TileName, out tile))
            {
                blacklist($"Cannot find tile {TileName} in Tileset {TilesetName}");
            }

            manualObject.begin(tileset.Material, OperationType.OT_TRIANGLE_LIST);

            IntVector3 dimen = (IntVector3)Dimensions;
            var ts = TileSize;

            var vOffset = (Dimensions * TileSize) / 2;

            //This order gives x, y for indices, 3rd quadrant x goes left y goes down
            for (var y = 0; y < dimen.y; ++y)
            {
                for (var x = 0; x < dimen.x; ++x)
                {
                    float vl = x * ts.x - vOffset.x;
                    float vt = y * ts.y - vOffset.y;
                    float vr = (x + 1) * ts.x - vOffset.x;
                    float vb = (y + 1) * ts.y - vOffset.y;

                    //lt
                    manualObject.position(vl, -vt, 0);
                    manualObject.normal(0, 0, 1);
                    manualObject.binormal(0, 1, 0);
                    manualObject.tangent(1, 0, 0);
                    manualObject.textureCoord(tile.left, tile.top);
                    //rt
                    manualObject.position(vr, -vt, 0);
                    manualObject.normal(0, 0, 1);
                    manualObject.binormal(0, 1, 0);
                    manualObject.tangent(1, 0, 0);
                    manualObject.textureCoord(tile.right, tile.top);
                    //lb
                    manualObject.position(vl, -vb, 0);
                    manualObject.normal(0, 0, 1);
                    manualObject.binormal(0, 1, 0);
                    manualObject.tangent(1, 0, 0);
                    manualObject.textureCoord(tile.left, tile.bottom);
                    //rb
                    manualObject.position(vr, -vb, 0);
                    manualObject.normal(0, 0, 1);
                    manualObject.binormal(0, 1, 0);
                    manualObject.tangent(1, 0, 0);
                    manualObject.textureCoord(tile.right, tile.bottom);
                }
            }

            uint numSquares = (uint)(dimen.x * dimen.y);

            for (uint i = 0; i < numSquares; ++i)
            {
                var square = i * 4;
                var lt = square;
                var rt = square + 1;
                var lb = square + 2;
                var rb = square + 3;
                //ccw
                manualObject.index(lt);
                manualObject.index(lb);
                manualObject.index(rb);

                manualObject.index(rb);
                manualObject.index(rt);
                manualObject.index(lt);
            }

            manualObject.end();
        }
    }
}
