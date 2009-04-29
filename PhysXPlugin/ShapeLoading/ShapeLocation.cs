using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Engine.Resources;

namespace Engine.Physics.ShapeLoading
{
    /// <summary>
    /// A shape location keeps track of all the shapes loaded from a specific place
    /// on the filesystem.
    /// </summary>
    public class ShapeLocation
    {
        private static String[] pathSeparators = { "/", "\\" };

        public static String[] PathSeparators
        {
            get
            {
                return pathSeparators;
            }
        }

        private LinkedList<String> shapesFound = new LinkedList<string>();
        private LinkedList<String> hullsFound = new LinkedList<string>();
        private LinkedList<String> meshesFound = new LinkedList<string>();
        private LinkedList<String> materialsFound = new LinkedList<string>();
        private LinkedList<String> softBodiesFound = new LinkedList<string>();

        public String LocName { get; set; }
        public ResourceType Type { get; set; }
        public bool Recursive { get; set; }
        public bool Loaded { get; set; }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="locName">The name of the location (the path).</param>
        /// <param name="type">The type of the location.</param>
        /// <param name="recursive">If this is true and the location is a directory it will be scanned for all valid files.</param>
        public ShapeLocation(String locName, ResourceType type, bool recursive, ShapeGroup parentGroup)
        {
            this.LocName = locName;
            this.Type = type;
            this.Recursive = recursive;
            this.Loaded = false;
            this.Path = parentGroup.Name + pathSeparators[0] + locName;
        }

        /// <summary>
        /// Destroy any resources loaded from this location.
        /// </summary>
        /// <param name="repository">The repository containing the resources to destroy.</param>
        public void destroyResources(ShapeRepository repository)
        {
            foreach (String shape in shapesFound)
            {
                repository.removeCollection(shape);
            }
            foreach (String hull in hullsFound)
            {
                repository.destroyConvexMesh(hull);
            }
            foreach (String triMesh in meshesFound)
            {
                repository.destroyTriangleMesh(triMesh);
            }
            foreach (String material in materialsFound)
            {
                repository.destroyMaterial(material);
            }
            foreach (String softBody in softBodiesFound)
            {
                repository.destroySoftBodyMesh(softBody);
            }
            hullsFound.Clear();
            shapesFound.Clear();
            meshesFound.Clear();
            materialsFound.Clear();
            softBodiesFound.Clear();
        }

        /// <summary>
        /// Add a shape that was found for the current location.
        /// </summary>
        /// <param name="name">The name of the shape to add.</param>
        public void addShape(String name)
        {
            shapesFound.AddLast(name);
        }

        /// <summary>
        /// Add a hull that was found for the current location.
        /// </summary>
        /// <param name="name">The name of the hull that was added.</param>
        public void addHull(String name)
        {
            hullsFound.AddLast(name);
        }

        /// <summary>
        /// Add a mesh that was found for the current location.
        /// </summary>
        /// <param name="name">The name of the mesh that was found.</param>
        public void addMesh(String name)
        {
            meshesFound.AddLast(name);
        }

        /// <summary>
        /// Add a material that was found in this location.
        /// </summary>
        /// <param name="name">The name of the found material.</param>
        public void addMaterial(String name)
        {
            materialsFound.AddLast(name);
        }

        /// <summary>
        /// Add a soft body that was found in this location.
        /// </summary>
        /// <param name="name">The name of the discovered soft body.</param>
        public void addSoftBody(String name)
        {
            softBodiesFound.AddLast(name);
        }

        public String Path { get; private set; }
    }
}
