using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Engine;
using System.Runtime.InteropServices;
using Engine.Attributes;

namespace OgrePlugin
{
    [NativeSubsystemType]
    public class SceneNode : Node
    {
#region Static

        private static SceneNode createWrapper(IntPtr ogreSceneNode, object[] args)
        {
            return new SceneNode(ogreSceneNode);
        }

        private static WrapperCollection<SceneNode> sceneNodes = new WrapperCollection<SceneNode>(SceneNode.createWrapper);

        internal static SceneNode getManagedNode(IntPtr ogreSceneNode)
        {
            return sceneNodes.getObject(ogreSceneNode);
        }

        internal static void destroyManagedNode(SceneNode sceneNode)
        {
            sceneNodes.destroyObject(sceneNode.ogreNode);
        }

#endregion

        private Dictionary<String, MovableObject> nodeObjectList = new Dictionary<String, MovableObject>();

        protected SceneNode(IntPtr ogreSceneNode)
            :base(ogreSceneNode)
        {

        }

        public override void Dispose()
        {
            base.Dispose();
        }

        /// <summary>
	    /// Add another scene node as a child to this node.
	    /// </summary>
	    /// <param name="child">The child scene node to add.</param>
        public void addChild(SceneNode child)
        {
            SceneNode_addChild(ogreNode, child.ogreNode);
        }

	    /// <summary>
	    /// Removes a child scene node.
	    /// </summary>
	    /// <param name="child">The child scene node to remove.</param>
        public void removeChild(SceneNode child)
        {
            SceneNode_removeChild(ogreNode, child.ogreNode);
        }

	    /// <summary>
	    /// Attach a MovableObject such as a Entity or a Light to this node.
	    /// </summary>
	    /// <param name="object">The object to attach to this node.</param>
        public void attachObject(MovableObject obj)
        {
            nodeObjectList.Add(obj.getName(), obj);
            SceneNode_attachObject(ogreNode, obj.OgreObject);
        }

	    /// <summary>
	    /// Detach a MovableObject such as a Entity or a Light to this node.
	    /// </summary>
	    /// <param name="object">The object to detach from this node.</param>
        public void detachObject(MovableObject obj)
        {
            nodeObjectList.Remove(obj.getName());
            SceneNode_detachObject(ogreNode, obj.OgreObject);
        }

	    /// <summary>
	    /// Get an iterator over the SceneNodeObjects on this node.
	    /// </summary>
	    /// <returns>A list of SceneNodeObjects on this node.</returns>
        public IEnumerable<MovableObject> getNodeObjectIter()
        {
            return nodeObjectList.Values;
        }

	    /// <summary>
	    /// Get the render node object specified by name.
	    /// </summary>
	    /// <param name="name">The name of the requested object.</param>
	    /// <returns>The requested MovableObject or null if it does not exist.</returns>
        public MovableObject getNodeObject(String name)
        {
            MovableObject obj = null;
            nodeObjectList.TryGetValue(name, out obj);
            return obj;
        }

	    /// <summary>
	    /// Enables / disables automatic tracking of a SceneNode.
	    /// 
	    /// Remarks:
        /// If you enable auto-tracking, this Camera will automatically rotate to look at the 
	    /// target SceneNode every frame, no matter how it or SceneNode move. This is handy if 
	    /// you want a Camera to be focused on a single object or group of objects. Note that by
	    /// default the Camera looks at the origin of the SceneNode, if you want to tweak this, 
	    /// e.g. if the object which is attached to this target node is quite big and you want 
	    /// to point the camera at a specific point on it, provide a vector in the 'offset' 
	    /// parameter and the camera's target point will be adjusted. 
	    /// </summary>
	    /// <param name="enabled">If true, the Camera will track the SceneNode supplied as the next parameter (cannot be null). If false the camera will cease tracking and will remain in it's current orientation.</param>
	    /// <param name="target">Pointer to the SceneNode which this Camera will track. Make sure you don't delete this SceneNode before turning off tracking (e.g. SceneManager::clearScene will delete it so be careful of this). Can be null if and only if the enabled param is false.</param>
	    /// <param name="offset">If supplied, the camera targets this point in local space of the target node instead of the origin of the target node. Good for fine tuning the look at point.</param>
        public void setAutoTracking(bool enabled, SceneNode target, Vector3 offset)
        {
            SceneNode_setAutoTracking(ogreNode, enabled, target.ogreNode, offset);
        }


