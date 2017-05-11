using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using Logging;
using Engine;
using Engine.Resources;

namespace Engine
{
    /// <summary>
    /// This class implements a shape loader that can parse XML shape files.
    /// </summary>
    public class XMLShapeLoader : ShapeLoader
    {
        private static String COMPOUND = "Compound";
        private static String SPHERE = "Sphere";
        private static String BOX = "Box";
        private static String MESH = "Mesh";
        private static String CONVEXHULL = "ConvexHull";
        private static String PLANE = "Plane";
        private static String CAPSULE = "Capsule";
        private static String RADIUS = "radius";
        private static String HEIGHT = "height";
        private static String LOCATION = "Location";
        private static String ROTATION = "Rotation";
        private static String NAME = "name";
        private static String COLLISION_SHAPE = "CollisionShape";
        private static String EXTENTS = "extents";
        private static String VERTICES = "Vertices";
        private static String FACES = "Faces";
        private static String NORMAL = "normal";
        private static String DISTANCE = "distance";
        private static String MATERIAL = "Material";
        private static String RESTITUTION = "Restitution";
        private static String STATIC_FRICTION = "StaticFriction";
        private static String DYNAMIC_FRICTION = "DynamicFriction";
        private static String SOFT_BODY = "SoftBody";

        private static float DEG_TO_RAD = 0.0174532925f;

        private static char[] SEPS = { ',' };

        private String currentMaterial = null;

        public XMLShapeLoader()
            : base(".collision")
        {

        }

        public override bool canLoadShape(string filename, VirtualFileSystem vfs)
        {
            bool canLoad = filename.EndsWith(extension);
            if (canLoad)
            {
                try
                {
                    using (var stream = vfs.openStream(filename, FileMode.Open, FileAccess.Read))
                    {
                        using (XmlReader xmlReader = XmlReader.Create(stream, new XmlReaderSettings() { DtdProcessing = DtdProcessing.Prohibit, IgnoreWhitespace = true }))
                        {

                        }
                    }
                }
                catch (Exception e)
                {
                    Log.Error("Failed to load {0} because of exception {1}", filename, e.Message);
                    canLoad = false;
                }
            }
            return canLoad;
        }

        public override void loadShapes(ShapeBuilder builder, string filename, VirtualFileSystem vfs)
        {
            Log.Default.sendMessage("Loading collision shapes from " + filename + ".", LogLevel.Info, "ShapeLoading");
            using (var stream = vfs.openStream(filename, FileMode.Open, FileAccess.Read))
            {
                using (XmlReader textReader = XmlReader.Create(stream, new XmlReaderSettings() { DtdProcessing = DtdProcessing.Prohibit, IgnoreWhitespace = true }))
                {
                    while (textReader.Read())
                    {
                        if (textReader.NodeType == XmlNodeType.Element)
                        {
                            if (textReader.Name.Equals(COMPOUND))
                            {
                                loadCompound(textReader, builder);
                            }
                            else if (textReader.Name.Equals(SPHERE))
                            {
                                loadSphere(textReader, builder);
                            }
                            else if (textReader.Name.Equals(BOX))
                            {
                                loadBox(textReader, builder);
                            }
                            else if (textReader.Name.Equals(MESH))
                            {
                                loadMesh(textReader, builder);
                            }
                            else if (textReader.Name.Equals(CONVEXHULL))
                            {
                                loadConvexHull(textReader, builder);
                            }
                            else if (textReader.Name.Equals(MATERIAL))
                            {
                                loadMaterial(textReader, builder);
                            }
                            else if (textReader.Name.Equals(PLANE))
                            {
                                loadPlane(textReader, builder);
                            }
                            else if (textReader.Name.Equals(CAPSULE))
                            {
                                loadCapsule(textReader, builder);
                            }
                            else if (textReader.Name.Equals(SOFT_BODY))
                            {
                                loadSoftBody(textReader, builder);
                            }
                        }
                    }
                }
            }
#if VERBOSE
            Log.Default.sendMessage("Finished loading collision shapes from " + filename + ".", LogLevel.Info, "ShapeLoading");
#endif
        }

