using System;
using System.Linq;
using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using Anomalous.OSPlatform;
using Android.Views.InputMethods;
using Android.Content.PM;
using System.Collections.Generic;
using Android.Text;
using Engine;
using Engine.ObjectManagement;
using Anomalous.OSPlatform.Android;

namespace AndroidBaseApp
{
	[Activity (Label = "AndroidBaseApp", MainLauncher = true, Icon = "@drawable/icon", Theme="@android:style/Theme.NoTitleBar.Fullscreen", 
		ConfigurationChanges= ConfigChanges.Orientation | ConfigChanges.KeyboardHidden | ConfigChanges.ScreenSize | ConfigChanges.ScreenLayout,
		WindowSoftInputMode = SoftInput.StateAlwaysHidden)]
	[MetaData("android.app.lib_name", Value = AndroidPlatformPlugin.LibraryName)]
	public class MainActivity : AndroidActivity
	{
		static MainActivity()
		{
			Java.Lang.JavaSystem.LoadLibrary ("openal");
		}

		public MainActivity()
			:base(Resource.Layout.Main, Resource.Id.editText1)
		{

		}

		protected override void createApp ()
		{
			Logging.Log.Default.addLogListener (new Logging.LogConsoleListener ());

			//var app = new Anomalous.Minimus.OgreOnly.OgreOnlyApp();
			var app = new Anomalous.Minimus.Full.MinimalApp ();
			app.Initialized += HandleInitialized;
			app.run();
		}

		void HandleInitialized (Anomalous.Minimus.Full.MinimalApp obj)
		{
			setInputHandler (obj.EngineController.InputHandler);

			String archivePath = System.IO.Path.Combine(Application.Context.ObbDir.AbsolutePath, "AnomalousMedical.dat");
			if (System.IO.File.Exists (archivePath) || System.IO.Directory.Exists(archivePath)) 
			{
				VirtualFileSystem.Instance.addArchive (archivePath);
			} 
			else 
			{
				Logging.Log.Warning ("Cannot find primarydata file");
			}

			var resourceManager = obj.EngineController.PluginManager.createLiveResourceManager("Main");
			var ogreResources = resourceManager.getSubsystemResource("Ogre");
			var shaders = ogreResources.addResourceGroup("Shaders");
			shaders.addResource("Shaders/Articulometrics", "EngineArchive", true);
			var models = ogreResources.addResourceGroup("Models");
			models.addResource("Models/Export/Spine", "EngineArchive", false);
			resourceManager.initializeResources();

			GenericSimObjectDefinition simObj = new GenericSimObjectDefinition("TestObj");
			var node = new OgrePlugin.SceneNodeDefinition("Node");
			var entity = new OgrePlugin.EntityDefinition("Entity")
			{
				MeshName = "c1.mesh",
				//MeshName = "LigamentaFlava.mesh"
			};
			node.addMovableObjectDefinition(entity);
			simObj.addElement(node);
			simObj.register(obj.Scene.getDefaultSubScene());

			using (var c1Str = VirtualFileSystem.Instance.openStream ("Models/Export/Spine/c1.mesh", Engine.Resources.FileMode.Open, Engine.Resources.FileAccess.Read)) {
				Logging.Log.Debug ("Stream size {0}", c1Str.Length);
			}

			obj.Scene.buildScene();
		}
	}
}


