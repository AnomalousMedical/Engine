using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Engine.Attributes;
using System.Runtime.InteropServices;
using Engine;

namespace OgrePlugin
{
    [SingleEnum]
    public enum OperationType : int
    {
	    OT_POINT_LIST = 1,
	    OT_LINE_LIST = 2,
	    OT_LINE_STRIP = 3,
	    OT_TRIANGLE_LIST = 4,
	    OT_TRIANGLE_STRIP = 5,
	    OT_TRIANGLE_FAN = 6
    };

    [NativeSubsystemType]
    public class ManualObject : MovableObject
    {
        WrapperCollection<ManualObjectSection> sections = new WrapperCollection<ManualObjectSection>(ManualObjectSection.createWrapper);

        internal static ManualObject createWrapper(IntPtr manualObject, object[] args)
        {
            return new ManualObject(manualObject);
        }

        private ManualObject(IntPtr manualObject)
            :base(manualObject)
        {

        }

        public override void Dispose()
        {
            base.Dispose();
            sections.Dispose();
        }

        /// <summary>
	    /// Completely clear the contents of the object. 
	    /// </summary>
        public void clear()
        {
            ManualObject_clear(ogreObject);
        }

	    /// <summary>
	    /// Estimate the number of vertices ahead of time.
	    /// </summary>
	    /// <param name="count">The number of vertices to predict.</param>
        public void estimateVertexCount(uint count)
        {
            ManualObject_estimateVertexCount(ogreObject, count);
        }

	    /// <summary>
	    /// Estimate the number of indices ahead of time. 
	    /// </summary>
	    /// <param name="count">The number of indicies to predict.</param>
        public void estimateIndexCount(uint count)
        {
            ManualObject_estimateIndexCount(ogreObject, count);
        }

	    /// <summary>
	    /// Start defining a part of the object. 
	    /// </summary>
	    /// <param name="materialName">The name of the material to use.</param>
	    /// <param name="opType">The type of object to define.</param>
        public void begin(String materialName, OperationType opType)
        {
            ManualObject_begin(ogreObject, materialName, opType);
        }

	    /// <summary>
	    /// Use before defining geometry to indicate that you intend to update the geometry 
	    /// regularly and want the internal structure to reflect that.
	    /// </summary>
	    /// <param name="dyn">True to indicate dynamic geometry.</param>
        public void setDynamic(bool dyn)
        {
            ManualObject_setDynamic(ogreObject, dyn);
        }

	    /// <summary>
	    /// Gets whether this object is marked as dynamic. 
	    /// </summary>
	    /// <returns>True if the object is dynamic.  False if it is not.</returns>
        public bool getDynamic()
        {
            return ManualObject_getDynamic(ogreObject);
        }

	    /// <summary>
	    /// Start the definition of an update to a part of the object. 
	    /// </summary>
	    /// <param name="sectionIndex">The section to update.</param>
        public void beginUpdate(uint sectionIndex)
        {
            ManualObject_beginUpdate(ogreObject, sectionIndex);
        }

	    /// <summary>
	    /// Add a vertex position, starting a new vertex at the same time. 
	    /// </summary>
	    /// <param name="pos">A vector3 with the position of the vertex.</param>
        public void position(ref Vector3 pos)
        {
            ManualObject_positionRef(ogreObject, ref pos);
        }

	    /// <summary>
	    /// Add a vertex position, starting a new vertex at the same time. 
	    /// </summary>
	    /// <param name="pos">A vector3 with the position of the vertex.</param>
        public void position(Vector3 pos)
        {
            ManualObject_position(ogreObject, pos);
        }

	    /// <summary>
	    /// Add a vertex position, starting a new vertex at the same time.
	    /// A vertex position is slightly special among the other vertex data methods like normal() 
	    /// and textureCoord(), since calling it indicates the start of a new vertex. All other 
	    /// vertex data methods you call after this are assumed to be adding more information 
	    /// (like normals or texture coordinates) to the last vertex started with position(). 
	    /// </summary>
	    /// <param name="x">x</param>
	    /// <param name="y">y</param>
	    /// <param name="z">z</param>
        public void position(float x, float y, float z)
        {
            ManualObject_positionRaw(ogreObject, x, y, z);
        }

