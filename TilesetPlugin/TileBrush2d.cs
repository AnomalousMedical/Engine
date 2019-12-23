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
using Microsoft.Extensions.DependencyInjection;

namespace Anomalous.SidescrollerCore
{
    class TileBrush2d : BehaviorInterface
    {
        [Editable]
        public Vector3 Dimensions { get; set; } = new Vector3(1, 1, 1);

        [Editable]
        public String TilesetName { get; set; }

        [Editable]
        public String SideTileName { get; set; }

        [Editable]
        public String NodeName { get; set; } = "Node";

        [Editable]
        public String ManualObjectName { get; set; } = "ManualObject";

        [Editable]
        public String RigidBodyName { get; set; } = "RigidBody";

        [Editable]
        public Vector3 TileSize { get; set; } = new Vector3(1, 1, 1);

        private ManualObject manualObject;

        private ReshapeableRigidBody rigidBody;

        protected override void link()
        {
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
            return new TileBrush2d()
            {
                NodeName = NodeName,
                ManualObjectName = ManualObjectName,
                TileSize = TileSize,
                Dimensions = Dimensions,
                TilesetName = TilesetName,
                SideTileName = SideTileName,
            };
        }

        protected void drawTile()
        {
            var tileManager = Scope.ServiceProvider.GetRequiredService<TilesetManager>();
            if (tileManager == null)
            {
                blacklist("No tile manager service");
            }

            Tileset tileset;
            if (!tileManager.tryGetTileset(TilesetName, out tileset))
            {
                blacklist($"Cannot file tileset {TilesetName}");
            }

            manualObject.begin(tileset.Material, OperationType.OT_TRIANGLE_LIST);

            IntVector3 dimen = (IntVector3)Dimensions;
            var ts = TileSize; //Tile size
            var rs = Dimensions * TileSize; //Real size
            var off = rs / 2; //offsets
            var z = 0.0f;

            uint numCreatedSquares = 0;

            Tile mainTile;
            if (!tileset.tryGetTile(SideTileName, out mainTile))
            {
                blacklist($"Cannot find side tile {SideTileName} in Tileset {TilesetName}");
            }

            //This order gives x, y for indices, 3rd quadrant x goes left y goes down
            //Front
            #region Front
            for (var y = 0; y < dimen.y; ++y)
            {
                for (var x = 0; x < dimen.x; ++x)
                {
                    float vl = x * ts.x - off.x;
                    float vt = y * ts.y - off.y;
                    float vr = (x + 1) * ts.x - off.x;
                    float vb = (y + 1) * ts.y - off.y;

                    //lt
                    manualObject.position(vl, -vt, z);
                    manualObject.normal(0, 0, 1);
                    manualObject.binormal(0, 1, 0);
                    manualObject.tangent(1, 0, 0);
                    manualObject.textureCoord(mainTile.left, mainTile.top);
                    //rt
                    manualObject.position(vr, -vt, z);
                    manualObject.normal(0, 0, 1);
                    manualObject.binormal(0, 1, 0);
                    manualObject.tangent(1, 0, 0);
                    manualObject.textureCoord(mainTile.right, mainTile.top);
                    //lb
                    manualObject.position(vl, -vb, z);
                    manualObject.normal(0, 0, 1);
                    manualObject.binormal(0, 1, 0);
                    manualObject.tangent(1, 0, 0);
                    manualObject.textureCoord(mainTile.left, mainTile.bottom);
                    //rb
                    manualObject.position(vr, -vb, z);
                    manualObject.normal(0, 0, 1);
                    manualObject.binormal(0, 1, 0);
                    manualObject.tangent(1, 0, 0);
                    manualObject.textureCoord(mainTile.right, mainTile.bottom);

                    ++numCreatedSquares;
                }
            }
            #endregion

            for (uint i = 0; i < numCreatedSquares; ++i)
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