        private Quaternion readRotation(XmlReader textReader)
        {
            Quaternion rotation = new Quaternion();
            textReader.Read();
            String[] rots = textReader.Value.Split(SEPS);
            //Euler angles
            if (rots.Length == 3)
            {
                rotation.setEuler(NumberParser.ParseFloat(rots[0]) * DEG_TO_RAD, NumberParser.ParseFloat(rots[1]) * DEG_TO_RAD, NumberParser.ParseFloat(rots[2]) * DEG_TO_RAD);
            }
            else
            {
                Log.Default.sendMessage("Error loading rotation on line " + /*textReader.LineNumber*/ "cannot get line number" + " does not contain 3 or 4 numbers value: " + textReader.Value, LogLevel.Warning, "ShapeLoading");
                rotation = Quaternion.Identity;
            }
            return rotation;
        }

        private Vector3 readTranslation(XmlReader textReader)
        {
            Vector3 translation = new Vector3();
            textReader.Read();
            String[] locs = textReader.Value.Split(SEPS);
            if (locs.Length == 3)
            {
                translation.x = NumberParser.ParseFloat(locs[0]);
                translation.y = NumberParser.ParseFloat(locs[1]);
                translation.z = NumberParser.ParseFloat(locs[2]);
            }
            else
            {
                Log.Default.sendMessage("Error loading translation on line " + /*textReader.LineNumber*/ "cannot get line number" + " does not contain 3 numbers value: " + textReader.Value, LogLevel.Error, "ShapeLoading");
                translation = Vector3.Zero;
            }
            return translation;
        }

        private String readName(XmlReader textReader)
        {
            return textReader.GetAttribute(NAME);
        }

        private float readRadius(XmlReader textReader)
        {
            return NumberParser.ParseFloat(textReader.GetAttribute(RADIUS));
        }

        private float readHeight(XmlReader textReader)
        {
            return NumberParser.ParseFloat(textReader.GetAttribute(HEIGHT));
        }

        private Vector3 readExtents(XmlReader textReader)
        {
            Vector3 extents = new Vector3();
            String[] exts = textReader.GetAttribute(EXTENTS).Split(SEPS);
            if (exts.Length == 3)
            {
                extents.x = NumberParser.ParseFloat(exts[0]);
                extents.y = NumberParser.ParseFloat(exts[1]);
                extents.z = NumberParser.ParseFloat(exts[2]);
            }
            else
            {
                Log.Default.sendMessage("Error loading extents on line " + /*textReader.LineNumber*/ "cannot get line number" + " does not contain 3 numbers value: " + textReader.Value, LogLevel.Error, "ShapeLoading");
            }
            return extents;
        }

        private Vector3 readPlaneNormal(XmlReader textReader)
        {
            Vector3 nml = new Vector3();
            String[] n = textReader.GetAttribute(NORMAL).Split(SEPS);
            if (n.Length == 3)
            {
                nml.x = NumberParser.ParseFloat(n[0]);
                nml.y = NumberParser.ParseFloat(n[1]);
                nml.z = NumberParser.ParseFloat(n[2]);
            }
            else
            {
                Log.Default.sendMessage("Error loading normal on line " + /*textReader.LineNumber*/ "cannot get line number" + " does not contain 3 numbers value: " + textReader.Value, LogLevel.Error, "ShapeLoading");
            }
            return nml;
        }

        private float readPlaneDistance(XmlReader textReader)
        {
            return NumberParser.ParseFloat(textReader.GetAttribute(DISTANCE));
        }

