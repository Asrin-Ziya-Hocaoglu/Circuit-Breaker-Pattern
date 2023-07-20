using Polly;
using Polly.Extensions.Http;

namespace MainProject
{
    public class CircuitBreakerPolicy
    {
        public void OnHalfOpen()

        {
            Console.WriteLine("Circuit in test mode, one request will be allowed.");
            Console.WriteLine("\n--------------------------------------------------------------");
        }

        public void OnReset()

        {
            Console.WriteLine("Circuit closed, requests flow normally.");
            Console.WriteLine("\n--------------------------------------------------------------");
        }

        public void OnBreak(DelegateResult<HttpResponseMessage> result, TimeSpan durationOfBreak)

        {
            Console.WriteLine("Provider A is Unavailable, please try later on");
            Console.WriteLine($"Duration Of Break is : {durationOfBreak}");
            Console.WriteLine("Circuit cut, requests will not flow.");
            Console.WriteLine("\n--------------------------------------------------------------");

        }

        public IAsyncPolicy<HttpResponseMessage> GetCircuitBreakerPolicy()

        {
            return HttpPolicyExtensions

            .HandleTransientHttpError()

            .CircuitBreakerAsync(3, TimeSpan.FromSeconds(10), onReset: OnReset, onBreak: OnBreak, onHalfOpen: OnHalfOpen);
        }
    }
}