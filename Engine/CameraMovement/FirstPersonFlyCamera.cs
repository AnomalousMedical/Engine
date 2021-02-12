using Engine.Platform;
using System;
using System.Collections.Generic;
using System.Text;

namespace Engine.CameraMovement
{
    public class FirstPersonFlyCamera : IDisposable
    {
        private const float HALF_PI = (float)Math.PI / 2.0f - 0.001f;
        private readonly EventManager eventManager;
        Vector3 camPos = Vector3.Zero;
        Quaternion camRot = Quaternion.Identity;
        float yaw = 0;
        float pitch = 0;
        float moveSpeed = 10.0f;
        float viewSpeed = 1.0f;

        float xSensitivity = 0.005f;
        float ySensitivity = 0.005f;

        Vector3 currentForward = Vector3.Forward;
        Vector3 currentLeft = Vector3.Left;

        ButtonEvent moveForward = new ButtonEvent(EventLayers.Default, keys: new KeyboardButtonCode[] { KeyboardButtonCode.KC_W });
        ButtonEvent moveBackward = new ButtonEvent(EventLayers.Default, keys: new KeyboardButtonCode[] { KeyboardButtonCode.KC_S });
        ButtonEvent moveLeft = new ButtonEvent(EventLayers.Default, keys: new KeyboardButtonCode[] { KeyboardButtonCode.KC_A });
        ButtonEvent moveRight = new ButtonEvent(EventLayers.Default, keys: new KeyboardButtonCode[] { KeyboardButtonCode.KC_D });
        ButtonEvent moveUp = new ButtonEvent(EventLayers.Default, keys: new KeyboardButtonCode[] { KeyboardButtonCode.KC_E });
        ButtonEvent moveDown = new ButtonEvent(EventLayers.Default, keys: new KeyboardButtonCode[] { KeyboardButtonCode.KC_Q });
        ButtonEvent pitchUp = new ButtonEvent(EventLayers.Default, keys: new KeyboardButtonCode[] { KeyboardButtonCode.KC_UP });

        ButtonEvent pitchDown = new ButtonEvent(EventLayers.Default, keys: new KeyboardButtonCode[] { KeyboardButtonCode.KC_DOWN });
        ButtonEvent yawLeft = new ButtonEvent(EventLayers.Default, keys: new KeyboardButtonCode[] { KeyboardButtonCode.KC_LEFT });
        ButtonEvent yawRight = new ButtonEvent(EventLayers.Default, keys: new KeyboardButtonCode[] { KeyboardButtonCode.KC_RIGHT });

        ButtonEvent mouseLook = new ButtonEvent(EventLayers.Default, mouseButtons: new MouseButtonCode[] { MouseButtonCode.MB_BUTTON1 });

        public FirstPersonFlyCamera(EventManager eventManager)
        {
            eventManager.addEvent(moveForward);
            eventManager.addEvent(moveBackward);
            eventManager.addEvent(moveLeft);
            eventManager.addEvent(moveRight);
            eventManager.addEvent(moveUp);
            eventManager.addEvent(moveDown);
            eventManager.addEvent(pitchUp);
            eventManager.addEvent(pitchDown);
            eventManager.addEvent(yawLeft);
            eventManager.addEvent(yawRight);
            eventManager.addEvent(mouseLook);
            this.eventManager = eventManager;
        }

        public void Dispose()
        {
            eventManager.removeEvent(moveForward);
            eventManager.removeEvent(moveBackward);
            eventManager.removeEvent(moveLeft);
            eventManager.removeEvent(moveRight);
            eventManager.removeEvent(moveUp);
            eventManager.removeEvent(moveDown);
            eventManager.removeEvent(pitchUp);
            eventManager.removeEvent(pitchDown);
            eventManager.removeEvent(yawLeft);
            eventManager.removeEvent(yawRight);
            eventManager.removeEvent(mouseLook);
        }

        public void UpdateInput(Clock clock)
        {
            bool updateRotation = false;

            if (pitchUp.Down)
            {
                pitch += clock.DeltaSeconds * viewSpeed;
                updateRotation = true;
            }

            if (pitchDown.Down)
            {
                pitch -= clock.DeltaSeconds * viewSpeed;
                updateRotation = true;
            }

            if (yawLeft.Down)
            {
                yaw -= clock.DeltaSeconds * viewSpeed;
                updateRotation = true;
            }

            if (yawRight.Down)
            {
                yaw += clock.DeltaSeconds * viewSpeed;
                updateRotation = true;
            }

            if (mouseLook.Down)
            {
                var mousePos = eventManager.Mouse.RelativePosition;
                if (mousePos != IntVector3.Zero)
                {
                    updateRotation = true;
                    
                    mousePos.x = Math.Min(mousePos.x, 10);
                    mousePos.y = Math.Min(mousePos.y, 10);
                    
                    yaw += mousePos.x * xSensitivity;
                    pitch -= mousePos.y * ySensitivity;
                }
            }

            if (updateRotation)
            {
                if (pitch > HALF_PI)
                {
                    pitch = HALF_PI;
                }
                if (pitch < -HALF_PI)
                {
                    pitch = -HALF_PI;
                }

                var yawRot = new Quaternion(Vector3.Up, yaw);
                var pitchRot = new Quaternion(Vector3.Left, pitch);
                camRot = yawRot * pitchRot;

                currentForward = Quaternion.quatRotate(camRot, Vector3.Forward);
                currentLeft = Quaternion.quatRotate(camRot, Vector3.Left);
            }

            if (moveForward.Down)
            {
                camPos += currentForward * clock.DeltaSeconds * moveSpeed;
            }

            if (moveBackward.Down)
            {
                camPos -= currentForward * clock.DeltaSeconds * moveSpeed;
            }

            if (moveLeft.Down)
            {
                camPos += currentLeft * clock.DeltaSeconds * moveSpeed;
            }

            if (moveRight.Down)
            {
                camPos -= currentLeft * clock.DeltaSeconds * moveSpeed;
            }

            if (moveUp.Down)
            {
                camPos += Vector3.Up * clock.DeltaSeconds * moveSpeed;
            }

            if (moveDown.Down)
            {
                camPos += Vector3.Down * clock.DeltaSeconds * moveSpeed;
            }
        }

        public Vector3 Position
        {
            get
            {
                return camPos;
            }
            set
            {
                camPos = value;
            }
        }

        public Quaternion Orientation => camRot;

        public float Yaw
        {
            get
            {
                return yaw;
            }
            set
            {
                yaw = value;
            }
        }

        public float Pitch
        {
            get
            {
                return pitch;
            }
            set
            {
                pitch = value;
            }
        }
    }
}