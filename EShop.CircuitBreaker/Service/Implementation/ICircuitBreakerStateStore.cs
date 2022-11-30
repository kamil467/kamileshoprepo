using System;

namespace EShop.CircuitBreaker.Service.Interface
{
    /// <summary>
    /// State of the circuitbreaker.
    /// </summary>
    public enum CircuitBreakerStateEnum { 
    
        /// <summary>
        /// State is Closed.Service is healthy.can accpet request.
        /// </summary>
        Closed=1, 

        /// <summary>
        /// State is Open, service cannot accept any request.
        /// </summary>
        Open=2,

        /// <summary>
        /// Service is recovering.
        /// </summary>
        HalfOpen=3
    }

    /// <summary>
    /// This is interface provides the state functionalities of circuitBreaker.
    /// </summary>
   public interface ICircuitBreakerStateStore
	{
        /// <summary>
        /// State of the circuit breaker.
        /// </summary>
        CircuitBreakerStateEnum State { get;  }
        
        /// <summary>
        /// last exception which caused the breaker to open.
        /// </summary>
        Exception LastException { get; }

        DateTime  LastStateChangedDateTimeInUTC { get; }

        /// <summary>
        /// Move state to Open and record the exception with laststatechangetime.
        /// </summary>
        /// <param name="ecxception"></param>
        void Trip(Exception ecxception);

        /// <summary>
        /// move state to Closed.
        /// </summary>
        void Reset();

        /// <summary>
        /// Move state to half open.
        /// </summary>
        void HalfOpen();

        /// <summary>
        /// True - if state is closed else false.
        /// </summary>
        bool IsClosed { get; }    
    }
}
