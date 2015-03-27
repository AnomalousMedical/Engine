﻿using System;
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

namespace AndroidBaseApp
{
	[Activity (Label = "AndroidBaseApp", MainLauncher = true, Icon = "@drawable/icon", Theme="@android:style/Theme.NoTitleBar.Fullscreen", 
		ConfigurationChanges= ConfigChanges.Orientation | ConfigChanges.KeyboardHidden | ConfigChanges.ScreenSize | ConfigChanges.ScreenLayout,
		WindowSoftInputMode = SoftInput.StateAlwaysHidden)]
	[MetaData("android.app.lib_name", Value="OSHelper")]
	public class MainActivity : NativeActivity
	{
		private EditText editText;
		bool fireInputs = true;
		private NativeInputHandler inputHandler;

		protected override void OnCreate (Bundle bundle)
		{
			Logging.Log.Default.addLogListener (new Logging.LogConsoleListener ());

			Java.Lang.JavaSystem.LoadLibrary ("openal");

			AndroidFunctions.EasyAttributeSetup (Resources.DisplayMetrics.Density, toggleKeyboard);

			//var app = new Anomalous.Minimus.OgreOnly.OgreOnlyApp();
			var app = new Anomalous.Minimus.Full.MinimalApp ();
			app.Initialized += HandleInitialized;
			app.run();

			base.OnCreate (bundle);

			SetContentView (Resource.Layout.Main);

			editText = FindViewById<EditText> (Resource.Id.editText1);
			editText.TextChanged += HandleTextChanged;
			Window.SetSoftInputMode (SoftInput.StateAlwaysHidden);
		}

		void HandleInitialized (Anomalous.Minimus.Full.MinimalApp obj)
		{
			inputHandler = obj.EngineController.InputHandler;
		}

		/// <summary>
		/// Handle the text changing in our edit box. Note that this only really handles text and not
		/// all keyboard events, other keyboard events like enter and delete(sometimes) will come from the 
		/// native code input function.
		/// </summary>
		void HandleTextChanged (object sender, Android.Text.TextChangedEventArgs e)
		{
			if (fireInputs) 
			{
				Logging.Log.Debug ("'{0}' bc: {2} ac: {1} s: {3}", e.Text.ToString (), e.AfterCount, e.BeforeCount, e.Start);
				if (e.AfterCount - e.BeforeCount >= 0) {
					foreach (char c in e.Text.Skip(e.Start + e.BeforeCount)) {
						inputHandler.injectKeyPressed (Engine.Platform.KeyboardButtonCode.KC_UNASSIGNED, c);
						inputHandler.injectKeyReleased (Engine.Platform.KeyboardButtonCode.KC_UNASSIGNED);
					}
				} else {
					int count = e.BeforeCount - e.AfterCount;
					for (int i = 0; i < count; ++i) {
						inputHandler.injectKeyPressed (Engine.Platform.KeyboardButtonCode.KC_BACK, 0);
						inputHandler.injectKeyReleased (Engine.Platform.KeyboardButtonCode.KC_BACK);
					}
				}
			} 
			else 
			{
				Logging.Log.Debug ("Clearing");
			}
		}

		void toggleKeyboard(bool visible)
		{
			RunOnUiThread (() => 
			{
				fireInputs = false;
				editText.Text = "";
				fireInputs = true;
				InputMethodManager inputMethod = GetSystemService (InputMethodService) as InputMethodManager;
				if (visible) 
				{
					inputMethod.ShowSoftInput (editText, ShowFlags.Forced);
				}
				else 
				{
					inputMethod.HideSoftInputFromWindow (editText.WindowToken, 0);
				}
			});
		}
	}
}


