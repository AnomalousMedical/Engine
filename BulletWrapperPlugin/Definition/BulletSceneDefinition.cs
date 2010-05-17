using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Engine.ObjectManagement;
using Engine.Editing;
using Engine.Reflection;
using Engine.Saving;
using Engine;

namespace BulletPlugin
{
    public class BulletSceneDefinition : SimElementManagerDefinition
    {
        static MemberScanner memberScanner = new MemberScanner();

        static BulletSceneDefinition()
        {
            memberScanner.ProcessFields = false;
            memberScanner.Filter = new EditableAttributeFilter();
        }

        private String name;
        internal BulletSceneInfo sceneInfo;
        EditInterface editInterface;

        public BulletSceneDefinition(String name)
        {
            this.name = name;
            this.sceneInfo = new BulletSceneInfo();
        }

        public void Dispose()
        {
            
        }

        public EditInterface getEditInterface()
        {
            if(editInterface == null)
	        {
		        editInterface = ReflectedEditInterface.createEditInterface(this, memberScanner, name + " - Bullet Scene", null);
	        }
	        return editInterface;
        }

        public SimElementManager createSimElementManager()
        {
            return BulletInterface.Instance.createScene(this);
        }

        public string Name
        {
            get { return name; }
        }

        public Type getSimElementManagerType()
        {
            return typeof(BulletSceneDefinition);
        }

        [Editable]
        public Vector3 WorldAabbMin
	    {
		    get
		    {
			    return sceneInfo.worldAabbMin;
		    }
		    set
		    {
                sceneInfo.worldAabbMin = value;
		    }
	    }

	    [Editable]
        public Vector3 WorldAabbMax
	    {
		    get
		    {
                return sceneInfo.worldAabbMax;
		    }
		    set
		    {
                sceneInfo.worldAabbMax = value;
		    }
	    }

	    [Editable]
        public int MaxProxies
	    {
		    get
		    {
                return sceneInfo.maxProxies;
		    }
		    set
		    {
                sceneInfo.maxProxies = value;
		    }
	    }

	    [Editable]
	    public Vector3 Gravity
	    {
		    get
		    {
                return sceneInfo.gravity;
		    }
		    set
		    {
                sceneInfo.gravity = value;
		    }
	    }

         /// <summary>
        /// Create function for commands.
        /// </summary>
        /// <param name="name">The name of the definition to create.</param>
        /// <returns>A new definition.</returns>
        internal static SimElementManagerDefinition Create(String name, EditUICallback callback)
        {
            return new BulletSceneDefinition(name);
        }

        #region Saveable Members

        protected BulletSceneDefinition(LoadInfo info)
        {
            name = info.GetString("Name");
            WorldAabbMin = info.GetVector3("WorldAABBMin");
            WorldAabbMax = info.GetVector3("WorldAABBMax");
            MaxProxies = info.GetInt32("MaxProxies");
            Gravity = info.GetVector3("Gravity");
        }

        public void getInfo(SaveInfo info)
        {
            info.AddValue("Name", name);
            info.AddValue("WorldAABBMin", WorldAabbMin);
            info.AddValue("WorldAABBMax", WorldAabbMax);
            info.AddValue("MaxProxies", MaxProxies);
            info.AddValue("Gravity", Gravity);
        }

        #endregion
    }
}
