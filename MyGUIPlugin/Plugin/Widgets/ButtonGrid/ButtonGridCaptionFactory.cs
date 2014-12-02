using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyGUIPlugin
{
    /// <summary>
    /// This class creates captions for button grids.
    /// </summary>
    public abstract class ButtonGridCaptionFactory
    {
        /// <summary>
        /// Create a caption.
        /// </summary>
        /// <param name="name">The text to display on the caption.</param>
        /// <returns>A new ButtonGridCaption.</returns>
        protected internal abstract ButtonGridCaption createCaption(String name);

        /// <summary>
        /// Destroy a caption.
        /// </summary>
        /// <param name="caption">The caption to destroy.</param>
        protected internal abstract void destroyCaption(ButtonGridCaption caption);

        /// <summary>
        /// Set the ButtonGrid for this caption.
        /// </summary>
        /// <param name="grid">The grid to set.</param>
        internal void setButtonGrid(ButtonGrid grid)
        {
            this.Grid = grid;
            gridSet();
        }

        /// <summary>
        /// The ButtonGrid to create captions on.
        /// </summary>
        protected ButtonGrid Grid { get; private set; }

        /// <summary>
        /// Called when the grid is set for this factory.
        /// </summary>
        protected abstract void gridSet();
    }
}