        float[] readVertices(XmlReader textReader)
        {
            textReader.Read();
            String[] strVerts = textReader.Value.Split(SEPS);
            float[] vertices;
            float junk;
            if (NumberParser.TryParse(strVerts[strVerts.Length - 1], out junk))
            {
                vertices = new float[strVerts.Length];
            }
            else
            {
                vertices = new float[strVerts.Length - 1];
            }
            for (uint i = 0; i < vertices.Length; ++i)
            {
                vertices[i] = NumberParser.ParseFloat(strVerts[i]);
            }
            return vertices;
        }

        int[] readFaces(XmlReader textReader)
        {
            textReader.Read();
            String[] strVerts = textReader.Value.Split(SEPS);
            int[] faces;
            int junk;
            if (NumberParser.TryParse(strVerts[strVerts.Length - 1], out junk))
            {
                faces = new int[strVerts.Length];
            }
            else
            {
                faces = new int[strVerts.Length - 1];
            }
            for (uint i = 0; i < faces.Length; ++i)
            {
                faces[i] = NumberParser.ParseInt(strVerts[i]);
            }
            return faces;
        }

        private void loadSphere(XmlReader textReader, ShapeBuilder builder)
        {
            float radius = readRadius(textReader);
            Vector3 location = new Vector3();
            String name = readName(textReader);
            while (!(textReader.Name == SPHERE && textReader.NodeType == XmlNodeType.EndElement) && textReader.Read())
            {
                if (textReader.NodeType == XmlNodeType.Element)
                {
                    if (textReader.Name.Equals(LOCATION))
                    {
                        location = readTranslation(textReader);
                    }
                }
            }
            builder.buildSphere(name, radius, location, currentMaterial);
            currentMaterial = null;
#if VERBOSE
            Log.Default.sendMessage("Creating collision sphere named " + name + ".", LogLevel.Info, "ShapeLoading");
#endif
        }

        private void loadBox(XmlReader textReader, ShapeBuilder builder)
        {
            Vector3 extents = readExtents(textReader);
            String name = readName(textReader);
            Quaternion rotation = Quaternion.Identity;
            Vector3 location = Vector3.Zero;
            while (!(textReader.Name == BOX && textReader.NodeType == XmlNodeType.EndElement) && textReader.Read())
            {
                if (textReader.NodeType == XmlNodeType.Element)
                {
                    if (textReader.Name.Equals(LOCATION))
                    {
                        location = readTranslation(textReader);
                    }
                    else if (textReader.Name.Equals(ROTATION))
                    {
                        rotation = readRotation(textReader);
                    }
                }
            }
            builder.buildBox(name, extents, location, rotation, currentMaterial);
            currentMaterial = null;
#if VERBOSE
            Log.Default.sendMessage("Creating collision box named " + name + ".", LogLevel.Info, "ShapeLoading");
#endif
        }

        private void loadMesh(XmlReader textReader, ShapeBuilder builder)
        {
            String name = readName(textReader);
            Vector3 location = Vector3.Zero;
            Quaternion rotation = Quaternion.Identity;
            float[] vertices = null;
            int[] faces = null;
            while (!(textReader.Name == MESH && textReader.NodeType == XmlNodeType.EndElement) && textReader.Read())
            {
                if (textReader.NodeType == XmlNodeType.Element)
                {
                    if (textReader.Name.Equals(LOCATION))
                    {
                        location = readTranslation(textReader);
                    }
                    else if (textReader.Name.Equals(ROTATION))
                    {
                        rotation = readRotation(textReader);
                    }
                    else if (textReader.Name.Equals(VERTICES))
                    {
                        vertices = readVertices(textReader);
                    }
                    else if (textReader.Name.Equals(FACES))
                    {
                        faces = readFaces(textReader);
                    }
                }
            }
            builder.buildMesh(name, vertices, faces, location, rotation, currentMaterial);
            currentMaterial = null;
#if VERBOSE
            Log.Default.sendMessage("Creating collision mesh named " + name + ".", LogLevel.Info, "ShapeLoading");
#endif
        }

