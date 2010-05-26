using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Engine.Attributes;
using Engine;
using System.Runtime.InteropServices;

namespace OgreWrapper
{
    public abstract class Node : IDisposable
    {
        [SingleEnum]
        public enum TransformSpace : uint
        {
	        TS_LOCAL,
	        TS_PARENT,
	        TS_WORLD
        };

        protected IntPtr ogreNode;

        public Node(IntPtr ogreNode)
        {
            this.ogreNode = ogreNode;
        }

        public virtual void Dispose()
        {
            ogreNode = IntPtr.Zero;
        }

        /// <summary>
        /// Returns a quaternion representing the nodes orientation. 
        /// </summary>
        /// <returns>A quaternion.</returns>
        public Quaternion getOrientation()
        {
            return Node_getOrientation(ogreNode);
        }

        /// <summary>
        /// Sets the orientation of this node via a quaternion. 
        /// 
        /// Orientations, unlike other transforms, are not always inherited by child
        /// nodes. Whether or not orientations affect the orientation of the child
        /// nodes depends on the setInheritOrientation option of the child. In some
        /// cases you want a orientating of a parent node to apply to a child node
        /// (e.g. where the child node is a part of the same object, so you want it
        /// to be the same relative orientation based on the parent's orientation),
        /// but not in other cases (e.g. where the child node is just for
        /// positioning another object, you want it to maintain it's own
        /// orientation). The default is to inherit as with other transforms. 
        /// 
        /// Note that rotations are oriented around the node's origin. 
        /// </summary>
        /// <param name="q">The orientation to set.</param>
        public void setOrientation(Quaternion q)
        {
            Node_setOrientation(ogreNode, q);
        }

        /// <summary>
        /// Sets the orientation of this node via quaternion parameters. 
        /// </summary>
        /// <param name="x">x</param>
        /// <param name="y">y</param>
        /// <param name="z">z</param>
        /// <param name="w">w</param>
        public void setOrientation(float x, float y, float z, float w)
        {
            Node_setOrientationRaw(ogreNode, x, y, z, w);
        }

        /// <summary>
        /// Resets the nodes orientation (local axes as world axes, no rotation). 
        /// </summary>
        public void resetOrientation()
        {
            Node_resetOrientation(ogreNode);
        }

        /// <summary>
        /// Sets the position of the node relative to it's parent. 
        /// </summary>
        /// <param name="pos">The position to set.</param>
        public void setPosition(Vector3 pos)
        {
            Node_setPosition(ogreNode, pos);
        }

        /// <summary>
        /// Gets the position of the node relative to it's parent.
        /// </summary>
        /// <returns>A Vector3.</returns>
        public Vector3 getPosition()
        {
            return Node_getPosition(ogreNode);
        }

        /// <summary>
        /// Sets the scaling factor applied to this node.
        ///
        /// Scaling factors, unlike other transforms, are not always inherited by
        /// child nodes. Whether or not scalings affect the size of the child nodes
        /// depends on the setInheritScale option of the child. In some cases you
        /// want a scaling factor of a parent node to apply to a child node (e.g.
        /// where the child node is a part of the same object, so you want it to be
        /// the same relative size based on the parent's size), but not in other
        /// cases (e.g. where the child node is just for positioning another object,
        /// you want it to maintain it's own size). The default is to inherit as
        /// with other transforms. 
        /// 
        /// Note that like rotations, scalings are oriented around the node's
        /// origin. 
        /// </summary>
        /// <param name="scale">The scale to set.</param>
        public void setScale(Vector3 scale)
        {
            Node_setScale(ogreNode, scale);
        }

        /// <summary>
        /// Gets the scaling factor of this node. 
        /// </summary>
        /// <returns>The scaling factor.</returns>
        public Vector3 getScale()
        {
            return Node_getScale(ogreNode);
        }

        /// <summary>
        /// Tells the node whether it should inherit orientation from it's parent
        /// node.
        /// 
        /// Orientations, unlike other transforms, are not always inherited by child
        /// nodes. Whether or not orientations affect the orientation of the child
        /// nodes depends on the setInheritOrientation option of the child. In some
        /// cases you want a orientating of a parent node to apply to a child node
        /// (e.g. where the child node is a part of the same object, so you want it
        /// to be the same relative orientation based on the parent's orientation),
        /// but not in other cases (e.g. where the child node is just for
        /// positioning another object, you want it to maintain it's own
        /// orientation). The default is to inherit as with other transforms.
        /// </summary>
        /// <param name="inherit">If true, this node's orientation will be affected by its parent's orientation. If false, it will not be affected.</param>
        public void setInheritOrientation(bool inherit)
        {
            Node_setInheritOrientation(ogreNode, inherit);
        }

