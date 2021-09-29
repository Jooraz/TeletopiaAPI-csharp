using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using TeletopiaAPI.Models;
using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace TeletopiaAPI
{
    public class SenderService
    {
        private const string MAIN_URL = "teletopiasms.no";

        public readonly string[] API_URLS = new string[] {
            "api1." + MAIN_URL,
            "api2." + MAIN_URL,
            "api3." + MAIN_URL
        };

        private HttpClient _client;
        private ILogger _logger;
        private UsernamePassword _userPass;

        public SenderService(HttpClient client, UsernamePassword userPass, ILogger logger = null)
        {
            _client = client;
            if (logger == null)
            {
                logger = NullLogger.Instance;
            }
            _logger = logger;
            _userPass = userPass;
        }

        public async Task<SendResponse> SendMessage(SendMessage message)
        {
            if (message.Auth is null)
            {
                if (_userPass is null)
                {
                    throw new NullReferenceException("Auth is not set");
                }
                message.Auth = _userPass;
            }

            foreach (var url in API_URLS)
            {
                var req = new HttpRequestMessage(HttpMethod.Post, url);
                try
                {
                    var result = await _client.SendAsync(req);

                    if (result.IsSuccessStatusCode && result.StatusCode == HttpStatusCode.OK)
                    {
                        return await result.Content.ReadFromJsonAsync<SendResponse>();
                    }
                    else if (result.StatusCode == HttpStatusCode.RequestTimeout)
                    {
                        continue;
                    }
                }
                catch (OperationCanceledException) //actually TimeoutException
                {
                    // todo: any processing actually?
                    continue;
                }
                catch (HttpRequestException hre)
                {
                    _logger.LogError(hre.Message);
                }
            }

            _logger.LogError("Failed to send SMS");
            return null;
        }
    }
}