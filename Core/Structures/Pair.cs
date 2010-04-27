using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Engine
{
    /// <summary>
    /// A generic pair class.
    /// </summary>
    /// <typeparam name="F">The type of First.</typeparam>
    /// <typeparam name="S">The type of Second.</typeparam>
    public class Pair<F, S>
    {
        /// <summary>
        /// Constructor.
        /// </summary>
        public Pair()
        {

        }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="first">Value for first.</param>
        /// <param name="second">Value for second.</param>
        public Pair(F first, S second)
        {
            this.First = first;
            this.Second = second;
        }

        /// <summary>
        /// The first item in the pair.
        /// </summary>
        public F First { get; set; }

        /// <summary>
        /// The second item in the pair.
        /// </summary>
        public S Second { get; set; }
    }
}
