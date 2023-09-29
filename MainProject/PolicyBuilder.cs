using Polly;
using Polly.Extensions.Http;
using System.Net;

namespace MainProject
{
    public class PolicyBuilder
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

            .CircuitBreakerAsync(4, TimeSpan.FromSeconds(10), onReset: OnReset, onBreak: OnBreak, onHalfOpen: OnHalfOpen);
        }

        public IAsyncPolicy<HttpResponseMessage> GetRetryrPolicy(HttpStatusCode[] httpStatusCodes)

        {
            return HttpPolicyExtensions

            .HandleTransientHttpError()
            .OrResult(r => httpStatusCodes.Contains(r.StatusCode))
            .RetryAsync(3, onRetry: (exception, retryCount) =>
            {
                Console.WriteLine($" Tekrar Deneniyor => Exception Thrown => : {exception.Exception.Message}");
            });
        }
    }
}