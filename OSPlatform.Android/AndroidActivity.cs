using Android.App;
using Android.OS;
using Android.Text;
using Android.Views;
using Android.Views.InputMethods;
using Android.Widget;
using Engine.Platform;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Anomalous.OSPlatform.Android
{
    /// <summary>
    /// This class provides a simple Activity instance that can be used for Anomalous Engine apps.
    /// You will need to define attributes on your subclass that look like the following:
    /// <code>
    /// [Activity (Label = "App Name", MainLauncher = true, Icon = "@drawable/icon", Theme="@android:style/Theme.NoTitleBar.Fullscreen", ConfigurationChanges= ConfigChanges.Orientation | ConfigChanges.KeyboardHidden | ConfigChanges.ScreenSize | ConfigChanges.ScreenLayout, WindowSoftInputMode = SoftInput.StateAlwaysHidden)]
    /// </code>
    /// <code>
    /// [MetaData("android.app.lib_name", Value = AndroidPlatformPlugin.LibraryName)]
    /// </code>
    /// </summary>
    /// <remarks>
    /// If you need to load any other static libraries use the static constructor of your subclass and load them there.
    /// </remarks>
    public abstract class AndroidActivity : NativeActivity
    {
        private EditText editText;
        bool fireInputs = true;
        private NativeInputHandler inputHandler;
        int contentViewId;
        int editTextId;

        public AndroidActivity(int contentViewId, int editTextId)
        {
            new AndroidRuntimePlatformInfo(this);
            this.contentViewId = contentViewId;
            this.editTextId = editTextId;
        }

        protected override sealed void OnCreate(Bundle bundle)
        {
            AndroidFunctions.EasyAttributeSetup(Resources.DisplayMetrics.Density, toggleKeyboard);

            createApp();

            base.OnCreate(bundle);

            SetContentView(contentViewId);

            this.editText = FindViewById<EditText>(editTextId);
            this.editText.TextChanged += editText_TextChanged;
            Window.SetSoftInputMode(SoftInput.StateAlwaysHidden);
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
            global::Android.OS.Process.KillProcess(global::Android.OS.Process.MyPid());
        }

        /// <summary>
        /// Create the app instance that will run for this activity. This will be called during OnCreate,
        /// which is sealed in this class since it relies on a specific order of doing things.
        /// </summary>
        protected abstract void createApp();

        /// <summary>
        /// Helper function to output some debugging info about the current runtime.
        /// </summary>
        protected void printRuntimeInfo()
        {
            AndroidFunctions.OutputCurrentABI();
        }

        /// <summary>
        /// Set the input handler once you can get an instance to it.
        /// </summary>
        /// <param name="inputHandler">The InputHandler to use to fire input events.</param>
        protected void setInputHandler(NativeInputHandler inputHandler)
        {
            this.inputHandler = inputHandler;
        }

        private void toggleKeyboard(OnscreenKeyboardMode mode)
        {
            RunOnUiThread(() =>
            {
                fireInputs = false;
                editText.Text = "";
                fireInputs = true;

                InputMethodManager inputMethod = GetSystemService(InputMethodService) as InputMethodManager;
                switch(mode)
                {
                    case OnscreenKeyboardMode.Hidden:
                        inputMethod.HideSoftInputFromWindow(editText.WindowToken, 0);
                        break;                        
                    case OnscreenKeyboardMode.Secure:
                        editText.InputType = InputTypes.TextVariationWebPassword;
                        inputMethod.ShowSoftInput(editText, ShowFlags.Forced);
                        break;
                    //Make normal and default the same in case we add a type but we haven't added it here yet.
                    case OnscreenKeyboardMode.Normal:
                    default:
                        editText.InputType = InputTypes.ClassText | InputTypes.TextFlagMultiLine;
                        inputMethod.ShowSoftInput(editText, ShowFlags.Forced);
                        break;
                }
            });
        }

        void editText_TextChanged(object sender, global::Android.Text.TextChangedEventArgs e)
        {
            if (fireInputs)
            {
                //Logging.Log.Debug ("'{0}' bc: {2} ac: {1} s: {3}", e.Text.ToString (), e.AfterCount, e.BeforeCount, e.Start);
                if (e.AfterCount - e.BeforeCount >= 0)
                {
                    foreach (char c in e.Text.Skip(e.Start + e.BeforeCount))
                    {
                        inputHandler.injectKeyPressed(Engine.Platform.KeyboardButtonCode.KC_UNASSIGNED, c);
                        inputHandler.injectKeyReleased(Engine.Platform.KeyboardButtonCode.KC_UNASSIGNED);
                    }
                }
                else
                {
                    int count = e.BeforeCount - e.AfterCount;
                    for (int i = 0; i < count; ++i)
                    {
                        inputHandler.injectKeyPressed(Engine.Platform.KeyboardButtonCode.KC_BACK, 0);
                        inputHandler.injectKeyReleased(Engine.Platform.KeyboardButtonCode.KC_BACK);
                    }
                }
            } 
        }
    }
}
