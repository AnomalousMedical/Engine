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
using System.Collections.Generic;

namespace AndroidBaseApp
{
	[Activity (Label = "AndroidBaseApp", MainLauncher = true, Icon = "@drawable/icon", Theme="@android:style/Theme.NoTitleBar.Fullscreen", 
		ConfigurationChanges= ConfigChanges.Orientation | ConfigChanges.KeyboardHidden | ConfigChanges.ScreenSize | ConfigChanges.ScreenLayout,
		WindowSoftInputMode = SoftInput.StateAlwaysHidden)]
	[MetaData("android.app.lib_name", Value="OSHelper")]
	public class MainActivity : NativeActivity
	{
		private EditText editText;

		protected override void OnCreate (Bundle bundle)
		{
			Logging.Log.Default.addLogListener (new Logging.LogConsoleListener ());

			Java.Lang.JavaSystem.LoadLibrary ("openal");

			AndroidFunctions.EasyAttributeSetup (Resources.DisplayMetrics.Density, toggleKeyboard);

			//var app = new Anomalous.Minimus.OgreOnly.OgreOnlyApp();
			var app = new Anomalous.Minimus.Full.MinimalApp ();
			app.run();

			base.OnCreate (bundle);

			SetContentView (Resource.Layout.Main);

			editText = FindViewById<EditText> (Resource.Id.editText1);
			editText.TextChanged += HandleBeforeTextChanged;
			Window.SetSoftInputMode (SoftInput.StateAlwaysHidden);
		}

		void HandleBeforeTextChanged (object sender, Android.Text.TextChangedEventArgs e)
		{
			Logging.Log.Debug (e.Text.ToString());
		}

		void toggleKeyboard(bool visible)
		{
			InputMethodManager inputMethod = GetSystemService (InputMethodService) as InputMethodManager;
			if (visible) 
			{
				inputMethod.ShowSoftInput (editText, ShowFlags.Forced);
			} 
			else 
			{
				inputMethod.HideSoftInputFromWindow (editText.WindowToken, 0);
			}
		}
	}
}


