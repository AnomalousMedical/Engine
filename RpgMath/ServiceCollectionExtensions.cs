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
            services.AddSingleton<IDamageCalculator, DamageCalculator>();
            services.AddSingleton<IXpCalculator, XpCalculator>();
            services.AddSingleton<ITurnTimer, TurnTimer>();
            services.AddScoped<ICharacterTimer, CharacterTimer>();
            services.AddScoped<ILevelCalculator, LevelCalculator>();

            return services;
        }
    }
}
