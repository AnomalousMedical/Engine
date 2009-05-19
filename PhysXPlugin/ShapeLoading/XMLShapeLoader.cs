using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using PhysXWrapper;
using Logging;
using Engine;

namespace PhysXPlugin
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
            :base(".collision")
        {

        }

        public override bool canLoadShape(string filename)
        {
            bool canLoad = filename.EndsWith(extension);
            if (canLoad)
            {
                try
                {
                    XmlTextReader textReader = new XmlTextReader(filename);
                    textReader.Close();
                }
                catch (Exception e)
                {
                    canLoad = false;
                }
            }
            return canLoad;
        }

        public override void loadShapes(ShapeBuilder builder, string filename)
        {
            Log.Default.sendMessage("Loading collision shapes from " + filename + ".", LogLevel.Info, "ShapeLoading");
            XmlTextReader textReader = new XmlTextReader(filename);

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

            textReader.Close();
            Log.Default.sendMessage("Finished loading collision shapes from " + filename + ".", LogLevel.Info, "ShapeLoading");
        }

        private Quaternion readRotation(XmlTextReader textReader)
        {
            Quaternion rotation = new Quaternion();
            textReader.Read();
            String[] rots = textReader.Value.Split(SEPS);
            //Euler angles
            if (rots.Length == 3)
            {
                rotation.setEuler(float.Parse(rots[0]) * DEG_TO_RAD, float.Parse(rots[1]) * DEG_TO_RAD, float.Parse(rots[2]) * DEG_TO_RAD);
            }
            else
            {
                Log.Default.sendMessage("Error loading rotation on line " + textReader.LineNumber + " does not contain 3 or 4 numbers value: " + textReader.Value, LogLevel.Error, "ShapeLoading");
                rotation = Quaternion.Identity;
            }
            return rotation;
        }

        private Vector3 readTranslation(XmlTextReader textReader)
        {
            Vector3 translation = new Vector3();
            textReader.Read();
            String[] locs = textReader.Value.Split(SEPS);
            if (locs.Length == 3)
            {
                translation.x = float.Parse(locs[0]);
                translation.y = float.Parse(locs[1]);
                translation.z = float.Parse(locs[2]);
            }
            else
            {
                Log.Default.sendMessage("Error loading translation on line " + textReader.LineNumber + " does not contain 3 numbers value: " + textReader.Value, LogLevel.Error, "ShapeLoading");
                translation = Vector3.Zero;
            }
            return translation;
        }

        private String readName(XmlTextReader textReader)
        {
            return textReader.GetAttribute(NAME);
        }

        private float readRadius(XmlTextReader textReader)
        {
            return float.Parse(textReader.GetAttribute(RADIUS));
        }

        private float readHeight(XmlTextReader textReader)
        {
            return float.Parse(textReader.GetAttribute(HEIGHT));
        }

        private Vector3 readExtents(XmlTextReader textReader)
        {
            Vector3 extents = new Vector3();
            String[] exts = textReader.GetAttribute(EXTENTS).Split(SEPS);
            if (exts.Length == 3)
            {
                extents.x = float.Parse(exts[0]);
                extents.y = float.Parse(exts[1]);
                extents.z = float.Parse(exts[2]);
            }
            else
            {
                Log.Default.sendMessage("Error loading extents on line " + textReader.LineNumber + " does not contain 3 numbers value: " + textReader.Value, LogLevel.Error, "ShapeLoading");
            }
            return extents;
        }

        private Vector3 readPlaneNormal(XmlTextReader textReader)
        {
            Vector3 nml = new Vector3();
            String[] n = textReader.GetAttribute(NORMAL).Split(SEPS);
            if (n.Length == 3)
            {
                nml.x = float.Parse(n[0]);
                nml.y = float.Parse(n[1]);
                nml.z = float.Parse(n[2]);
            }
            else
            {
                Log.Default.sendMessage("Error loading normal on line " + textReader.LineNumber + " does not contain 3 numbers value: " + textReader.Value, LogLevel.Error, "ShapeLoading");
            }
            return nml;
        }

        private float readPlaneDistance(XmlTextReader textReader)
        {
            return float.Parse(textReader.GetAttribute(DISTANCE));
        }

        float[] readVertices(XmlTextReader textReader)
        {
            textReader.Read();
            String[] strVerts = textReader.Value.Split(SEPS);
            float[] vertices;
            float junk;
            if (float.TryParse(strVerts[strVerts.Length - 1], out junk))
            {
                vertices = new float[strVerts.Length];
            }
            else
            {
                vertices = new float[strVerts.Length - 1];
            }
            for (uint i = 0; i < vertices.Length; ++i)
            {
                vertices[i] = float.Parse(strVerts[i]);
            }
            return vertices;
        }

        int[] readFaces(XmlTextReader textReader)
        {
            textReader.Read();
            String[] strVerts = textReader.Value.Split(SEPS);
            int[] faces;
            int junk;
            if (int.TryParse(strVerts[strVerts.Length - 1], out junk))
            {
                faces = new int[strVerts.Length];
            }
            else
            {
                faces = new int[strVerts.Length - 1];
            }
            for (uint i = 0; i < faces.Length; ++i)
            {
                faces[i] = int.Parse(strVerts[i]);
            }
            return faces;
        }

        private void loadSphere(XmlTextReader textReader, ShapeBuilder builder)
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
            Log.Default.sendMessage("Creating collision sphere named " + name + ".", LogLevel.Info, "ShapeLoading");
        }

        private void loadBox(XmlTextReader textReader, ShapeBuilder builder)
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
            Log.Default.sendMessage("Creating collision box named " + name + ".", LogLevel.Info, "ShapeLoading");
        }

        private void loadMesh(XmlTextReader textReader, ShapeBuilder builder)
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
            Log.Default.sendMessage("Creating collision mesh named " + name + ".", LogLevel.Info, "ShapeLoading");
        }

        void loadPlane(XmlTextReader textReader, ShapeBuilder builder)
        {
            String name = readName(textReader);
            Vector3 normal = readPlaneNormal(textReader);
            float d = readPlaneDistance(textReader);
            builder.buildPlane(name, normal, d, currentMaterial);
            currentMaterial = null;
            Log.Default.sendMessage("Creating collision plane named " + name + ".", LogLevel.Info, "ShapeLoading");
        }

        void loadCapsule(XmlTextReader textReader, ShapeBuilder builder)
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
            Log.Default.sendMessage("Creating collision capsule named " + name + ".", LogLevel.Info, "ShapeLoading");
        }

        void loadConvexHull(XmlTextReader textReader, ShapeBuilder builder)
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
            Log.Default.sendMessage("Creating collision convex hull named " + name + ".", LogLevel.Info, "ShapeLoading");
        }

        private void loadSoftBody(XmlTextReader textReader, ShapeBuilder builder)
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
            Log.Default.sendMessage("Creating collision soft body named " + name + ".", LogLevel.Info, "ShapeLoading");
            builder.buildSoftBody(name, vertices, tetrahedra, location, rotation);
            currentMaterial = null;
        }

        void loadCompound(XmlTextReader textReader, ShapeBuilder builder)
        {
            String name = readName(textReader);
            builder.startCompound(name);
            Log.Default.sendMessage("--Started compound collision shape named " + name + ".--", LogLevel.Info, "ShapeLoading");
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
            Log.Default.sendMessage("--Finished compound collision shape named " + name + ".--", LogLevel.Info, "ShapeLoading");
        }

        private void loadMaterial(XmlTextReader textReader, ShapeBuilder builder)
        {
            currentMaterial = textReader.GetAttribute("name");
            Log.Default.sendMessage("Creating material named {0}.", LogLevel.Info, "ShapeLoading", currentMaterial);
            PhysMaterialDesc desc = new PhysMaterialDesc();
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

        private float readDynamicFriction(XmlTextReader textReader)
        {
            float result = 0.0f;
            while (!(textReader.Name == DYNAMIC_FRICTION && textReader.NodeType == XmlNodeType.EndElement) && textReader.Read())
            {
                if (textReader.NodeType == XmlNodeType.Text)
                {
                    float.TryParse(textReader.Value, out result);
                }
            }
            return result;
        }

        private float readStaticFriction(XmlTextReader textReader)
        {
            float result = 0.0f;
            while (!(textReader.Name == STATIC_FRICTION && textReader.NodeType == XmlNodeType.EndElement) && textReader.Read())
            {
                if (textReader.NodeType == XmlNodeType.Text)
                {
                    float.TryParse(textReader.Value, out result);
                }
            }
            return result;
        }

        private float readRestitution(XmlTextReader textReader)
        {
            float result = 0.0f;
            while (!(textReader.Name == RESTITUTION && textReader.NodeType == XmlNodeType.EndElement) && textReader.Read())
            {
                if (textReader.NodeType == XmlNodeType.Text)
                {
                    float.TryParse(textReader.Value, out result);
                }
            }
            return result;
        }        
    }
}
