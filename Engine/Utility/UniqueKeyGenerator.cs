using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Engine
{
    public class UniqueKeyGenerator
    {
        private UniqueKeyGenerator()
        {

        }

        public static String generateStringKey()
        {
            return Guid.NewGuid().ToString();
        }
    }
}