	    /// <summary>
	    /// Add a vertex normal to the current vertex. 
	    /// </summary>
	    /// <param name="normal">A vector3 with the normal.</param>
        public void normal(ref Vector3 normal)
        {
            ManualObject_normalRef(ogreObject, ref normal);
        }

	    /// <summary>
	    /// Add a vertex normal to the current vertex. 
	    /// </summary>
	    /// <param name="normal">A vector3 with the normal.</param>
        public void normal(Vector3 normal)
        {
            ManualObject_normal(ogreObject, normal);
        }

	    /// <summary>
	    /// Add a vertex normal to the current vertex. 
	    /// Vertex normals are most often used for dynamic lighting, and their
	    /// components should be normalised. 
	    /// </summary>
	    /// <param name="x">x</param>
	    /// <param name="y">y</param>
	    /// <param name="z">z</param>
        public void normal(float x, float y, float z)
        {
            ManualObject_normalRaw(ogreObject, x, y, z);
        }

	    /// <summary>
	    /// Add a texture coordinate to the current vertex. 
	    /// </summary>
	    /// <param name="u"></param>
        public void textureCoord(float u)
        {
            ManualObject_textureCoordU(ogreObject, u);
        }

	    /// <summary>
	    /// Add a texture coordinate to the current vertex.
	    /// You can call this method multiple times between position() calls to add multiple texture 
	    /// coordinates to a vertex. Each one can have between 1 and 3 dimensions, depending on your 
	    // needs, although 2 is most common. There are several versions of this method for the 
	    /// variations in number of dimensions.
	    /// </summary>
	    /// <param name="u"></param>
	    /// <param name="v"></param>
        public void textureCoord(float u, float v)
        {
            ManualObject_textureCoordUV(ogreObject, u, v);
        }

	    /// <summary>
	    /// Add a texture coordinate to the current vertex.
	    /// </summary>
	    /// <param name="u">u</param>
	    /// <param name="v">v</param>
	    /// <param name="w">w</param>
        public void textureCoord(float u, float v, float w)
        {
            ManualObject_textureCoordUVW(ogreObject, u, v, w);
        }

	    /// <summary>
	    /// Add a texture coordinate to the current vertex.
	    /// </summary>
	    /// <param name="x">x</param>
	    /// <param name="y">y</param>
	    /// <param name="z">z</param>
	    /// <param name="w">w</param>
        public void textureCoord(float x, float y, float z, float w)
        {
            ManualObject_textureCoordRaw(ogreObject, x, y, z, w);
        }

	    /// <summary>
	    /// Add a texture coordinate to the current vertex.
	    /// </summary>
	    /// <param name="uvw">A Vector3 with the coord.</param>
        public void textureCoord(ref Vector3 uvw)
        {
            ManualObject_textureCoord(ogreObject, ref uvw);
        }

	    /// <summary>
	    /// Add a vertex colour to a vertex. 
	    /// </summary>
	    /// <param name="r">r</param>
	    /// <param name="g">g</param>
	    /// <param name="b">b</param>
	    /// <param name="a">a</param>
        public void color(float r, float g, float b, float a)
        {
            ManualObject_color(ogreObject, r, g, b, a);
        }

	    /// <summary>
	    /// Add a vertex index to construct faces / lines / points via indexing rather than just by 
	    /// a simple list of vertices.  You will have to call this 3 times for each face for a triangle 
	    /// list, or use the alternative 3-parameter version. Other operation types require different 
	    /// numbers of indexes.
	    /// </summary>
	    /// <param name="idx">A vertex index from 0 to 4294967295.</param>
        public void index(uint idx)
        {
            ManualObject_index(ogreObject, idx);
        }

