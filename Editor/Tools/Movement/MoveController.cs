using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Engine;

namespace Editor
{
    public delegate void TranslationChanged(Vector3 newTranslation, Object sender);

    public class MoveController
    {
        public event TranslationChanged OnTranslationChanged;

        private Vector3 currentTranslation;

        public MoveController()
        {

        }

        public void setTranslation(ref Vector3 translation, Object sender)
        {
            currentTranslation = translation;
            if (OnTranslationChanged != null)
            {
                OnTranslationChanged.Invoke(translation, sender);
            }
        }

        public Vector3 Translation
        {
            get
            {
                return currentTranslation;
            }
        }
    }
}
