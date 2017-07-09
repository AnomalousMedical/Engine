using Android.App;
using Android.OS;
using Android.Text;
using Android.Util;
using Android.Views;
using Android.Views.InputMethods;
using Android.Widget;
using Anomalous.Shim;
using Engine.Platform;
using Engine.Shim;
using Engine.Threads;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Anomalous.OSPlatform.Android
{
    /// <summary>
    /// This class provides a simple Activity base class that can be used for Anomalous Engine apps.
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
        static AndroidActivity()
        {
            NetFrameworkShim.SetShimImpl(new FullNetFrameworkShim());
        }

        private EditText editText;
        bool fireInputs = true;
        private InputHandler inputHandler;
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

            VolumeControlStream = global::Android.Media.Stream.Music;

            this.editText = FindViewById<EditText>(editTextId);
            this.editText.TextChanged += editText_TextChanged;
            Window.SetSoftInputMode(SoftInput.StateAlwaysHidden);
        }

        public override bool DispatchKeyEvent(KeyEvent e)
        {
            //You can handle multiple controllers by watching them connect and disconnect, will do later
            //https://developer.android.com/training/game-controllers/multiple-controllers.html
            //Getting weird values for source, so not checking it for now, buttons are specifically joystick anyway
            //if ((e.Source & InputSourceType.ClassJoystick) == InputSourceType.ClassJoystick)
            //{
            //    Log.Debug("woot", $"Source blocked {e.KeyCode}");
            //    return true;
            //}
            switch (e.KeyCode)
            {
                case Keycode.ButtonA:
                case Keycode.ButtonB:
                case Keycode.ButtonC:
                case Keycode.ButtonL1:
                case Keycode.ButtonL2:
                case Keycode.ButtonMode:
                case Keycode.ButtonR1:
                case Keycode.ButtonR2:
                case Keycode.ButtonSelect:
                case Keycode.ButtonStart:
                case Keycode.ButtonThumbl:
                case Keycode.ButtonThumbr:
                case Keycode.ButtonX:
                case Keycode.ButtonY:
                case Keycode.ButtonZ:
                case Keycode.Button1:
                case Keycode.Button2:
                case Keycode.Button3:
                case Keycode.Button4:
                case Keycode.Button5:
                case Keycode.Button6:
                case Keycode.Button7:
                case Keycode.Button8:
                case Keycode.Button9:
                case Keycode.Button10:
                case Keycode.Button11:
                case Keycode.Button12:
                case Keycode.Button13:
                case Keycode.Button14:
                case Keycode.Button15:
                case Keycode.Button16:
                    Log.Debug("woot", $"Dispatch key event block button {e.KeyCode} {e.Source} {e.DeviceId}");
                    return true;
            }
            return base.DispatchKeyEvent(e);
        }

        public override bool DispatchGenericMotionEvent(MotionEvent ev)
        {
            if(ev.Source == InputSourceType.Joystick)
            {
                Log.Debug("woot", $"Dispatch generic motion {ev.Action} {ev.Device.Name} {ev.Source}");
                return true;
            }

            return base.DispatchGenericMotionEvent(ev);
        }

        /// <summary>
        /// Call this function to kill the app process, ideally this is done at the end of OnDestroy in your client app.
        /// </summary>
        protected void killAppProcess()
        {
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
        protected void setInputHandler(InputHandler inputHandler)
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
                        //editText.InputType = InputTypes.ClassText | InputTypes.TextFlagMultiLine; //Allows for autocorrect
                        editText.InputType = InputTypes.TextVariationWebPassword | InputTypes.TextFlagMultiLine;
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
                    //This will create a fair amount of garbage, but the user probably cannot type that fast
                    //Calling ThreadManager.invoke for each char creates garbage also.
                    char[] inputArray = e.Text.Skip(e.Start + e.BeforeCount).ToArray();
                    ThreadManager.invoke(() =>
                    {
                        foreach (char c in inputArray)
                        {
                            inputHandler.injectKeyPressed(Engine.Platform.KeyboardButtonCode.KC_UNASSIGNED, c);
                            inputHandler.injectKeyReleased(Engine.Platform.KeyboardButtonCode.KC_UNASSIGNED);
                        }
                    });
                }
                else
                {
                    int count = e.BeforeCount - e.AfterCount;
                    ThreadManager.invoke(() =>
                    {
                        //Inject the backspace count on the main thread
                        for (int i = 0; i < count; ++i)
                        {
                            inputHandler.injectKeyPressed(Engine.Platform.KeyboardButtonCode.KC_BACK, 0);
                            inputHandler.injectKeyReleased(Engine.Platform.KeyboardButtonCode.KC_BACK);
                        }
                    });
                }
            } 
        }

        public float DeviceSizeInches
        {
            get
            {
                DisplayMetrics dm = new DisplayMetrics();
                WindowManager.DefaultDisplay.GetMetrics(dm);
                double x = Math.Pow(dm.WidthPixels / dm.Xdpi, 2);
                double y = Math.Pow(dm.HeightPixels / dm.Ydpi, 2);
                return (float)Math.Sqrt(x + y);
            }
        }
    }
}