        void loadPlane(XmlReader textReader, ShapeBuilder builder)
        {
            String name = readName(textReader);
            Vector3 normal = readPlaneNormal(textReader);
            float d = readPlaneDistance(textReader);
            builder.buildPlane(name, normal, d, currentMaterial);
            currentMaterial = null;
#if VERBOSE
            Log.Default.sendMessage("Creating collision plane named " + name + ".", LogLevel.Info, "ShapeLoading");
#endif
        }

        void loadCapsule(XmlReader textReader, ShapeBuilder builder)
        {
            String name = readName(textReader);
            Vector3 location = Vector3.Zero;
            Quaternion rotation = Quaternion.Identity;
            float radius = readRadius(textReader);
            float height = readHeight(textReader);
            while (!(textReader.Name == CAPSULE && textReader.NodeType == XmlNodeType.EndElement) && textReader.Read())
            {
                if (textReader.NodeType == XmlNodeType.Element)
                {
                    if (textReader.Name.Equals(LOCATION))
                    {
                        location = readTranslation(textReader);
                    }
                    else if (textReader.Name.Equals(ROTATION))
                    {
                        rotation = readRotation(textReader);
                    }
                }
            }
            builder.buildCapsule(name, radius, height, location, rotation, currentMaterial);
            currentMaterial = null;
#if VERBOSE
            Log.Default.sendMessage("Creating collision capsule named " + name + ".", LogLevel.Info, "ShapeLoading");
#endif
        }

        void loadConvexHull(XmlReader textReader, ShapeBuilder builder)
        {
            String name = readName(textReader);
            Vector3 location = Vector3.Zero;
            Quaternion rotation = Quaternion.Identity;
            float[] vertices = null;
            int[] faces = null;
            while (!(textReader.Name == CONVEXHULL && textReader.NodeType == XmlNodeType.EndElement) && textReader.Read())
            {
                if (textReader.NodeType == XmlNodeType.Element)
                {
                    if (textReader.Name.Equals(LOCATION))
                    {
                        location = readTranslation(textReader);
                    }
                    else if (textReader.Name.Equals(ROTATION))
                    {
                        rotation = readRotation(textReader);
                    }
                    else if (textReader.Name.Equals(VERTICES))
                    {
                        vertices = readVertices(textReader);
                    }
                    else if (textReader.Name.Equals(FACES))
                    {
                        faces = readFaces(textReader);
                    }
                }
            }
            if (faces != null)
            {
                builder.buildConvexHull(name, vertices, faces, location, rotation, currentMaterial);
            }
            else
            {
                builder.buildConvexHull(name, vertices, location, rotation, currentMaterial);
            }
            currentMaterial = null;
#if VERBOSE
            Log.Default.sendMessage("Creating collision convex hull named " + name + ".", LogLevel.Info, "ShapeLoading");
#endif
        }

        private void loadSoftBody(XmlReader textReader, ShapeBuilder builder)
        {
            String name = readName(textReader);
            Vector3 location = Vector3.Zero;
            Quaternion rotation = Quaternion.Identity;
            float[] vertices = null;
            int[] tetrahedra = null;
            while (!(textReader.Name == SOFT_BODY && textReader.NodeType == XmlNodeType.EndElement) && textReader.Read())
            {
                if (textReader.NodeType == XmlNodeType.Element)
                {
                    if (textReader.Name.Equals(LOCATION))
                    {
                        location = readTranslation(textReader);
                    }
                    else if (textReader.Name.Equals(ROTATION))
                    {
                        rotation = readRotation(textReader);
                    }
                    else if (textReader.Name.Equals(VERTICES))
                    {
                        vertices = readVertices(textReader);
                    }
                    else if (textReader.Name.Equals(FACES))
                    {
                        tetrahedra = readFaces(textReader);
                    }
                }
            }
#if VERBOSE
            Log.Default.sendMessage("Creating collision soft body named " + name + ".", LogLevel.Info, "ShapeLoading");
#endif
            builder.buildSoftBody(name, vertices, tetrahedra, location, rotation);
            currentMaterial = null;
        }

