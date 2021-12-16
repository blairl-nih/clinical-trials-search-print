using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

using Common.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace CancerGov.ClinicalTrialsAPI
{
    public class ClinicalTrialsAPIClient : IClinicalTrialsAPIClient
    {
        public const string ARGUMENT_NOT_NULL_MSG = "Argument must not be null.";
        public const string BASE_ADDRESS_REQUIRED_MSG = "A base address is required";
        public const string ARGUMENT_API_KEY_MSG = "A valid API key is required.";
        public const string INVALID_BASE_URL_MSG = "Must be a valid, absolute URL.";

        public const string JSON_CONTENT = "application/json";

        static ILog log = LogManager.GetLogger(typeof(ClinicalTrialsAPIClient));

        private HttpClient _client = null;

        /// <summary>
        /// Creates a new instance of a Clinicaltrials API client.
        /// </summary>
        /// <param name="client">An instance of HttpClient for making the actual HTTP calls.
        /// </param>
        /// <param name="baseAddress">The base address of the API. (e.g. "https://clinicaltrialsapi.cancer.gov/api/v2/")</param>
        /// <param name="apiKey">The API key</param>
        /// <remarks>The trials API always gzips the response, regardless of the accepted encodings. As a result, the HttpClient instance must be constructed with an
        /// HttpClientHandler which has the AutomaticDecompression property set to allow gzip. Anticipating future changes, deflate is also required.
        /// 
        /// The classic .Net framework does not support brotli compression.
        /// </remarks>
        public ClinicalTrialsAPIClient(HttpClient client, string baseAddress, string apiKey)
        {
            if (client == null)
                throw new ArgumentNullException(nameof(client), ARGUMENT_NOT_NULL_MSG);
            if (String.IsNullOrWhiteSpace(baseAddress))
                throw new ArgumentException(BASE_ADDRESS_REQUIRED_MSG, nameof(baseAddress));
            if (String.IsNullOrWhiteSpace(apiKey))
                throw new ArgumentException(ARGUMENT_API_KEY_MSG, nameof(apiKey));

            baseAddress = baseAddress.Trim();
            if (!baseAddress.EndsWith("/"))
                baseAddress += '/';

            Uri baseURI = new Uri(baseAddress);
            if (!(baseURI.IsAbsoluteUri && baseURI.IsWellFormedOriginalString()))
                throw new ArgumentException(INVALID_BASE_URL_MSG, nameof(baseAddress));

            client.BaseAddress = baseURI;
            client.DefaultRequestHeaders.Add("x-api-key", apiKey);
            client.DefaultRequestHeaders.AcceptEncoding.Add(new System.Net.Http.Headers.StringWithQualityHeaderValue("gzip"));
            client.DefaultRequestHeaders.AcceptEncoding.Add(new System.Net.Http.Headers.StringWithQualityHeaderValue("deflate"));

            this._client = client;
        }

        /// <summary>
        /// Retrieve a single clinical trial from the API via its ID.
        /// </summary>
        /// <param name="id">Either the NCI ID or the NCT ID</param>
        /// <returns>JSON object representing the clinical trial</returns>

        async public Task<JObject> GetOneTrial(string id)
        {
            if (String.IsNullOrWhiteSpace(id))
            {
                throw new ArgumentNullException("The trial identifier is null or an empty string");
            }

            JObject rtnTrial = null;

            // Get the HTTP response content from GET request
            HttpContent httpContent = await ReturnGetRespContent("trials", id);
            if(httpContent != null)
            {
                string responseBody = await httpContent.ReadAsStringAsync();
                rtnTrial = JObject.Parse(responseBody);
            }

            return rtnTrial;
        }

        /// <summary>
        /// Retrieve the details of a list of trials.
        /// </summary>
        /// <param name="trialIDs">The set of trials to retrieve</param>
        /// <returns>An object containing an array of trial details.</returns>
#pragma warning disable CS1998
        async public Task<JObject> GetMultipleTrials(
            IEnumerable<string> trialIDs
        )
        {
            throw new NotImplementedException();
        }
#pragma warning restore

        /// <summary>
        /// Gets a collection of disease details from the API.
        /// </summary>
        /// <param name="diseaseCodes">List of disease concept codes to retrieve.</param>
        /// <returns>Collection of disease details</returns>
#pragma warning disable CS1998
        async public Task<IEnumerable<JObject>> LookupDiseaseCodes(
            IEnumerable<string> diseaseCodes
        )
        {
            throw new NotImplementedException();
        }
#pragma warning restore

        /// <summary>
        /// Gets a collection of interventions from the API.
        /// </summary>
        /// <param name="interventionCodes">List of intervention concept codes to retrieve.</param>
        /// <returns>Collection of intervention details</returns>
#pragma warning disable CS1998
        async public Task<IEnumerable<JObject>> LookupInterventionCodes(
            IEnumerable<string> interventionCodes
        )
        {
            throw new NotImplementedException();
        }
#pragma warning restore


        /// <summary>
        /// Gets the response content of a GET request.
        /// </summary>
        /// <param name="path">Path for client address</param>
        /// <param name="param">Param in URL</param>
        /// <returns>HTTP response content</returns>
        async protected Task<HttpContent> ReturnGetRespContent(String path, String param)
        {
            //NOTE: When using HttpClient.BaseAddress as we are, the path must not have a preceeding slash
            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, path + "/" + param);
            request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue(JSON_CONTENT));

            HttpContent content = null;
            HttpResponseMessage response = await _client.SendAsync(request);
            if (response.IsSuccessStatusCode)
            {
                content = response.Content;
            }
            else
            {
                string errorResponse = null;
                if(response.Content != null)
                    errorResponse = await response.Content.ReadAsStringAsync();
                string errorMessage = $"Response: '{errorResponse}' \nAPI path: {this._client.BaseAddress.ToString() + path + "/" + param}";
                if (response.StatusCode == HttpStatusCode.NotFound)
                {
                    // If trial is not found, log 404 message and return content as null
                    log.Debug(errorMessage);
                }
                else
                {
                    // If response is other error message, log and throw exception
                    log.Error(errorMessage);
                    throw new APIServerErrorException(errorMessage);
                }
            }

            return content;
        }


        /// <summary>
        /// Gets the response content of a POST request.
        /// </summary>
        /// <param name="path">Path for client address</param>
        /// <param name="request">Params passed in with request body</param>
        /// <returns>HTTP response content</returns>
        async protected Task<HttpContent> ReturnPostRespContent(String path, JObject requestBody)
        {
            HttpResponseMessage response = null;
            HttpContent content = null;

            _client.DefaultRequestHeaders.Accept.Clear();
            _client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

            //We want this to be synchronus, so call Result right away.
            //NOTE: When using HttpClient.BaseAddress as we are, the path must not have a preceeding slash
            response = await _client.PostAsync(path, new StringContent(requestBody.ToString(), Encoding.UTF8, "application/json"));
            if (response.IsSuccessStatusCode)
            {
                content = response.Content;
            }
            else
            {
                //TODO: Add more checking here if the respone does not actually have any content
                string errorMessage = "Response: " + response.Content.ReadAsStringAsync().Result + "\nAPI path: " + this._client.BaseAddress.ToString() + path;
                throw new Exception(errorMessage);
            }

            return content;
        }


    }
}
