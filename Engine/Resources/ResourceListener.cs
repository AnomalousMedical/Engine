using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Engine.Resources
{
    public interface ResourceListener
    {
        void resourceAdded(ResourceGroup group, Resource resource);
	
	    void resourceRemoved(ResourceGroup group, Resource resource);

	    void resourceGroupAdded(ResourceGroup group);

	    void resourceGroupRemoved(ResourceGroup group);

	    void forceResourceRefresh();
    }
}
