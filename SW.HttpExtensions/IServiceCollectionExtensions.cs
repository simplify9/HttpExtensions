using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;

namespace SW.HttpExtensions
{
    public static class IServiceCollectionExtensions
    {

        public static IServiceCollection AddJwtTokenParameters(this IServiceCollection serviceCollection, Action<JwtTokenParameters> configure = null)
        {
            var jwtTokenParameters = new JwtTokenParameters();
            if (configure != null) configure.Invoke(jwtTokenParameters);

            var serviceProvider = serviceCollection.BuildServiceProvider();
            var configuration = serviceProvider.GetRequiredService<IConfiguration>();

            configuration.GetSection(JwtTokenParameters.ConfigurationSection).Bind(jwtTokenParameters);

            serviceCollection.AddSingleton(jwtTokenParameters);

            return serviceCollection;
        }

        public static IServiceCollection AddApiClient<TInterface, TImplementation, TOptions>(this IServiceCollection serviceCollection, Action<TOptions> configure = null)
            where TOptions : ApiClientOptionsBase, new()
            where TImplementation : class, TInterface
            where TInterface : class
        {
            var clientOptions = new TOptions();

            if (configure != null) configure.Invoke(clientOptions);

            var serviceProvider = serviceCollection.BuildServiceProvider();
            var configuration = serviceProvider.GetRequiredService<IConfiguration>();

            configuration.GetSection(clientOptions.ConfigurationSection).Bind(clientOptions);

            if (!clientOptions.Token.IsValid())
                configuration.GetSection(JwtTokenParameters.ConfigurationSection).Bind(clientOptions.Token);

            serviceCollection.AddSingleton(clientOptions);

            serviceCollection.AddHttpClient<TInterface, TImplementation>(httpClient =>
            {
                httpClient.BaseAddress = new Uri(clientOptions.BaseUrl);
            });

            return serviceCollection;
        }
    }
}
