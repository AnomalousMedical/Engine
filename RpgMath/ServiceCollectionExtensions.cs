using Microsoft.Extensions.DependencyInjection.Extensions;
using RpgMath;
using System;
using System.Collections.Generic;
using System.Text;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class RpgMathServiceCollectionExtensions
    {
        public static IServiceCollection AddRpgMath(this IServiceCollection services)
        {
            services.TryAddSingleton<IDamageCalculator, DamageCalculator>();
            services.TryAddSingleton<IXpCalculator, XpCalculator>();

            return services;
        }
    }
}