	    /// <summary>
	    /// Add a set of 3 vertex indices to construct a triangle; this is a shortcut to calling index() 
	    /// 3 times. 
	    /// </summary>
	    /// <param name="i1">First index.</param>
	    /// <param name="i2">Second index.</param>
	    /// <param name="i3">Third index.</param>
        public void triangle(uint i1, uint i2, uint i3)
        {
            ManualObject_triangle(ogreObject, i1, i2, i3);
        }

	    /// <summary>
	    /// Add a set of 4 vertex indices to construct a quad (out of 2 triangles); this is a shortcut 
	    /// to calling index() 6 times, or triangle() twice. 
	    /// </summary>
	    /// <param name="i1">First index.</param>
	    /// <param name="i2">Second index.</param>
	    /// <param name="i3">Third index.</param>
	    /// <param name="i4">Fourth index.</param>
        public void quad(uint i1, uint i2, uint i3, uint i4)
        {
            ManualObject_quad(ogreObject, i1, i2, i3, i4);
        }

	    /// <summary>
	    /// Finish defining the object and compile the final renderable version.
	    /// </summary>
        public ManualObjectSection end()
        {
            return sections.getObject(ManualObject_end(ogreObject));
        }

	    /// <summary>
	    /// Alter the material for a subsection of this object after it has been specified. 
	    /// </summary>
	    /// <param name="subindex">The index of the subsection to alter.</param>
	    /// <param name="name">	The name of the new material to use.</param>
        public void setMaterialName(uint subindex, String name)
        {
            ManualObject_setMaterialName(ogreObject, subindex, name);
        }

	    /// <summary>
	    /// Sets whether or not to use an 'identity' projection.  Usually ManualObjects will use a 
	    /// projection matrix as determined by the active camera. However, if they want they can cancel 
	    /// this out and use an identity projection, which effectively projects in 2D using a {-1, 1} 
	    /// view space. Useful for overlay rendering. Normally you don't need to change this. The 
	    /// default is false.
	    /// </summary>
	    /// <param name="useIdentityProjection">True to use the identity projection.</param>
        public void setUseIdentityProjection(bool useIdentityProjection)
        {
            ManualObject_setUseIdentityProjection(ogreObject, useIdentityProjection);
        }

	    /// <summary>
	    /// Returns whether or not to use an 'identity' projection. 
	    /// </summary>
	    /// <returns>True if the identity projection is being used.  False if it is not.</returns>
        public bool getUseIdentityProjection()
        {
            return ManualObject_getUseIdentityProjection(ogreObject);
        }

	    /// <summary>
	    /// Sets whether or not to use an 'identity' view.  Usually ManualObjects will use a view 
	    /// matrix as determined by the active camera. However, if they want they can cancel this out 
	    /// and use an identity matrix, which means all geometry is assumed to be relative to camera 
	    /// space already. Useful for overlay rendering. Normally you don't need to change this. The 
	    /// default is false.
	    /// </summary>
	    /// <param name="useIdentityView">True to use the identity view matrix.</param>
        public void setUseIdentityView(bool useIdentityView)
        {
            ManualObject_setUseIdentityView(ogreObject, useIdentityView);
        }

	    /// <summary>
	    /// Returns whether or not to use an 'identity' view. 
	    /// </summary>
	    /// <returns>True if the identity view is being used.</returns>
        public bool getUseIdentityView()
        {
            return ManualObject_getUseIdentityView(ogreObject);
        }

	    /// <summary>
	    /// Gets a reference to a ManualObjectSection.
	    /// </summary>
	    /// <param name="index">The index of the section to get.</param>
	    /// <returns>The section at index or null if no section is at that index.</returns>
        public ManualObjectSection getSection(uint index)
        {
            return sections.getObject(ManualObject_getSection(ogreObject, index));
        }

