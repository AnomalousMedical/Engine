using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Engine.Renderer;
using Engine.Platform;
using Engine.ObjectManagement;
using Engine.Editing;
using Editor;
using Engine;

namespace Anomaly
{
    class EditInterfaceRendererController : UpdateListener
    {
        private RendererPlugin renderer;
        private DebugDrawingSurface debugSurface;
        private UpdateTimer timer;
        private EditInterfaceRenderer currentRenderer;
        private IObjectEditorGUI mainEditor;
        private Vector3 currentOrigin;
        private Quaternion currentRotation;
        private Instance instance;

        public EditInterfaceRendererController(RendererPlugin renderer, UpdateTimer timer, SceneController sceneController, IObjectEditorGUI mainEditor)
        {
            this.renderer = renderer;
            this.timer = timer;
            this.mainEditor = mainEditor;
            sceneController.OnSceneLoaded += new SceneControllerEvent(sceneController_OnSceneLoaded);
            sceneController.OnSceneUnloading += new SceneControllerEvent(sceneController_OnSceneUnloading);
            timer.addFixedUpdateListener(this);

            mainEditor.ActiveInterfaceChanged += new ObjectEditorGUIEvent(mainEditor_ActiveInterfaceChanged);
            mainEditor.FieldChanged += new ObjectEditorGUIEvent(mainEditor_FieldChanged);
            mainEditor.MainInterfaceChanged += new ObjectEditorGUIEvent(mainEditor_MainInterfaceChanged);
        }

        void mainEditor_MainInterfaceChanged(EditInterface editInterface, object editingObject)
        {
            //Check for an instance object, if it is found set the debug surface position to that instance.
            instance = editingObject as Instance;
            updatePosition();
        }

        void mainEditor_FieldChanged(EditInterface editInterface, object editingObject)
        {
            if (currentRenderer != null)
            {
                currentRenderer.propertiesChanged(debugSurface);
            }
        }

        void mainEditor_ActiveInterfaceChanged(EditInterface editInterface, object editingObject)
        {
            if (currentRenderer != null)
            {
                currentRenderer.interfaceDeselected(debugSurface);
            }
            if(debugSurface != null)
            {
                debugSurface.clearAll();
            }
            currentRenderer = editInterface.Renderer;
            if (currentRenderer != null)
            {
                currentRenderer.interfaceSelected(debugSurface);
            }
        }

        void sceneController_OnSceneLoaded(SceneController controller, SimScene scene)
        {
            debugSurface = renderer.createDebugDrawingSurface("EditInterfaceRenderer", scene.getDefaultSubScene());
            if (debugSurface != null)
            {
                debugSurface.moveOrigin(currentOrigin);
                debugSurface.setOrientation(currentRotation);
            }
        }

        void sceneController_OnSceneUnloading(SceneController controller, Engine.ObjectManagement.SimScene scene)
        {
            if(debugSurface != null)
            {
                renderer.destroyDebugDrawingSurface(debugSurface);
            }
        }

        private void updatePosition()
        {
            if (instance != null)
            {
                currentOrigin = instance.Translation;
                currentRotation = instance.Definition.Rotation;
            }
            else
            {
                currentOrigin = Vector3.Zero;
                currentRotation = Quaternion.Identity;
            }
            if (debugSurface != null)
            {
                debugSurface.moveOrigin(currentOrigin);
                debugSurface.setOrientation(currentRotation);
            }
        }

        #region UpdateListener Members

        public void sendUpdate(Clock clock)
        {
            if (currentRenderer != null)
            {
                updatePosition();
                currentRenderer.frameUpdate(debugSurface);
            }
        }

        public void loopStarting()
        {
            
        }

        public void exceededMaxDelta()
        {
            
        }

        #endregion
    }
}
