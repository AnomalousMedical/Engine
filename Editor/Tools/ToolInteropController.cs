using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Engine;

namespace Editor
{
    /// <summary>
    /// This class provides a simple way to make the tools communicate with each
    /// other. This is optional and the intertool communication can be handled
    /// without this class if desired.
    /// </summary>
    public class ToolInteropController
    {
        private MoveController moveController;
        private SelectionController selectionController;
        private RotateController rotateController;

        public ToolInteropController()
        {

        }

        public void setMoveController(MoveController moveController)
        {
            this.moveController = moveController;
            moveController.OnTranslationChanged += onTranslationChanged;
        }

        void onTranslationChanged(Vector3 newTranslation, object sender)
        {
            if (selectionController != null)
            {
                selectionController.translateSelectedObject(ref newTranslation);
            }
        }

        public void setRotateController(RotateController rotateController)
        {
            this.rotateController = rotateController;
            rotateController.OnRotationChanged += onRotationChanged;
        }

        void onRotationChanged(Quaternion newRotation, object sender)
        {
            if (selectionController != null)
            {
                selectionController.rotateSelectedObject(ref newRotation);
            }
        }

        public void setSelectionController(SelectionController selectionController)
        {
            this.selectionController = selectionController;
            selectionController.OnSelectionChanged += onSelectionChanged;
            selectionController.OnSelectionRotated += onSelectionRotated;
            selectionController.OnSelectionTranslated += onSelectionTranslated;
        }

        void onSelectionTranslated(Vector3 newPosition)
        {
            if (moveController != null)
            {
                moveController.setTranslation(ref newPosition, selectionController);
            }
        }

        void onSelectionRotated(Quaternion newRotation)
        {
            if (rotateController != null)
            {
                rotateController.setRotation(ref newRotation, selectionController);
            }
        }

        void onSelectionChanged(SelectionChangedArgs args)
        {
            if (moveController != null)
            {
                Vector3 translation = args.Owner.getSelectionTranslation();
                moveController.setTranslation(ref translation, selectionController);
            }
        }
    }
}
