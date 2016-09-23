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
        public String SideTileName { get; set; }

        [Editable]
        public String TopTileName { get; set; }

        [Editable]
        public String BottomTileName { get; set; }

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
                SideTileName = SideTileName,
                TopTileName = TopTileName,
                BottomTileName = BottomTileName
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

            manualObject.begin(tileset.Material, OperationType.OT_TRIANGLE_LIST);

            IntVector3 dimen = (IntVector3)Dimensions;
            var ts = TileSize; //Tile size
            var rs = Dimensions * TileSize; //Real size
            var off = rs / 2; //offsets
            var ss = rs - off; //Static sides, side values for the one that does not need to be computed

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
                    manualObject.position(vl, -vt, ss.z);
                    manualObject.normal(0, 0, 1);
                    manualObject.binormal(0, 1, 0);
                    manualObject.tangent(1, 0, 0);
                    manualObject.textureCoord(mainTile.left, mainTile.top);
                    //rt
                    manualObject.position(vr, -vt, ss.z);
                    manualObject.normal(0, 0, 1);
                    manualObject.binormal(0, 1, 0);
                    manualObject.tangent(1, 0, 0);
                    manualObject.textureCoord(mainTile.right, mainTile.top);
                    //lb
                    manualObject.position(vl, -vb, ss.z);
                    manualObject.normal(0, 0, 1);
                    manualObject.binormal(0, 1, 0);
                    manualObject.tangent(1, 0, 0);
                    manualObject.textureCoord(mainTile.left, mainTile.bottom);
                    //rb
                    manualObject.position(vr, -vb, ss.z);
                    manualObject.normal(0, 0, 1);
                    manualObject.binormal(0, 1, 0);
                    manualObject.tangent(1, 0, 0);
                    manualObject.textureCoord(mainTile.right, mainTile.bottom);

                    ++numCreatedSquares;
                }
            }
            #endregion

            //Back
            #region Back
            for (var y = 0; y < dimen.y; ++y)
            {
                for (var x = 0; x < dimen.x; ++x)
                {
                    float vl = x * ts.x - off.x;
                    float vt = y * ts.y - off.y;
                    float vr = (x + 1) * ts.x - off.x;
                    float vb = (y + 1) * ts.y - off.y;

                    //rt
                    manualObject.position(vr, -vt, -ss.z);
                    manualObject.normal(0, 0, -1);
                    manualObject.binormal(0, 1, 0);
                    manualObject.tangent(1, 0, 0);
                    manualObject.textureCoord(mainTile.right, mainTile.top);
                    //lt
                    manualObject.position(vl, -vt, -ss.z);
                    manualObject.normal(0, 0, -1);
                    manualObject.binormal(0, 1, 0);
                    manualObject.tangent(1, 0, 0);
                    manualObject.textureCoord(mainTile.left, mainTile.top);
                    //rb
                    manualObject.position(vr, -vb, -ss.z);
                    manualObject.normal(0, 0, -1);
                    manualObject.binormal(0, 1, 0);
                    manualObject.tangent(1, 0, 0);
                    manualObject.textureCoord(mainTile.right, mainTile.bottom);
                    //lb
                    manualObject.position(vl, -vb, -ss.z);
                    manualObject.normal(0, 0, -1);
                    manualObject.binormal(0, 1, 0);
                    manualObject.tangent(1, 0, 0);
                    manualObject.textureCoord(mainTile.left, mainTile.bottom);

                    ++numCreatedSquares;
                }
            }
            #endregion

            //Left x is z and y is y
            #region Left
            for (var y = 0; y < dimen.y; ++y)
            {
                for (var x = 0; x < dimen.z; ++x)
                {
                    float vl = x * ts.z - off.z;
                    float vt = y * ts.y - off.y;
                    float vr = (x + 1) * ts.z - off.z;
                    float vb = (y + 1) * ts.y - off.y;

                    //lt
                    manualObject.position(-ss.x, -vt, vl);
                    manualObject.normal(-1, 0, 0);
                    manualObject.binormal(0, 1, 0);
                    manualObject.tangent(0, 0, 1);
                    manualObject.textureCoord(mainTile.left, mainTile.top);
                    //rt
                    manualObject.position(-ss.x, -vt, vr);
                    manualObject.normal(-1, 0, 0);
                    manualObject.binormal(0, 1, 0);
                    manualObject.tangent(0, 0, 1);
                    manualObject.textureCoord(mainTile.right, mainTile.top);
                    //lb
                    manualObject.position(-ss.x, -vb, vl);
                    manualObject.normal(-1, 0, 0);
                    manualObject.binormal(0, 1, 0);
                    manualObject.tangent(0, 0, 1);
                    manualObject.textureCoord(mainTile.left, mainTile.bottom);
                    //rb
                    manualObject.position(-ss.x, -vb, vr);
                    manualObject.normal(-1, 0, 0);
                    manualObject.binormal(0, 1, 0);
                    manualObject.tangent(0, 0, 1);
                    manualObject.textureCoord(mainTile.right, mainTile.bottom);

                    ++numCreatedSquares;
                }
            }
            #endregion

            //Right x is z and y is y
            #region Right
            for (var y = 0; y < dimen.y; ++y)
            {
                for (var x = 0; x < dimen.z; ++x)
                {
                    float vl = x * ts.z - off.z;
                    float vt = y * ts.y - off.y;
                    float vr = (x + 1) * ts.z - off.z;
                    float vb = (y + 1) * ts.y - off.y;

                    //rt
                    manualObject.position(ss.x, -vt, vr);
                    manualObject.normal(1, 0, 0);
                    manualObject.binormal(0, 1, 0);
                    manualObject.tangent(0, 0, 1);
                    manualObject.textureCoord(mainTile.right, mainTile.top);
                    //lt
                    manualObject.position(ss.x, -vt, vl);
                    manualObject.normal(1, 0, 0);
                    manualObject.binormal(0, 1, 0);
                    manualObject.tangent(0, 0, 1);
                    manualObject.textureCoord(mainTile.left, mainTile.top);
                    //rb
                    manualObject.position(ss.x, -vb, vr);
                    manualObject.normal(1, 0, 0);
                    manualObject.binormal(0, 1, 0);
                    manualObject.tangent(0, 0, 1);
                    manualObject.textureCoord(mainTile.right, mainTile.bottom);
                    //lb
                    manualObject.position(ss.x, -vb, vl);
                    manualObject.normal(1, 0, 0);
                    manualObject.binormal(0, 1, 0);
                    manualObject.tangent(0, 0, 1);
                    manualObject.textureCoord(mainTile.left, mainTile.bottom);

                    ++numCreatedSquares;
                }
            }
            #endregion

            //Top
            if (!tileset.tryGetTile(TopTileName, out mainTile))
            {
                blacklist($"Cannot find top tile {TopTileName} in Tileset {TilesetName}");
            }
            #region Top
            for (var y = 0; y < dimen.z; ++y)
            {
                for (var x = 0; x < dimen.x; ++x)
                {
                    float vl = x * ts.x - off.x;
                    float vt = y * ts.z - off.z;
                    float vr = (x + 1) * ts.x - off.x;
                    float vb = (y + 1) * ts.z - off.z;

                    //rt
                    manualObject.position(vr, ss.y, -vt);
                    manualObject.normal(0, 1, 0);
                    manualObject.binormal(0, 0, 1);
                    manualObject.tangent(1, 0, 0);
                    manualObject.textureCoord(mainTile.right, mainTile.top);
                    //lt
                    manualObject.position(vl, ss.y, -vt);
                    manualObject.normal(0, 1, 0);
                    manualObject.binormal(0, 0, 1);
                    manualObject.tangent(1, 0, 0);
                    manualObject.textureCoord(mainTile.left, mainTile.top);
                    //rb
                    manualObject.position(vr, ss.y, -vb);
                    manualObject.normal(0, 1, 0);
                    manualObject.binormal(0, 0, 1);
                    manualObject.tangent(1, 0, 0);
                    manualObject.textureCoord(mainTile.right, mainTile.bottom);
                    //lb
                    manualObject.position(vl, ss.y, -vb);
                    manualObject.normal(0, 1, 0);
                    manualObject.binormal(0, 0, 1);
                    manualObject.tangent(1, 0, 0);
                    manualObject.textureCoord(mainTile.left, mainTile.bottom);

                    ++numCreatedSquares;
                }
            }
            #endregion

            //Botton
            if (!tileset.tryGetTile(BottomTileName, out mainTile))
            {
                blacklist($"Cannot find bottom tile {BottomTileName} in Tileset {TilesetName}");
            }
            #region Bottom
            for (var y = 0; y < dimen.z; ++y)
            {
                for (var x = 0; x < dimen.x; ++x)
                {
                    float vl = x * ts.x - off.x;
                    float vt = y * ts.z - off.z;
                    float vr = (x + 1) * ts.x - off.x;
                    float vb = (y + 1) * ts.z - off.z;

                    //lt
                    manualObject.position(vl, -ss.y, -vt);
                    manualObject.normal(0, -1, 0);
                    manualObject.binormal(0, 0, 1);
                    manualObject.tangent(1, 0, 0);
                    manualObject.textureCoord(mainTile.left, mainTile.top);
                    //rt
                    manualObject.position(vr, -ss.y, -vt);
                    manualObject.normal(0, -1, 0);
                    manualObject.binormal(0, 0, 1);
                    manualObject.tangent(1, 0, 0);
                    manualObject.textureCoord(mainTile.right, mainTile.top);
                    //lb
                    manualObject.position(vl, -ss.y, -vb);
                    manualObject.normal(0, -1, 0);
                    manualObject.binormal(0, 0, 1);
                    manualObject.tangent(1, 0, 0);
                    manualObject.textureCoord(mainTile.left, mainTile.bottom);
                    //rb
                    manualObject.position(vr, -ss.y, -vb);
                    manualObject.normal(0, -1, 0);
                    manualObject.binormal(0, 0, 1);
                    manualObject.tangent(1, 0, 0);
                    manualObject.textureCoord(mainTile.right, mainTile.bottom);

                    ++numCreatedSquares;
                }
            }
            #endregion

            //If you want to do the math instead of counting
            //uint numSquares = (uint)(dimen.x * dimen.y * 2 + dimen.x * dimen.z * 2 + dimen.y * dimen.z * 2);

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
