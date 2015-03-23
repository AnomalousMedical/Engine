using System;

using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using Anomalous.OSPlatform;

namespace AndroidBaseApp
{
	[Activity (Label = "AndroidBaseApp", MainLauncher = true, Icon = "@drawable/icon", Theme="@android:style/Theme.NoTitleBar.Fullscreen")]
	public class MainActivity : Activity
	{
		protected override void OnCreate (Bundle bundle)
		{
			Logging.Log.Default.addLogListener (new Logging.LogConsoleListener ());

			Java.Lang.JavaSystem.LoadLibrary ("openal");

			base.OnCreate (bundle);

			AndroidFunctions.EasyAttributeSetup (Resources.DisplayMetrics.Density);

			RequestWindowFeature (WindowFeatures.NoTitle);
			Window.SetFlags (WindowManagerFlags.Fullscreen, WindowManagerFlags.Fullscreen);

			//var app = new Anomalous.Minimus.OgreOnly.OgreOnlyApp();
			var app = new Anomalous.Minimus.Full.MinimalApp ();
			app.run();

			Intent intent = new Intent (this, typeof(NativeActivity));
			intent.SetFlags (ActivityFlags.ClearTop);

			StartActivity (intent);
			Finish ();
		}
	}
}


