using Anomalous.OSPlatform;
using Engine;
using Engine.Platform;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Anomalous.Minimus
{
    public class TouchMouseGuiForwarder
    {
        private int currentFingerId = int.MinValue;
        private IntVector2 gestureStartPos;
        private Touches touches;
        private NativeOSWindow window;
        private NativeInputHandler inputHandler;
		private bool enabled = true;
		private OnscreenKeyboardMode keyboardMode = OnscreenKeyboardMode.Hidden;

        public TouchMouseGuiForwarder(EventManager eventManager, NativeInputHandler inputHandler, NativeOSWindow window, Object lastEventLayer)
        {
            this.touches = eventManager.Touches;
            this.touches.FingerStarted += HandleFingerStarted;
            this.inputHandler = inputHandler;
            this.window = window;

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
                var finger = touches.Fingers[0];
                currentFingerId = finger.Id;
                touches.FingerEnded += fingerEnded;
                touches.FingerMoved += HandleFingerMoved;
				touches.FingersCanceled += HandleFingersCanceled;
                gestureStartPos = new IntVector2(finger.PixelX, finger.PixelY);
                inputHandler.injectMoved(finger.PixelX, finger.PixelY);
                inputHandler.injectButtonDown(MouseButtonCode.MB_BUTTON0);
            }
        }

        void HandleFingerMoved(Finger obj)
        {
            if (obj.Id == currentFingerId)
            {
                inputHandler.injectMoved(obj.PixelX, obj.PixelY);
            }
        }

        void fingerEnded(Finger obj)
        {
            if (obj.Id == currentFingerId)
            {
				stopTrackingFinger();
                inputHandler.injectMoved(obj.PixelX, obj.PixelY);
                inputHandler.injectButtonUp(MouseButtonCode.MB_BUTTON0);
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
