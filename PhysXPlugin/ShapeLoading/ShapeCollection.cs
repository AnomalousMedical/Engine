using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PhysXWrapper;

namespace PhysXPlugin
{
    /// <summary>
    /// A collection of one or more shapes that describe the collision for an object.
    /// </summary>
    public class ShapeCollection
    {
        LinkedList<PhysShapeDesc> shapes = new LinkedList<PhysShapeDesc>();
        //Dictionary<PhysShapeDesc, String> materialBindings = new Dictionary<PhysShapeDesc, string>();

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

        ///// <summary>
        ///// Add a binding from a given shape to a material. That material will
        ///// be used on the shape when it is created.
        ///// </summary>
        ///// <param name="shape">The shape to add the binding for.</param>
        ///// <param name="materialName"></param>
        //public void addMaterialBinding(PhysShapeDesc shape, String materialName)
        //{
        //    materialBindings.Add(shape, materialName);
        //}

        //public void removeMaterialBinding(PhysShapeDesc shape)
        //{
        //    materialBindings.Remove(shape);
        //}

        //public bool hasShapeBinding(PhysShapeDesc shape)
        //{
        //    return materialBindings.ContainsKey(shape);
        //}

        //public String getMaterialBinding(PhysShapeDesc shape)
        //{
        //    return materialBindings[shape];
        //}

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
