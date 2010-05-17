using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Engine;
using Logging;

namespace BulletPlugin
{
    class BulletShapeRepository : ShapeRepository
    {
        Dictionary<String, BulletShapeCollection> shapes = new Dictionary<String, BulletShapeCollection>();

        public BulletShapeRepository()
        {

        }

        public override void Dispose()
        {
            
        }

        public bool addCollection(BulletShapeCollection collection)
        {
            if (shapes.ContainsKey(collection.Name))
            {
		        Log.Default.sendMessage("Attempted to add a shape with a duplicate name " + collection.Name + " ignoring the new shape.", Logging.LogLevel.Error, "Physics");
                return false;
            }
            else
            {
                shapes.Add(collection.Name, collection);
                collection.SourceLocation = CurrentLoadingLocation;
                CurrentLoadingLocation.addShape(collection.Name);
                return true;
            }
        }

        public override void removeCollection(string name)
        {
            if (shapes.ContainsKey(name))
            {
                BulletShapeCollection collection = shapes[name];
                shapes.Remove(name);
                collection.Dispose();
            }
            else
            {
		        Logging.Log.Default.sendMessage("Attempted to remove a shape " + name + " that does not exist.  No changes made.", Logging.LogLevel.Error, "Physics");
            }
        }

        public BulletShapeCollection getCollection(String name)
        {
            if (name != null && shapes.ContainsKey(name))
            {
                return shapes[name];
            }
            else
            {
		        Logging.Log.Default.sendMessage("Could not find a shape named " + name + ".", Logging.LogLevel.Error, "Physics");
            }
            return null;
        }

        public bool containsValidCollection(String name)
        {
            return name != null && shapes.ContainsKey(name) && shapes[name].Count > 0;
        }

        public override void addConvexMesh(string name, ConvexMesh mesh)
        {
            throw new NotImplementedException();
        }

        public override void destroyConvexMesh(string name)
        {
            throw new NotImplementedException();
        }

        public override void addTriangleMesh(string name, TriangleMesh mesh)
        {
            throw new NotImplementedException();
        }

        public override void destroyTriangleMesh(string name)
        {
            throw new NotImplementedException();
        }

        public override void addMaterial(string name, ShapeMaterial materialDesc)
        {
            throw new NotImplementedException();
        }

        public override bool hasMaterial(string name)
        {
            throw new NotImplementedException();
        }

        public override ShapeMaterial getMaterial(string name)
        {
            throw new NotImplementedException();
        }

        public override void destroyMaterial(string name)
        {
            throw new NotImplementedException();
        }

        public override void addSoftBodyMesh(SoftBodyMesh softBodyMesh)
        {
            throw new NotImplementedException();
        }

        public override SoftBodyMesh getSoftBodyMesh(string name)
        {
            throw new NotImplementedException();
        }

        public override void destroySoftBodyMesh(string name)
        {
            throw new NotImplementedException();
        }
    }
}
