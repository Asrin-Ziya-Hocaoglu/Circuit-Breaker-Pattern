using Microsoft.AspNetCore.Mvc;
using Polly.CircuitBreaker;
using System.Net;

namespace MainProject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PingController : ControllerBase
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public PingController(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        [HttpGet]
        public async Task<ActionResult<string>> GetDataFromProvider()
        {
            try
            {
                var client = _httpClientFactory.CreateClient("providerApiClient");

                var response = await client.GetAsync("api/Pong/pong");

                client.Dispose();
                Console.WriteLine(response.Content.ReadAsStringAsync().Result);
                return response.Content.ReadAsStringAsync().Result;
            }
            catch (BrokenCircuitException ex)
            {
                Console.WriteLine("Switching another provider to connect...");
                Console.WriteLine("\n--------------------------------------------------------------");

                var clientB = _httpClientFactory.CreateClient("providerApiBClient");

                try
                {
                    var responseB = await clientB.GetAsync("api/Pong/pong");
                    Console.WriteLine(responseB.Content.ReadAsStringAsync().Result);
                    clientB.Dispose();
                    return responseB.Content.ReadAsStringAsync().Result;
                }
                catch
                {
                    Console.WriteLine("Provider B is unavailable.");
                    Console.WriteLine("\n--------------------------------------------------------------");
                }

                return String.Empty;
            }

        }
    }
}