using DiligentEngine;
using DiligentEngine.RT;
using Engine;
using Engine.Resources;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddDiligentEngineRtShapes(this IServiceCollection services)
        {
            

            return services;
        }

        public static IServiceCollection AddDiligentEngineRt(this IServiceCollection services, Action<RTOptions> configure = null)
        {
            

            return services;
        }
    }
}
