using Anomalous.OSPlatform;
using Engine;
using Engine.Platform;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Anomalous.OSPlatform
{
    public class TouchMouseGuiForwarder : OnscreenKeyboardManager
    {
        enum MouseStatus
        {
            Released,
            Left,
            Right
        }

        private const long RightClickDeltaTime = 800000; //microseconds

        private int currentFingerId = int.MinValue;
        private long fingerDownTime = long.MinValue;
        private TravelTracker captureClickZone = new TravelTracker(ScaleHelper.Scaled(5));
        private MouseStatus mouseInjectionMode = MouseStatus.Released;

        private Touches touches;
        private NativeOSWindow window;
        private NativeInputHandler inputHandler;
		private bool enabled = true;
		private OnscreenKeyboardMode keyboardMode = OnscreenKeyboardMode.Hidden;
        private SystemTimer systemTimer;

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="eventManager">The EventManager to use.</param>
        /// <param name="inputHandler">The InputHandler to use.</param>
        /// <param name="window">The window to show the onscreen keyboard on.</param>
        /// <param name="lastEventLayer">The last event layer in the eventManager.</param>
        public TouchMouseGuiForwarder(EventManager eventManager, NativeInputHandler inputHandler, SystemTimer systemTimer, NativeOSWindow window, Object lastEventLayer)
        {
            this.touches = eventManager.Touches;
            this.touches.FingerStarted += HandleFingerStarted;
            this.inputHandler = inputHandler;
            this.window = window;
            this.systemTimer = systemTimer;

            eventManager[lastEventLayer].Keyboard.KeyPressed += HandleKeyPressed;
            eventManager[lastEventLayer].Keyboard.KeyReleased += HandleKeyReleased;
        }

        /// <summary>
        /// In rare instances you might have to toggle the onscreen keyboard manually right away, this function will
        /// do that, but most times this is handled automatically with no problems.
        /// </summary>
        public void toggleKeyboard()
        {
            if (keyboardMode != window.KeyboardMode)
            {
                window.KeyboardMode = keyboardMode;
            }
        }

        /// <summary>
        /// Enable / Disable finger tracking.
        /// </summary>
		public bool Enabled
		{
			get
			{
				return enabled;
			}
			set
			{
				enabled = value;
				if(!enabled && currentFingerId == int.MinValue)
				{
					stopTrackingFinger();
				}
			}
		}

        /// <summary>
        /// The keyboard mode that will be set the next time togglekeyboard is called. This does not reflect
        /// the actual keyboard status.
        /// </summary>
        public OnscreenKeyboardMode KeyboardMode
        {
            get
            {
                return keyboardMode;
            }
            set
            {
                keyboardMode = value;
            }
        }

        void HandleFingerStarted(Finger obj)
        {
            if (currentFingerId == int.MinValue && enabled)
            {
                fingerDownTime = systemTimer.getCurrentTime();
                var finger = touches.Fingers[0];
                currentFingerId = finger.Id;
                touches.FingerEnded += fingerEnded;
                touches.FingerMoved += HandleFingerMoved;
				touches.FingersCanceled += HandleFingersCanceled;
                captureClickZone.reset();
                inputHandler.injectMoved(finger.PixelX, finger.PixelY);
                mouseInjectionMode = MouseStatus.Released;
            }
        }

        void HandleFingerMoved(Finger obj)
        {
            if (obj.Id == currentFingerId)
            {
                inputHandler.injectMoved(obj.PixelX, obj.PixelY);
                captureClickZone.traveled(new IntVector2(obj.PixelDeltaX, obj.PixelDeltaY));
                if (mouseInjectionMode == MouseStatus.Released && captureClickZone.TraveledOverLimit)
                {
                    if (systemTimer.getCurrentTime() - fingerDownTime < RightClickDeltaTime)
                    {
                        mouseInjectionMode = MouseStatus.Right;
                        inputHandler.injectButtonDown(MouseButtonCode.MB_BUTTON1);
                    }
                    else
                    {
                        mouseInjectionMode = MouseStatus.Left;
                        inputHandler.injectButtonDown(MouseButtonCode.MB_BUTTON0);
                    }
                }
            }
        }

        void fingerEnded(Finger obj)
        {
            if (obj.Id == currentFingerId)
            {
				stopTrackingFinger();
                inputHandler.injectMoved(obj.PixelX, obj.PixelY);
                switch(mouseInjectionMode)
                {
                    case MouseStatus.Released:
                        if (systemTimer.getCurrentTime() - fingerDownTime < RightClickDeltaTime)
                        {
                            inputHandler.injectButtonDown(MouseButtonCode.MB_BUTTON0);
                            inputHandler.injectButtonUp(MouseButtonCode.MB_BUTTON0);
                        }
                        else
                        {
                            inputHandler.injectButtonDown(MouseButtonCode.MB_BUTTON1);
                            inputHandler.injectButtonUp(MouseButtonCode.MB_BUTTON1);
                        }
                        break;
                    case MouseStatus.Left:
                        inputHandler.injectButtonUp(MouseButtonCode.MB_BUTTON0);
                        break;
                    case MouseStatus.Right:
                        inputHandler.injectButtonUp(MouseButtonCode.MB_BUTTON1);
                        break;
                }
                mouseInjectionMode = MouseStatus.Released;
				toggleKeyboard();
            }
		}

		void HandleFingersCanceled()
		{
			if(currentFingerId != int.MinValue)
			{
				stopTrackingFinger();
				inputHandler.injectButtonUp(MouseButtonCode.MB_BUTTON0);
				toggleKeyboard();
			}
		}

		void stopTrackingFinger()
		{
			touches.FingerEnded -= fingerEnded;
			touches.FingerMoved -= HandleFingerMoved;
			currentFingerId = int.MinValue;
		}

		void HandleKeyReleased(KeyboardButtonCode keyCode)
		{
			toggleKeyboard();
		}

		void HandleKeyPressed(KeyboardButtonCode keyCode, uint keyChar)
		{
			toggleKeyboard();
		}
    }
}
