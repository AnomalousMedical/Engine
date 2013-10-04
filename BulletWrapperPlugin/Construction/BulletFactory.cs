using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Engine.ObjectManagement;

namespace BulletPlugin
{
    public class BulletFactory : SimElementFactory
    {
        BulletScene scene;
	    List<BulletFactoryEntry> rigidBodies = new List<BulletFactoryEntry>();
        //List<BulletFactoryEntry> softBodies;
        List<BulletFactoryEntry> typedConstraints = new List<BulletFactoryEntry>();
        //List<SoftBodyProviderEntry> softBodyProviders;
        //List<BulletFactoryEntry> softBodyAnchors;

        public BulletFactory(BulletScene scene)
        {
            this.scene = scene;
        }

        internal void addRigidBody(RigidBodyDefinition definition, SimObjectBase instance)
        {
            rigidBodies.Add(new BulletFactoryEntry(instance, definition));
        }

        internal void addTypedConstraint(TypedConstraintDefinition definition, SimObjectBase instance)
        {
            typedConstraints.Add(new BulletFactoryEntry(instance, definition));
        }

        //internal void addSoftBody(SoftBodyDefinition definition, SimObjectBase instance);

        //internal void addSoftBodyProviderDefinition(SoftBodyProviderDefinition definition, SimObjectBase instance, SimSubScene subScene);

        //internal void addSoftBodyAnchorOrJointDefinition(BulletElementDefinition definition, SimObjectBase instance);

        public void createProducts()
        {
            foreach(BulletFactoryEntry entry in rigidBodies)
	        {
		        entry.createProduct(scene);
	        }
            //foreach(SoftBodyProviderEntry entry in softBodyProviders)
            //{
            //    entry.createProduct(scene);
            //}
            //foreach(BulletFactoryEntry entry in softBodies)
            //{
            //    entry.createProduct(scene);
            //}
            foreach (BulletFactoryEntry entry in typedConstraints)
            {
                entry.createProduct(scene);
            }
            //foreach(BulletFactoryEntry entry in softBodyAnchors)
            //{
            //    entry.createProduct(scene);
            //}
        }

        public void createStaticProducts()
        {
            foreach (BulletFactoryEntry entry in rigidBodies)
            {
                entry.createStaticProduct(scene);
            }
            //foreach(SoftBodyProviderEntry entry in softBodyProviders)
            //{
            //    entry.createStaticProduct(scene);
            //}
            //foreach(BulletFactoryEntry entry in softBodies)
            //{
            //    entry.createStaticProduct(scene);
            //}
            foreach (BulletFactoryEntry entry in typedConstraints)
            {
                entry.createStaticProduct(scene);
            }
            //foreach(BulletFactoryEntry entry in softBodyAnchors)
            //{
            //    entry.createStaticProduct(scene);
            //}
        }

        public void linkProducts()
        {
            
        }

        public void clearDefinitions()
        {
            rigidBodies.Clear();
            //softBodyProviders.Clear();
            //softBodies.Clear();
            //softBodyAnchors.Clear();
            typedConstraints.Clear();
        }
    }
}