        /// <summary>
        /// Determine if this node inherits its orientation.
        /// </summary>
        /// <returns>Returns true if this node is affected by orientation applied to the parent node.</returns>
        public bool getInheritOrientation()
        {
            return Node_getInheritOrientation(ogreNode);
        }

        /// <summary>
        /// Tells the node whether it should inherit scaling factors from it's
        /// parent node.
        /// 
        /// Scaling factors, unlike other transforms, are not always inherited by
        /// child nodes. Whether or not scalings affect the size of the child nodes
        /// depends on the setInheritScale option of the child. In some cases you
        /// want a scaling factor of a parent node to apply to a child node (e.g.
        /// where the child node is a part of the same object, so you want it to be
        /// the same relative size based on the parent's size), but not in other
        /// cases (e.g. where the child node is just for positioning another object,
        /// you want it to maintain it's own size). The default is to inherit as
        /// with other transforms.
        /// </summary>
        /// <param name="inherit">If true, this node's scale will be affected by its parent's scale. If false, it will not be affected.</param>
        public void setInheritScale(bool inherit)
        {
            Node_setInheritScale(ogreNode, inherit);
        }

        /// <summary>
        /// Returns true if this node is affected by scaling factors applied to the parent node.
        /// </summary>
        /// <returns></returns>
        public bool getInheritScale()
        {
            return Node_getInheritScale(ogreNode);
        }

        /// <summary>
        /// Scales the node, combining it's current scale with the passed in scaling
        /// factor.
        /// 
        /// This method applies an extra scaling factor to the node's existing
        /// scale, (unlike setScale which overwrites it) combining it's current
        /// scale with the new one. E.g. calling this method twice with
        /// Vector3(2,2,2) would have the same effect as setScale(Vector3(4,4,4)) if
        /// the existing scale was 1. 
        /// 
        /// Note that like rotations, scalings are oriented around the node's
        /// origin. 
        /// </summary>
        /// <param name="scale">The scale to set.</param>
        public void scale(Vector3 scale)
        {
            Node_scale(ogreNode, scale);
        }

        /// <summary>
        /// Scales the node, combining it's current scale with the passed in scaling
        /// factor.
        ///
        /// See the Vector3 version of this function for more info.
        /// </summary>
        /// <param name="x">x</param>
        /// <param name="y">y</param>
        /// <param name="z">z</param>
        public void scale(float x, float y, float z)
        {
            Node_scaleRaw(ogreNode, x, y, z);
        }

        /// <summary>
        /// Moves the node along the Cartesian axes.
        /// </summary>
        /// <param name="d">The distance to translate.</param>
        /// <param name="relativeTo">The transform space to use.</param>
        public void translate(Vector3 d, TransformSpace relativeTo)
        {
            Node_translate(ogreNode, d, relativeTo);
        }

        /// <summary>
        /// Moves the node along the Cartesian axes. 
        /// </summary>
        /// <param name="x">x</param>
        /// <param name="y">y</param>
        /// <param name="z">z</param>
        /// <param name="relativeTo">The transform space to use.</param>
        public void translate(float x, float y, float z, TransformSpace relativeTo)
        {
            Node_translateRaw(ogreNode, x, y, z, relativeTo);
        }

        /// <summary>
        /// Rotate the node around the Z-axis. 
        /// </summary>
        /// <param name="angle">The amount to rotate.</param>
        /// <param name="relativeTo">The transform space to use.</param>
        public void roll(float angle, TransformSpace relativeTo)
        {
            Node_roll(ogreNode, angle, relativeTo);
        }

        /// <summary>
        /// Rotate the node around the X-axis.
        /// </summary>
        /// <param name="angle">The amount to rotate.</param>
        /// <param name="relativeTo">The transform space to use.</param>
        public void pitch(float angle, TransformSpace relativeTo)
        {
            Node_pitch(ogreNode, angle, relativeTo);
        }

        /// <summary>
        /// Rotate the node around the Y-axis. 
        /// </summary>
        /// <param name="angle">The amount to rotate.</param>
        /// <param name="relativeTo">The transform space to use.</param>
        public void yaw(float angle, TransformSpace relativeTo)
        {
            Node_yaw(ogreNode, angle, relativeTo);
        }

        /// <summary>
        /// Rotate the node around an arbitrary axis. 
        /// </summary>
        /// <param name="axis">The axis to rotate around.</param>
        /// <param name="angle">The amount to rotate.</param>
        /// <param name="relativeTo">The transform space to use.</param>
        public void rotate(Vector3 axis, float angle, TransformSpace relativeTo)
        {
            Node_rotateAxis(ogreNode, axis, angle, relativeTo);
        }