	    /// <summary>
	    /// Retrieves the number of ManualObjectSection objects making up this ManualObject. 
	    /// </summary>
	    /// <returns>The number of sections.</returns>
        public uint getNumSections()
        {
            return ManualObject_getNumSections(ogreObject);
        }

	    /// <summary>
	    /// Sets whether or not to keep the original declaration order when queuing the renderables. 
	    /// </summary>
	    /// <param name="keepOrder">Whether to keep the declaration order or not.  This overrides the 
	    /// default behavior of the rendering queue, specifically stating the desired order of 
	    /// rendering. Might result in a performance loss, but lets the user to have more direct 
	    /// control when creating geometry through this class.</param>
        public void setKeepDeclarationOrder(bool keepOrder)
        {
            ManualObject_setKeepDeclarationOrder(ogreObject, keepOrder);
        }

	    /// <summary>
	    /// Gets whether or not the declaration order is to be kept or not. 
	    /// </summary>
	    /// <returns>True if the declaration order is being maintained.</returns>
        public bool getKeepDeclarationOrder()
        {
            return ManualObject_getKeepDeclarationOrder(ogreObject);
        }

	    /// <summary>
	    /// Retrieves the radius of the origin-centered bounding sphere for this object.
	    /// </summary>
	    /// <returns>The bounding radius.</returns>
        public float getBoundingRadius()
        {
            return ManualObject_getBoundingRadius(ogreObject);
        }

        #region PInvoke

        [DllImport(LibraryInfo.Name, CallingConvention=CallingConvention.Cdecl)]
        private static extern void ManualObject_clear(IntPtr manualObject);

	    [DllImport(LibraryInfo.Name, CallingConvention=CallingConvention.Cdecl)]
        private static extern void ManualObject_estimateVertexCount(IntPtr manualObject, uint count);

	    [DllImport(LibraryInfo.Name, CallingConvention=CallingConvention.Cdecl)]
        private static extern void ManualObject_estimateIndexCount(IntPtr manualObject, uint count);

	    [DllImport(LibraryInfo.Name, CallingConvention=CallingConvention.Cdecl)]
        private static extern void ManualObject_begin(IntPtr manualObject, String materialName, OperationType opType);

	    [DllImport(LibraryInfo.Name, CallingConvention=CallingConvention.Cdecl)]
        private static extern void ManualObject_setDynamic(IntPtr manualObject, bool dyn);

	    [DllImport(LibraryInfo.Name, CallingConvention=CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.I1)]
        private static extern bool ManualObject_getDynamic(IntPtr manualObject);

	    [DllImport(LibraryInfo.Name, CallingConvention=CallingConvention.Cdecl)]
        private static extern void ManualObject_beginUpdate(IntPtr manualObject, uint sectionIndex);

	    [DllImport(LibraryInfo.Name, CallingConvention=CallingConvention.Cdecl)]
        private static extern void ManualObject_positionRef(IntPtr manualObject, ref Vector3 pos);

	    [DllImport(LibraryInfo.Name, CallingConvention=CallingConvention.Cdecl)]
        private static extern void ManualObject_position(IntPtr manualObject, Vector3 pos);

	    [DllImport(LibraryInfo.Name, CallingConvention=CallingConvention.Cdecl)]
        private static extern void ManualObject_positionRaw(IntPtr manualObject, float x, float y, float z);

	    [DllImport(LibraryInfo.Name, CallingConvention=CallingConvention.Cdecl)]
        private static extern void ManualObject_normalRef(IntPtr manualObject, ref Vector3 normal);

	    [DllImport(LibraryInfo.Name, CallingConvention=CallingConvention.Cdecl)]
        private static extern void ManualObject_normal(IntPtr manualObject, Vector3 normal);

	    [DllImport(LibraryInfo.Name, CallingConvention=CallingConvention.Cdecl)]
        private static extern void ManualObject_normalRaw(IntPtr manualObject, float x, float y, float z);

