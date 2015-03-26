using System;

using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using Anomalous.OSPlatform;
using Android.Views.InputMethods;
using Android.Content.PM;

namespace AndroidBaseApp
{
	[Activity (Label = "AndroidBaseApp", MainLauncher = true, Icon = "@drawable/icon", Theme="@android:style/Theme.NoTitleBar.Fullscreen", 
		ConfigurationChanges= ConfigChanges.Orientation | ConfigChanges.KeyboardHidden | ConfigChanges.ScreenSize | ConfigChanges.ScreenLayout)]
	[MetaData("android.app.lib_name", Value="OSHelper")]
	public class MainActivity : NativeActivity
	{
		protected override void OnCreate (Bundle bundle)
		{
			Logging.Log.Default.addLogListener (new Logging.LogConsoleListener ());

			Java.Lang.JavaSystem.LoadLibrary ("openal");

			base.OnCreate (bundle);

			AndroidFunctions.EasyAttributeSetup (Resources.DisplayMetrics.Density, toggleKeyboard);

			//RequestWindowFeature (WindowFeatures.NoTitle);
			//Window.SetFlags (WindowManagerFlags.Fullscreen, WindowManagerFlags.Fullscreen);

			//var app = new Anomalous.Minimus.OgreOnly.OgreOnlyApp();
			var app = new Anomalous.Minimus.Full.MinimalApp ();
			app.run();
		}

		void toggleKeyboard()
		{
			//Logging.Log.Debug (Android.App.Application.Context.GetType ().ToString ());
			//InputMethodManager inputMethod = Android.App.Application.Context.GetSystemService (InputMethodService) as InputMethodManager;
			//inputMethod.ToggleSoftInput (ShowFlags.Forced, 0);
			//Logging.Log.Debug (inputMethod.ToString ());

			//for hidig (theoretically) http://comments.gmane.org/gmane.comp.handhelds.android.ndk/17582
			//InputMethodManager m = (InputMethodManager)NativeActivitySubclass.Get().getSystemService(Context.INPUT_METHOD_SERVICE);
			//m.toggleSoftInput(0, 0);   


			Handler mainHandler = new Handler (Android.App.Application.Context.MainLooper);
			//mainHandler.po



//			RunOnUiThread (() => {
//				var builder = new AlertDialog.Builder (gayness);
//				builder.SetMessage ("This is a message");
//				builder.Show ();
//			});
		}
	}
}


