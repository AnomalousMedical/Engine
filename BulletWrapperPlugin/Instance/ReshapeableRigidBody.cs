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
        private List<ReshapeableRigidBodySection> sections = new List<ReshapeableRigidBodySection>();
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
                section.Dispose();
            }
            sections.Clear();
            compoundShape.Dispose();
            compoundShape = null;
            base.Dispose();
        }

        public void beginUpdates()
        {
            if (compoundShape != null)
            {
                Scene.removeRigidBody(this);
            }
        }

        public void finishUpdates()
        {
            if (compoundShape != null)
            {
                float mass = getInvMass();
                if (mass > 0.0f)
                {
                    mass = 1.0f / mass;
                }

                Vector3 localInertia = new Vector3();
                compoundShape.calculateLocalInertia(mass, ref localInertia);
                this.setMassProps(mass, localInertia);

                Scene.addRigidBody(this, collisionFilterGroup, collisionFilterMask);
            }
        }

        /// <summary>
        /// Add a named shape to a given region, will return a new ReshapeableRigidBodySection if this works correctly.
        /// The returned handles will be owned by this object, so there is no need to dispose them or clean them up (if this
        /// object is disposed). However, you can manually remove a section at any time by calling destroySection.
        /// </summary>
        public ReshapeableRigidBodySection createSection(String shapeName, Vector3 translation, Quaternion rotation, Vector3 scale)
        {
            BulletShapeRepository repository = BulletInterface.Instance.ShapeRepository;
            if (repository.containsValidCollection(shapeName))
            {
                var section = new ReshapeableRigidBodySection(this, repository.getCollection(shapeName).CollisionShape.createClone(), translation, rotation);
                sections.Add(section);

                return section;
            }

            return null;
        }

        /// <summary>
        /// Empty and destroy a section removing it from the collision shape. It will no longer
        /// be usable after calling this function.
        /// </summary>
        /// <param name="name">The name of the region to destroy.</param>
        public void destroySection(ReshapeableRigidBodySection section)
        {
            sections.Remove(section);
            section.Dispose();
        }

        internal btCompoundShape CompoundShape
        {
            get
            {
                return compoundShape;
            }
        }
    }
}