        /// <summary>
        /// Rotate the node around an arbitrary axis. 
        /// </summary>
        /// <param name="q">The quaternion to rotate by.</param>
        /// <param name="relativeTo">The transform space to use.</param>
        public void rotate(Quaternion q, TransformSpace relativeTo)
        {
            Node_rotate(ogreNode, q, relativeTo);
        }

        /// <summary>
        /// Gets the position of the node as derived from all parents. 
        /// </summary>
        /// <returns>The derived position.</returns>
        public Vector3 getDerivedPosition()
        {
            return Node_getDerivedPosition(ogreNode);
        }

        /// <summary>
        /// Gets the scaling factor of the node as derived from all parents. 
        /// </summary>
        /// <returns>The derived scale.</returns>
        public Vector3 getDerivedScale()
        {
            return Node_getDerivedScale(ogreNode);
        }

        /// <summary>
        /// Gets the orientation of the node as derived from all parents. 
        /// </summary>
        /// <returns>The derived orientation.</returns>
        public Quaternion getDerivedOrientation()
        {
            return Node_getDerivedOrientation(ogreNode);
        }

        #region PInvoke

        [DllImport("OgreCWrapper")]
        private static extern Quaternion Node_getOrientation(IntPtr ogreNode);

        [DllImport("OgreCWrapper")]
        private static extern void Node_setOrientation(IntPtr ogreNode, Quaternion q);

        [DllImport("OgreCWrapper")]
        private static extern void Node_setOrientationRaw(IntPtr ogreNode, float x, float y, float z, float w);

        [DllImport("OgreCWrapper")]
        private static extern void Node_resetOrientation(IntPtr ogreNode);

        [DllImport("OgreCWrapper")]
        private static extern void Node_setPosition(IntPtr ogreNode, Vector3 pos);

        [DllImport("OgreCWrapper")]
        private static extern Vector3 Node_getPosition(IntPtr ogreNode);

        [DllImport("OgreCWrapper")]
        private static extern void Node_setScale(IntPtr ogreNode, Vector3 scale);

        [DllImport("OgreCWrapper")]
        private static extern Vector3 Node_getScale(IntPtr ogreNode);

        [DllImport("OgreCWrapper")]
        private static extern void Node_setInheritOrientation(IntPtr ogreNode, bool inherit);

        [DllImport("OgreCWrapper")]
        private static extern bool Node_getInheritOrientation(IntPtr ogreNode);

        [DllImport("OgreCWrapper")]
        private static extern void Node_setInheritScale(IntPtr ogreNode, bool inherit);

        [DllImport("OgreCWrapper")]
        private static extern bool Node_getInheritScale(IntPtr ogreNode);

        [DllImport("OgreCWrapper")]
        private static extern void Node_scale(IntPtr ogreNode, Vector3 scale);

        [DllImport("OgreCWrapper")]
        private static extern void Node_scaleRaw(IntPtr ogreNode, float x, float y, float z);

        [DllImport("OgreCWrapper")]
        private static extern void Node_translate(IntPtr ogreNode, Vector3 d, TransformSpace relativeTo);

        [DllImport("OgreCWrapper")]
        private static extern void Node_translateRaw(IntPtr ogreNode, float x, float y, float z, TransformSpace relativeTo);

        [DllImport("OgreCWrapper")]
        private static extern void Node_roll(IntPtr ogreNode, float angle, TransformSpace relativeTo);

        [DllImport("OgreCWrapper")]
        private static extern void Node_pitch(IntPtr ogreNode, float angle, TransformSpace relativeTo);

        [DllImport("OgreCWrapper")]
        private static extern void Node_yaw(IntPtr ogreNode, float angle, TransformSpace relativeTo);

        [DllImport("OgreCWrapper")]
        private static extern void Node_rotateAxis(IntPtr ogreNode, Vector3 axis, float angle, TransformSpace relativeTo);

        [DllImport("OgreCWrapper")]
        private static extern void Node_rotate(IntPtr ogreNode, Quaternion q, TransformSpace relativeTo);

        [DllImport("OgreCWrapper")]
        private static extern Vector3 Node_getDerivedPosition(IntPtr ogreNode);

        [DllImport("OgreCWrapper")]
        private static extern Vector3 Node_getDerivedScale(IntPtr ogreNode);

        [DllImport("OgreCWrapper")]
        private static extern Quaternion Node_getDerivedOrientation(IntPtr ogreNode);

        #endregion 
    }
}
