using EShop.CircuitBreaker.Service.Interface;
using Microsoft.Extensions.Caching.Memory;

namespace EShop.CircuitBreaker
{
    public class CircuitBreakerMain
    {
        /// <summary>
        /// Instance of IcircuitBreakerStateStore.
        /// </summary>
        private readonly ICircuitBreakerStateStore _stateStore;

        /// <summary>
        /// # minutes waiting time for Open to half_Open
        /// </summary>
        private readonly TimeSpan? opentoHalfOpenWaitTime = TimeSpan.FromMinutes(3); 

        private readonly IMemoryCache _memoryCache;
        /// <summary>
        /// Constructor Initialization.
        /// </summary>
        /// <param name="stateStore"></param>
        public CircuitBreakerMain(ICircuitBreakerStateStore stateStore, IMemoryCache memoryCache)
        {
            _stateStore = stateStore;
            this._memoryCache = memoryCache;
        }

        /// <summary>
        /// Used for limiting the access while circuitBreaker state in Half-Open.
        /// </summary>
        private readonly object halfOpenSyncObject = new object();


        public bool IsClosed { get { return this._stateStore.IsClosed; } }

        public bool IsOpen { get { return !IsClosed; } }


        public void Execution(Action action)
        {
            if(this.IsOpen)
            {
              
                
                if(this._stateStore.State == CircuitBreakerStateEnum.Open)
                {
                    // run the timer for moving the state.
                    //or
                    // perform health check at specified interval
                    // and if success then move the state to Half-Open

                    if(this._stateStore.LastStateChangedDateTimeInUTC + this.opentoHalfOpenWaitTime.Value >= DateTime.UtcNow)
                    {
                        // move the state to Half-Open.
                        this._stateStore.HalfOpen();
                    }
                    else
                    {
                        // return the response immediately.
                        // add -reason: Circuit breaker is still open.
                    }


                }
                else if ( this._stateStore.State == CircuitBreakerStateEnum.HalfOpen)
                {
                    // set threshold value of success attempts.
                    // if success request >= threshold value 
                    // then  move the state to Closed.
                    // else move the state to Open.
                }


            }

            try
            {
                //perform request operation to remote server.
                action();
            }
            catch(Exception exe)
            {


                this.TrackException(exe);
                throw;
            }

        }

        public void TrackException(Exception exception)
        {
            // if not success.

            // count the number of failed attempts.
         
            // failed attempts >= max attempts.

            // move the state to Open.

            // number of attempts counter should be associted with expiration.
            this._stateStore.Trip(exception);
        }
    }
}
