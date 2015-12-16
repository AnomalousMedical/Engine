using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Engine;
using System.Runtime.InteropServices;

namespace BulletPlugin
{
    public class ReshapeableRigidBody : RigidBody
    {
        class Section : IDisposable
        {
            btCollisionShape collisionShape;
            Vector3 translation;
            Quaternion rotation;

            public Section(btCollisionShape collisionShape, Vector3 translation, Quaternion rotation)
            {
                this.collisionShape = collisionShape;
                this.translation = translation;
                this.rotation = rotation;
            }

            public void Dispose()
            {
                collisionShape.Dispose();
            }

            public void addToShape(btCompoundShape compoundShape)
            {
                compoundShape.addChildShape(collisionShape, translation, rotation);
            }

            public void removeFromShape(btCompoundShape compoundShape)
            {
                compoundShape.removeChildShape(collisionShape);
            }
        }

        private List<Section> sections = new List<Section>();
        private btCompoundShape compoundShape;

        public ReshapeableRigidBody(ReshapeableRigidBodyDefinition description, BulletScene scene, btCompoundShape collisionShape, Vector3 initialTrans, Quaternion initialRot)
            : base(description, scene, collisionShape, initialTrans, initialRot)
        {
            this.compoundShape = collisionShape;
        }

        protected override void Dispose()
        {
            foreach(var section in sections)
            {
                section.removeFromShape(compoundShape);
                section.Dispose();
            }
            compoundShape.Dispose();
            base.Dispose();
        }

        /// <summary>
        /// Add a named shape to a given region, will return true if this works correctly
        /// </summary>
        /// <param name="regionName"></param>
        /// <param name="shapeName"></param>
        /// <param name="translation"></param>
        /// <param name="rotation"></param>
        /// <returns></returns>
        public bool addNamedShape(String regionName, String shapeName, Vector3 translation, Quaternion rotation, Vector3 scale)
        {
            BulletShapeRepository repository = BulletInterface.Instance.ShapeRepository;
            if (repository.containsValidCollection(shapeName))
            {
                Scene.removeRigidBody(this);

                var section = new Section(repository.getCollection(shapeName).CollisionShape.createClone(), translation, rotation);
                sections.Add(section);
                section.addToShape(compoundShape);

                recomputeMassProps();

                Scene.addRigidBody(this, collisionFilterGroup, collisionFilterMask);

                return true;
            }

            return false;
        }

        /// <summary>
        /// Empty and destroy a region removing it from the collision shape.
        /// </summary>
        /// <param name="name">The name of the region to destroy.</param>
        public void destroyRegion(String name)
        {
            
        }

        /// <summary>
        /// This function will recompute the mass props. It should be called when
        /// the collision shape is changed.
        /// </summary>
        public void recomputeMassProps()
        {
            //ReshapeableRigidBody_recomputeMassProps(nativeReshapeable);
            float mass = getInvMass();
            if (mass > 0.0f)
            {
                mass = 1.0f / mass;
            }

            Vector3 localInertia = new Vector3();
            compoundShape.calculateLocalInertia(mass, ref localInertia);
            this.setMassProps(mass, localInertia);
        }

        public void moveOrigin(String regionName, Vector3 translation, Quaternion rotation)
        {
            
        }

        public void setLocalScaling(String regionName, Vector3 scaling)
        {
            
        }
    }
}
