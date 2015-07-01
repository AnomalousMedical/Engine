using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OgrePlugin
{
    public abstract class MaterialBuilder
    {
        /// <summary>
        /// Build material(s) for the given description, add the materials created to the repo.
        /// </summary>
        /// <param name="description">The description to use to build material(s).</param>
        /// <param name="repo">The repository to use to store the materials. Put the results there.</param>
        public abstract void buildMaterial(MaterialDescription description, MaterialRepository repo);

        /// <summary>
        /// This will be called for all materials when they need to be cleaned up. This includes all variants
        /// made during buildMaterial.
        /// </summary>
        /// <param name="materialPtr">The MaterialPtr to destroy.</param>
        public abstract void destroyMaterial(MaterialPtr materialPtr);

        /// <summary>
        /// The name of this builder, used in material files to specify the builder they wish to use.
        /// </summary>
        public abstract String Name { get; }
    }
}
