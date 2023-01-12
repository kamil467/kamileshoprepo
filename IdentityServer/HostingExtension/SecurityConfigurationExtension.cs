namespace IdentityServer.HostingExtension
{
    public static class SecurityConfigurationExtension
    {
        public static void AddIdentityServerConfiguration(this IServiceCollection services)
        {
            services.AddIdentityServer()
                .AddInMemoryClients(Configuration.ServerConfig.GetClients()) // adding clients
                .AddInMemoryApiScopes(Configuration.ServerConfig.GetApiScopes());  // adding scopes
            
        }
    }
}
