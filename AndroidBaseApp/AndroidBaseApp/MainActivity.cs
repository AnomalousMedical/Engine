using System;

using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;

namespace AndroidBaseApp
{
	[Activity (Label = "AndroidBaseApp", MainLauncher = true, Icon = "@drawable/icon")]
	public class MainActivity : Activity
	{
		protected override void OnCreate (Bundle bundle)
		{
			Logging.Log.Default.addLogListener (new Logging.LogConsoleListener ());

			//Java.Lang.JavaSystem.LoadLibrary ("openal");

			base.OnCreate (bundle);

			RequestWindowFeature (WindowFeatures.NoTitle);
			Window.SetFlags (WindowManagerFlags.Fullscreen, WindowManagerFlags.Fullscreen);

			var app = new Anomalous.Minimus.OgreOnly.OgreOnlyApp();
			app.run();

			Intent intent = new Intent (this, typeof(NativeActivity));
			intent.SetFlags (ActivityFlags.ClearTop);

			StartActivity (intent);
			Finish ();
		}
	}
}


