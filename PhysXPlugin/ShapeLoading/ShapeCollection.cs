using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PhysXWrapper;

namespace Engine.Physics.ShapeLoading
{
    /// <summary>
    /// A collection of one or more shapes that describe the collision for an object.
    /// </summary>
    public class ShapeCollection
    {
        LinkedList<PhysShapeDesc> shapes = new LinkedList<PhysShapeDesc>();

        /// <summary>
        /// Constructor, takes a name.  This name must be unique and will be used to
        /// identify the shape in the shape repository.
        /// </summary>
        /// <param name="name">A name for the shape.  Must be unique.</param>
        public ShapeCollection(String name)
        {
            this.Name = name;
        }

        /// <summary>
        /// Add a shape to the collection.
        /// </summary>
        /// <param name="shape">The shape to add.</param>
        public void addShape(PhysShapeDesc shape)
        {
            shapes.AddLast(shape);
        }

        /// <summary>
        /// Remove a shape from the collection.
        /// </summary>
        /// <param name="shape">The shape to remove.</param>
        public void removeShape(PhysShapeDesc shape)
        {
            shapes.Remove(shape);
        }

        /// <summary>
        /// Get an enumerator over the shapes so they can be used elsewhere.
        /// </summary>
        /// <returns>An enumerator over the shapes.</returns>
        public IEnumerable<PhysShapeDesc> getShapeEnum()
        {
            return shapes;
        }

        /// <summary>
        /// This function will update all the material ID's for the shapes in the collection.
        /// This should be called whenever the material ID's might change.
        /// </summary>
        /// <param name="repository">The shape repository that contains the materials.</param>
        public void updateMaterials(ShapeRepository repository)
        {
            //foreach (PhysShapeDesc shape in shapes)
            //{
            //    shape.setMaterial(repository.getMaterial(shape.MaterialName));
            //}
        }

        /// <summary>
        /// The name of the shape.
        /// </summary>
        public String Name { get; private set; }

        /// <summary>
        /// The number of shapes in the collection.
        /// </summary>
        public int Count
        {
            get
            {
                return shapes.Count;
            }
        }

        public ShapeLocation SourceLocation { get; internal set; }
    }
}