        void loadCompound(XmlReader textReader, ShapeBuilder builder)
        {
            String name = readName(textReader);
            builder.startCompound(name);
#if VERBOSE
            Log.Default.sendMessage("--Started compound collision shape named " + name + ".--", LogLevel.Info, "ShapeLoading");
#endif
            while (!(textReader.Name == COMPOUND && textReader.NodeType == XmlNodeType.EndElement) && textReader.Read())
            {
                if (textReader.NodeType == XmlNodeType.Element)
                {
                    if (textReader.Name.Equals(SPHERE))
                    {
                        loadSphere(textReader, builder);
                    }
                    else if (textReader.Name.Equals(BOX))
                    {
                        loadBox(textReader, builder);
                    }
                    else if (textReader.Name.Equals(MESH))
                    {
                        loadMesh(textReader, builder);
                    }
                    else if (textReader.Name.Equals(CONVEXHULL))
                    {
                        loadConvexHull(textReader, builder);
                    }
                    else if (textReader.Name.Equals(MATERIAL))
                    {
                        loadMaterial(textReader, builder);
                    }
                    else if (textReader.Name.Equals(PLANE))
                    {
                        loadPlane(textReader, builder);
                    }
                    else if (textReader.Name.Equals(CAPSULE))
                    {
                        loadCapsule(textReader, builder);
                    }
                }
            }
            builder.stopCompound(name);
#if VERBOSE
            Log.Default.sendMessage("--Finished compound collision shape named " + name + ".--", LogLevel.Info, "ShapeLoading");
#endif
        }

        private void loadMaterial(XmlReader textReader, ShapeBuilder builder)
        {
            currentMaterial = textReader.GetAttribute("name");
#if VERBOSE
            Log.Default.sendMessage("Creating material named {0}.", LogLevel.Info, "ShapeLoading", currentMaterial);
#endif
            float restitution = 0.0f;
            float staticFriction = 0.0f;
            float dynamicFriction = 0.0f;
            while (!(textReader.Name == MATERIAL && textReader.NodeType == XmlNodeType.EndElement) && textReader.Read())
            {
                if (textReader.NodeType == XmlNodeType.Element)
                {
                    if (textReader.Name.Equals(RESTITUTION))
                    {
                        restitution = readRestitution(textReader);
                    }
                    else if (textReader.Name.Equals(STATIC_FRICTION))
                    {
                        staticFriction = readStaticFriction(textReader);
                    }
                    else if (textReader.Name.Equals(DYNAMIC_FRICTION))
                    {
                        dynamicFriction = readDynamicFriction(textReader);
                    }
                }
            }
            builder.createMaterial(currentMaterial, restitution, staticFriction, dynamicFriction);
        }

        private float readDynamicFriction(XmlReader textReader)
        {
            float result = 0.0f;
            while (!(textReader.Name == DYNAMIC_FRICTION && textReader.NodeType == XmlNodeType.EndElement) && textReader.Read())
            {
                if (textReader.NodeType == XmlNodeType.Text)
                {
                    NumberParser.TryParse(textReader.Value, out result);
                }
            }
            return result;
        }

        private float readStaticFriction(XmlReader textReader)
        {
            float result = 0.0f;
            while (!(textReader.Name == STATIC_FRICTION && textReader.NodeType == XmlNodeType.EndElement) && textReader.Read())
            {
                if (textReader.NodeType == XmlNodeType.Text)
                {
                    NumberParser.TryParse(textReader.Value, out result);
                }
            }
            return result;
        }

        private float readRestitution(XmlReader textReader)
        {
            float result = 0.0f;
            while (!(textReader.Name == RESTITUTION && textReader.NodeType == XmlNodeType.EndElement) && textReader.Read())
            {
                if (textReader.NodeType == XmlNodeType.Text)
                {
                    NumberParser.TryParse(textReader.Value, out result);
                }
            }
            return result;
        }
    }
}
