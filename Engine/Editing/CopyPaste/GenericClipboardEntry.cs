using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Engine.Editing
{
    public sealed class GenericClipboardEntry : ClipboardEntry
    {
        private Type objectType;

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="objectType">The type of the object this interface represents.</param>
        /// <param name="supportsPastingTypeFunction">The callback to determine if this interface can paste a source type. This can be null, but everything will be rejected.</param>
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

        public delegate bool SupportsPastingTypeDelegate(Type type);

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

        public bool supportsPastingType(Type type)
        {
            if (SupportsPastingTypeFunction != null)
            {
                return SupportsPastingTypeFunction.Invoke(type);
            }
            return ObjectType.IsAssignableFrom(type);
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

        /// <summary>
        /// This method must be implemented to accept pastes. It will be called
        /// to see if the type passed is supported. If this is null it will
        /// check the passed type against the type assigned to this entry.
        /// </summary>
        public SupportsPastingTypeDelegate SupportsPastingTypeFunction { get; set; }
    }
}
