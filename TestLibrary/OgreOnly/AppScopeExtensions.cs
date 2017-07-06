using Autofac;
using Autofac.Builder;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Anomalous.Minimus.OgreOnly
{
    enum LifetimeScopes
    {
        App,
        Scene
    }

    static class AppScopeExtensions
    {
        public static IRegistrationBuilder<TLimit, TActivatorData, TRegistrationStyle> InstancePerApp<TLimit, TActivatorData, TRegistrationStyle>(this IRegistrationBuilder<TLimit, TActivatorData, TRegistrationStyle> builder)
        {
            return builder.InstancePerMatchingLifetimeScope(LifetimeScopes.App);
        }

        public static IRegistrationBuilder<TLimit, TActivatorData, TRegistrationStyle> InstancePerScene<TLimit, TActivatorData, TRegistrationStyle>(this IRegistrationBuilder<TLimit, TActivatorData, TRegistrationStyle> builder)
        {
            return builder.InstancePerMatchingLifetimeScope(LifetimeScopes.Scene);
        }
    }
}
