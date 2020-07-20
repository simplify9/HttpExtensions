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
        public static IServiceCollection AddHttpClient<TInterface, TImplementation, TOptions>(this IServiceCollection serviceCollection, Action<TOptions> configure = null)
            where TOptions : HttpClientOptionsBase, new()
            where TImplementation : class, TInterface
            where TInterface : class
        {
            var clientOptions = new TOptions();

            if (configure != null) configure.Invoke(clientOptions);

            var serviceProvider = serviceCollection.BuildServiceProvider();
            var configuration = serviceProvider.GetRequiredService<IConfiguration>();

            configuration.GetSection(clientOptions.ConfigurationSection).Bind(clientOptions);

            if (!clientOptions.Token.IsValid())
                configuration.GetSection("Token").Bind(clientOptions.Token);

            serviceCollection.AddSingleton(clientOptions);

            serviceCollection.AddHttpClient<TInterface, TImplementation>(httpClient =>
            {
                httpClient.BaseAddress = new Uri(clientOptions.BaseUrl);
            });

            return serviceCollection;
        }
    }
}
