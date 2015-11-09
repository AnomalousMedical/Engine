using Engine;
using MyGUIPlugin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Anomalous.GuiFramework
{
    public abstract class TaskMenuAdProvider
    {
        public enum Alignment
        {
            Horizontal,
            Vertical,
            Hidden
        }

        public event Action<TaskMenuAdProvider> AdCreated;
        public event Action<TaskMenuAdProvider> AdDestroyed;
        public event Action<TaskMenuAdProvider> LayoutChanged;

        private bool isAdCreated = false;
        private Alignment adAlignment = Alignment.Vertical;
        private IntRect adRect;
        private bool changedProperties = false;

        /// <summary>
        /// Ad right when left aligned
        /// </summary>
        public int Right { get; set; }

        /// <summary>
        /// Ad top when bottom aligned
        /// </summary>
        public int Top { get; set; }

        public IntRect AdRect
        {
            get
            {
                return adRect;
            }
            set
            {
                if(adRect != value)
                {
                    adRect = value;
                    changedProperties = true;
                }
            }
        }

        public bool IsAdCreated
        {
            get
            {
                return isAdCreated;
            }
        }

        public Alignment AdAlignment
        {
            get
            {
                return adAlignment;
            }
            set
            {
                if(adAlignment != value)
                {
                    adAlignment = value;
                    changedProperties = true;
                }
            }
        }

        internal void fireLayoutChanged()
        {
            if (changedProperties)
            {
                if (LayoutChanged != null)
                {
                    LayoutChanged.Invoke(this);
                }
                changedProperties = false;
            }
        }

        protected internal Widget ParentWidget { get; set; }

        protected void fireAdCreated()
        {
            isAdCreated = true;
            if (AdCreated != null)
            {
                AdCreated.Invoke(this);
            }
        }

        protected void fireAdDestroyed()
        {
            if (AdDestroyed != null)
            {
                AdDestroyed.Invoke(this);
            }
            isAdCreated = false;
        }
    }
}
