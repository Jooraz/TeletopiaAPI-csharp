using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using TeletopiaAPI.Models;
using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using System.IO;
using System.Net.Http.Headers;
using System.Text;
using System.Xml;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace TeletopiaAPI
{
    public class SenderService
    {
        private const string MAIN_URL = "teletopiasms.no/gateway/v3/json";

        public readonly string[] API_URLS = new string[] {
            "https://api1." + MAIN_URL,
            "https://api2." + MAIN_URL,
            "https://api3." + MAIN_URL
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

        public async Task<SendResponsesArray> SendMessage(SendMessage message)
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
                    HttpResponseMessage result = null;
                    using (var ms = new MemoryStream())
                    {
                        req.Content = await CreateHttpContent(message, ms);
                        result = await _client.SendAsync(req);
                    }

                    if (result != null)
                    {
                        if (result.IsSuccessStatusCode && result.StatusCode == HttpStatusCode.OK)
                        {
                            var options = new JsonSerializerOptions()
                            {
                                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                                PropertyNameCaseInsensitive = true
                            };
                            options.Converters.Add(new LongToBooleanConverter());
                            var outputData = await result.Content.ReadFromJsonAsync<SendResponsesArray>(options);
                            return outputData;
                        }
                        else if (result.StatusCode == HttpStatusCode.RequestTimeout)
                        {
                            continue;
                        }
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
                catch (Exception ex)
                {
                    var x = ex;
                }
            }

            _logger.LogError("Failed to send SMS");
            return null;
        }

        private static async Task<HttpContent> CreateHttpContent(object content, Stream stream)
        {
            HttpContent httpContent = null;

            if (content != null)
            {
                var options = new JsonSerializerOptions()
                {
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase
                };

                await JsonSerializer.SerializeAsync(stream, content, content.GetType(), options);
                stream.Position = 0;
                httpContent = new StreamContent(stream);
                httpContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            }

            return httpContent;
        }
    }

    public class LongToBooleanConverter : JsonConverter<bool>
    {
        public override bool Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            switch (reader.TokenType)
            {
                case JsonTokenType.Number:
                    {
                        var stringValue = reader.GetInt64();
                        return stringValue == 1;
                    }
                case JsonTokenType.String:
                    {
                        var stringValue = reader.GetString();
                        bool output;
                        if (bool.TryParse(stringValue, out output))
                        {
                            return output;
                        }
                    }
                    break;
            }

            return reader.GetBoolean();
        }

        public override void Write(Utf8JsonWriter writer, bool value, JsonSerializerOptions options)
        {
            writer.WriteBooleanValue(value);
        }
    }
}