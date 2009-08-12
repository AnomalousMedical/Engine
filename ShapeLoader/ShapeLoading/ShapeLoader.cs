using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Engine.Resources;

namespace Engine
{
    /// <summary>
    /// This class allows generic loading of shapes in a variety of formats.
    /// </summary>
    public abstract class ShapeLoader
    {
        protected String extension;

        /// <summary>
        /// Constructor, takes an extension for files this loader can load.
        /// </summary>
        /// <param name="extension">The extension for files loaded by this loader.</param>
        public ShapeLoader(String extension)
        {
            this.extension = extension;
        }

        /// <summary>
        /// Returns the extension used by this loader.
        /// </summary>
        /// <returns>The extension.</returns>
        public String getExtension()
        {
            return extension;
        }

        /// <summary>
        /// Returns true if the file specified can be loaded by this loader.
        /// </summary>
        /// <param name="filename">The file to test.</param>
        /// <returns>True if the file can be loaded using this loader.</returns>
        public abstract bool canLoadShape(String filename);

        /// <summary>
        /// Loads the contents of the given file and bulids the shapes using the specified
        /// shape builder.
        /// </summary>
        /// <param name="builder">The builder to build the shapes with.</param>
        /// <param name="filename">The filename to load the shapes from.</param>
        public abstract void loadShapes(ShapeBuilder builder, String filename, Archive archive);
    }
}
