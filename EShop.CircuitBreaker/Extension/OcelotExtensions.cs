using Ocelot.Configuration;
using Ocelot.DependencyInjection;
using Ocelot.Logging;
using Ocelot.Provider.Polly;
using Polly;
using Polly.CircuitBreaker;
using Polly.Extensions.Http;
using Polly.Registry;
using Polly.Timeout;
using Polly.Wrap;
using System.Runtime.CompilerServices;

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

    }

    /// <summary>
    /// Polly Timeout Implementation for Ocelot.
    /// </summary>
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
                // we using Optimistic timeout , honoring CancellationToken
                //CancellationToken.ThrowIfCancellationRequested()  will be called by Polly to throw exception if cancellation requested from outside.
                // Polly internally creates new Cancellation Token and cancel it after timeout expired.  
                result =     this.timeoutPolicy()
                     .ExecuteAsync( async  (ct) =>
                     {
                         return   await base.SendAsync(request, ct);
                     },cancellationToken); // we no need to pass Cancellation token here. CancellationToken.None also work.

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

    /// <summary>
    /// Polly Circuit Breaker Implementation for Ocelot.
    /// </summary>
    public class PollyWithCircuitBreaker : DelegatingHandler
    {
        private static AsyncCircuitBreakerPolicy breakerPolicy;
        static PollyWithCircuitBreaker()
        {
            //breakerPolicy = Policy
            //           .Handle<Exception>()
            //                   .CircuitBreakerAsync(
            //            exceptionsAllowedBeforeBreaking: 5, // circuit will break after 2 failure attempts
            //              durationOfBreak: TimeSpan.FromMinutes(5), // circuit will be reset after 5 minutes
            //              onBreak: onBreakAction,
            //              onReset: onReset
            //                    );


            // below conbfiguration tell us that
            // within 2 minutes if more than 50 % request failed then move the circuit breaker state to Open State.
            // minimumThroughput - this many calls must have passed through the circuit within the
            // active samplingDuration for the circuit to consider breaking.
            // advanced circuit breaker configuration.
            breakerPolicy = Policy
                                        .Handle<Exception>()
                                        .AdvancedCircuitBreakerAsync(
                                         failureThreshold: 0.5, // failure percentage when Circuit breaker has to activate
                                         durationOfBreak: TimeSpan.FromMinutes(5), // duration of Open State.
                                         minimumThroughput: 2, // number of request in sampling time. failures will be considered if happen within sampling time.
                                         samplingDuration: TimeSpan.FromMinutes(2)); // sampling duration.

        }
            
       static Action<Exception, TimeSpan> onBreakAction = (exception, timespan) =>
        {
            Console.WriteLine("Circuit Broken:"+exception);
        };
        static Action onReset = () =>
        {
            Console.WriteLine("Reset the breaker");
        };
        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            try
            {
                var v = breakerPolicy.CircuitState;

                var result = breakerPolicy.ExecuteAsync(async (ct) =>
                  {
                      return await base.SendAsync(request, cancellationToken);
                  }, cancellationToken);

                return result;
            }
            catch(Exception e)
            {

            }
            return null;
        }
    }

    public class PollyCircuitBreakerAndTimeOut : DelegatingHandler
    {
        /// <summary>
        /// PolicyWrap Instance
        /// </summary>
        private static AsyncPolicyWrap _PolicyWrap;

        #region CircuitBreaker

        /// <summary>
        /// CircuitBreaker Policy , Break the circuit after 2 failure attempts and keep the circuit open for 5 minutes.
        /// </summary>
        /// <returns></returns>
        private static IAsyncPolicy breakerPolicy () => Policy.Handle<Exception>()
                                               .CircuitBreakerAsync(2, TimeSpan.FromMinutes(5));


        /// <summary>
        /// Action to be executed when Circuit gets Open.
        /// </summary>
        static Action<Exception, TimeSpan> onBreakAction = (exception, timespan) =>
        {
            Console.WriteLine("Circuit Broken:" + exception);
        };

        /// <summary>
        /// Action to be executed when Circuit resets.
        /// </summary>
        static Action onReset = () =>
        {
            Console.WriteLine("Reset the breaker");
        };
        #endregion

        #region Timeout
        /// <summary>
        /// TimeoutPolicy, request will be timedOut if no response received with-in 3 seconds.
        /// </summary>
        private static IAsyncPolicy timeoutPolicy ()=> Policy
                                                               .TimeoutAsync(
                                                               3,
                                                               TimeoutStrategy.Optimistic);
        #endregion

        /// <summary>
        /// Policy Wrap building.Creates Policy Instances only once.
        /// </summary>
        static PollyCircuitBreakerAndTimeOut()
        {
            // Wrap the policy Outer Policy : CircuitBreaker, Inner Policy = TimeOut
            _PolicyWrap = Policy.WrapAsync(breakerPolicy(), timeoutPolicy());
        }
 
        /// <summary>
        /// Override HttpRequest of Ocelot.
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            try
            {
                // to check Circuit Breaker State.
                var state = (ICircuitBreakerPolicy)breakerPolicy();

                //execute request with polcies
                return await _PolicyWrap.ExecuteAsync(async (ct) =>
                {
                    return await base.SendAsync(request, ct);
                }, cancellationToken);

            }
            catch(Exception e)
            {

            }
            return null;
        }
    }
}