	    /// <summary>
	    /// Makes all objects attached to this node become visible / invisible and
        /// all child nodes.
	    /// </summary>
	    /// <param name="visible">Whether the objects are to be made visible or invisible.</param>
        public void setVisible(bool visible)
        {
            SceneNode_setVisible(ogreNode, visible);
        }

	    /// <summary>
	    /// Makes all objects attached to this node become visible / invisible. 
	    /// </summary>
	    /// <param name="visible">Whether the objects are to be made visible or invisible.</param>
	    /// <param name="cascade">If true, this setting cascades into child nodes too.</param>
        public void setVisible(bool visible, bool cascade)
        {
            SceneNode_setVisibleCascade(ogreNode, visible, cascade);
        }

        public void lookAt(Vector3 targetPoint, TransformSpace relativeTo)
        {
            SceneNode_lookAt(ogreNode, targetPoint, relativeTo);
        }

        public void lookAt(Vector3 targetPoint, TransformSpace relativeTo, Vector3 localDirectionVector)
        {
            SceneNode_lookAtLocalDirection(ogreNode, targetPoint, relativeTo, localDirectionVector);
        }

        public void setDebugDisplayEnabled(bool enabled, bool cascade)
        {
            SceneNode_setDebugDisplayEnabled(ogreNode, enabled, cascade);
        }

        public void showBoundingBox(bool show)
        {
            SceneNode_showBoundingBox(ogreNode, show);
        }

        public object UserObject { get; set; }

        #region PInvoke

        [DllImport(LibraryInfo.Name, CallingConvention=CallingConvention.Cdecl)]
        private static extern void SceneNode_addChild(IntPtr sceneNode, IntPtr child);

        [DllImport(LibraryInfo.Name, CallingConvention=CallingConvention.Cdecl)]
        private static extern void SceneNode_removeChild(IntPtr sceneNode, IntPtr child);

        [DllImport(LibraryInfo.Name, CallingConvention=CallingConvention.Cdecl)]
        private static extern void SceneNode_attachObject(IntPtr sceneNode, IntPtr obj);

        [DllImport(LibraryInfo.Name, CallingConvention=CallingConvention.Cdecl)]
        private static extern void SceneNode_detachObject(IntPtr sceneNode, IntPtr obj);

        [DllImport(LibraryInfo.Name, CallingConvention=CallingConvention.Cdecl)]
        private static extern void SceneNode_setAutoTracking(IntPtr sceneNode, bool enabled, IntPtr target, Vector3 offset);

        [DllImport(LibraryInfo.Name, CallingConvention=CallingConvention.Cdecl)]
        private static extern void SceneNode_setVisible(IntPtr sceneNode, bool visible);

        [DllImport(LibraryInfo.Name, CallingConvention=CallingConvention.Cdecl)]
        private static extern void SceneNode_setVisibleCascade(IntPtr sceneNode, bool visible, bool cascade);

        [DllImport(LibraryInfo.Name, CallingConvention=CallingConvention.Cdecl)]
        private static extern void SceneNode_lookAt(IntPtr sceneNode, Vector3 targetPoint, TransformSpace relativeTo);

        [DllImport(LibraryInfo.Name, CallingConvention=CallingConvention.Cdecl)]
        private static extern void SceneNode_lookAtLocalDirection(IntPtr sceneNode, Vector3 targetPoint, TransformSpace relativeTo, Vector3 localDirectionVector);

        [DllImport(LibraryInfo.Name, CallingConvention=CallingConvention.Cdecl)]
        private static extern void SceneNode_setDebugDisplayEnabled(IntPtr sceneNode, bool enabled, bool cascade);

        [DllImport(LibraryInfo.Name, CallingConvention=CallingConvention.Cdecl)]
        private static extern void SceneNode_showBoundingBox(IntPtr sceneNode, bool show);

        #endregion
    }
}
