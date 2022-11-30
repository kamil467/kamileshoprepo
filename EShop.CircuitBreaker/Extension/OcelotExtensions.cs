using Ocelot.Configuration;
using Ocelot.DependencyInjection;
using Ocelot.Logging;
using Ocelot.Provider.Polly;
using Polly;
using Polly.Extensions.Http;
using Polly.Registry;
using Polly.Timeout;

namespace EShop.CircuitBreaker.Extension
{
    public static  class OcelotExtensions
    {
        public static void AddCustomTimeOut(this IOcelotBuilder ocelotBuilder, IServiceCollection services)
        {
            
            var timeOutPolicy = Policy.TimeoutAsync<HttpResponse>(3,TimeoutStrategy.Optimistic);
            var policyRegistry = new PolicyRegistry();
            policyRegistry.Add("timeout", timeOutPolicy);

            services.AddHttpClient().AddPolicyRegistry(policyRegistry); // available in microsoft policy extensions.

        }

        public static IAsyncPolicy<HttpResponseMessage> GetTimeoutPolicy()
        {
            //var v =  HttpPolicyExtensions
            //    .HandleTransientHttpError()
            //    .OrResult(msg => msg.StatusCode == System.Net.HttpStatusCode.NotFound)
            //    .WaitAndRetryAsync(6, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2,
            //                                                                retryAttempt)));
            return Policy.TimeoutAsync<HttpResponseMessage>(3, TimeoutStrategy.Optimistic);

        }

    }

    public class PollyWithTimeOutDelegatingHandler : DelegatingHandler
    {
        /// <summary>
        /// Ocelot logger
        /// </summary>
        private readonly IOcelotLogger ocelotLogger;
        
        /// <summary>
        /// TimeoutPolicy
        /// </summary>
        private  IAsyncPolicy<HttpResponseMessage> timeoutPolicy () => Policy.TimeoutAsync<HttpResponseMessage>(3, TimeoutStrategy.Optimistic);


       

        /// <summary>
        /// Provides Granular level control for HttpClient.
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            Task<HttpResponseMessage> result = null;
            try
            {
              result =     this.timeoutPolicy()
                     .ExecuteAsync( async  (ct) =>
                     {
                         return   await base.SendAsync(request, ct);
                     },cancellationToken);

                return  await result;
            }
            catch(TimeoutRejectedException execption)
            {
                // perform call Fallback.
            }
            catch(Exception exe)
            {
                // print common exception here
            }

            return null;
        }
    }
}