	    [DllImport(LibraryInfo.Name, CallingConvention=CallingConvention.Cdecl)]
        private static extern void ManualObject_textureCoordU(IntPtr manualObject, float u);

	    [DllImport(LibraryInfo.Name, CallingConvention=CallingConvention.Cdecl)]
        private static extern void ManualObject_textureCoordUV(IntPtr manualObject, float u, float v);

	    [DllImport(LibraryInfo.Name, CallingConvention=CallingConvention.Cdecl)]
        private static extern void ManualObject_textureCoordUVW(IntPtr manualObject, float u, float v, float w);

	    [DllImport(LibraryInfo.Name, CallingConvention=CallingConvention.Cdecl)]
        private static extern void ManualObject_textureCoordRaw(IntPtr manualObject, float x, float y, float z, float w);

	    [DllImport(LibraryInfo.Name, CallingConvention=CallingConvention.Cdecl)]
        private static extern void ManualObject_textureCoord(IntPtr manualObject, ref Vector3 uvw);

	    [DllImport(LibraryInfo.Name, CallingConvention=CallingConvention.Cdecl)]
        private static extern void ManualObject_color(IntPtr manualObject, float r, float g, float b, float a);

	    [DllImport(LibraryInfo.Name, CallingConvention=CallingConvention.Cdecl)]
        private static extern void ManualObject_index(IntPtr manualObject, uint idx);

	    [DllImport(LibraryInfo.Name, CallingConvention=CallingConvention.Cdecl)]
        private static extern void ManualObject_triangle(IntPtr manualObject, uint i1, uint i2, uint i3);

	    [DllImport(LibraryInfo.Name, CallingConvention=CallingConvention.Cdecl)]
        private static extern void ManualObject_quad(IntPtr manualObject, uint i1, uint i2, uint i3, uint i4);

	    [DllImport(LibraryInfo.Name, CallingConvention=CallingConvention.Cdecl)]
        private static extern IntPtr ManualObject_end(IntPtr manualObject);

	    [DllImport(LibraryInfo.Name, CallingConvention=CallingConvention.Cdecl)]
        private static extern void ManualObject_setMaterialName(IntPtr manualObject, uint subindex, String name);

	    [DllImport(LibraryInfo.Name, CallingConvention=CallingConvention.Cdecl)]
        private static extern void ManualObject_setUseIdentityProjection(IntPtr manualObject, bool useIdentityProjection);

	    [DllImport(LibraryInfo.Name, CallingConvention=CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.I1)]
        private static extern bool ManualObject_getUseIdentityProjection(IntPtr manualObject);

	    [DllImport(LibraryInfo.Name, CallingConvention=CallingConvention.Cdecl)]
        private static extern void ManualObject_setUseIdentityView(IntPtr manualObject, bool useIdentityView);

	    [DllImport(LibraryInfo.Name, CallingConvention=CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.I1)]
        private static extern bool ManualObject_getUseIdentityView(IntPtr manualObject);

	    [DllImport(LibraryInfo.Name, CallingConvention=CallingConvention.Cdecl)]
        private static extern IntPtr ManualObject_getSection(IntPtr manualObject, uint index);

	    [DllImport(LibraryInfo.Name, CallingConvention=CallingConvention.Cdecl)]
        private static extern uint ManualObject_getNumSections(IntPtr manualObject);

	    [DllImport(LibraryInfo.Name, CallingConvention=CallingConvention.Cdecl)]
        private static extern void ManualObject_setKeepDeclarationOrder(IntPtr manualObject, bool keepOrder);

	    [DllImport(LibraryInfo.Name, CallingConvention=CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.I1)]
        private static extern bool ManualObject_getKeepDeclarationOrder(IntPtr manualObject);

	    [DllImport(LibraryInfo.Name, CallingConvention=CallingConvention.Cdecl)]
        private static extern float ManualObject_getBoundingRadius(IntPtr manualObject);

        #endregion 
    }
}
