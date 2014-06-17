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
        static FilteredMemberScanner memberScanner = new FilteredMemberScanner();

        static BulletSceneDefinition()
        {
            memberScanner.ProcessFields = false;
            memberScanner.Filter = new EditableAttributeFilter();
        }

        private String name;
        internal BulletSceneInfo sceneInfo = new BulletSceneInfo();
        EditInterface editInterface;

        public BulletSceneDefinition(String name)
        {
            this.name = name;
            sceneInfo.maxProxies = 1024;
            sceneInfo.gravity = new Vector3(0f, -9.8f, 0f);
            sceneInfo.worldAabbMax = new Vector3(10000f, 10000f, 10000f);
            sceneInfo.worldAabbMin = new Vector3(-10000f, -10000f, -10000f);
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

        public virtual SimElementManager createSimElementManager()
        {
            return new BulletScene(this, BulletInterface.Instance.UpdateTimer);
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

        public virtual void getInfo(SaveInfo info)
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
