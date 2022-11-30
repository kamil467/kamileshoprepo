using EShop.CircuitBreaker.Service.Interface;

namespace EShop.CircuitBreaker.Service.Implementation
{
    /// <summary>
    /// This is to maintain the state of CircuitBreaker.
    /// </summary>
    public class InMemoryCircuitBreakerStateStore : ICircuitBreakerStateStore
    {
        public CircuitBreakerStateEnum State => throw new NotImplementedException();

        public Exception LastException => throw new NotImplementedException();

        public DateTime LastStateChangedDateTimeInUTC => throw new NotImplementedException();

        public bool IsClosed => throw new NotImplementedException();

        /// <summary>
        /// Move the state from Open to HalfOpen.
        /// </summary>
        /// <exception cref="NotImplementedException"></exception>
        public void HalfOpen()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Move the state to Half-Open to Closed.
        /// </summary>
        /// <exception cref="NotImplementedException"></exception>
        public void Reset()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Move the state from Closed to Open.
        /// </summary>
        /// <param name="ecxception"></param>
        /// <exception cref="NotImplementedException"></exception>
        public void Trip(Exception ecxception)
        {
            throw new NotImplementedException();
        }
    }
}
