using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Engine.Editing
{
    public class GenericClipboardEntry : ClipboardEntry
    {
        private Type objectType;

        public GenericClipboardEntry(Type objectType)
        {
            this.objectType = objectType;
        }

        /// <summary>
        /// This delegate is used for cut and copy operations.
        /// </summary>
        /// <returns>The object.</returns>
        public delegate object GetObjectDelegate();

        /// <summary>
        /// This delegate is used for paste operations.
        /// </summary>
        /// <param name="pasted">The object to paste.</param>
        public delegate void PasteDelegate(Object pasted);

        public object copy()
        {
            return CopyFunction.Invoke();
        }

        public object cut()
        {
            return CutFunction.Invoke();
        }

        public void paste(object pasted)
        {
            PasteFunction.Invoke(pasted);
        }

        public Type ObjectType
        {
            get
            {
                return objectType;
            }
        }

        public IEnumerable<Type> SupportedTypes { get; set; }

        public bool SupportsCopy
        {
            get { return CopyFunction != null; }
        }

        public bool SupportsCut
        {
            get { return CutFunction != null; }
        }

        public bool SupportsPaste
        {
            get { return PasteFunction != null; }
        }

        public GetObjectDelegate CopyFunction { get; set; }

        public GetObjectDelegate CutFunction { get; set; }

        public PasteDelegate PasteFunction { get; set; }
    }
}
