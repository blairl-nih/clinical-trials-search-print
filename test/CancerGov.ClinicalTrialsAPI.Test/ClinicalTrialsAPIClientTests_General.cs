using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

using Xunit;

namespace CancerGov.ClinicalTrialsAPI.Test
{
    public class ClinicalTrialsAPIClientTests_General
    {
        [Fact]
        public void NoHttpClient()
        {
            ArgumentNullException ex = Assert.Throws<ArgumentNullException>
            (
                () => new ClinicalTrialsAPIClient(null, "https://www.example.com/", "some key")
            );

            Assert.Equal("client", ex.ParamName);
            Assert.StartsWith(ClinicalTrialsAPIClient.ARGUMENT_NOT_NULL_MSG, ex.Message);
        }


        [Theory]
        [InlineData("")]
        [InlineData(null)]
        public void NoBaseAddress(string baseAddress)
        {
            HttpClient dummyClient = new HttpClient();

            ArgumentException ex = Assert.Throws<ArgumentException>
            (
                () => new ClinicalTrialsAPIClient(dummyClient, baseAddress, "some key")
            );

            Assert.Equal("baseAddress", ex.ParamName);
            Assert.StartsWith(ClinicalTrialsAPIClient.BASE_ADDRESS_REQUIRED_MSG, ex.Message);
        }


        [Theory]
        [InlineData("")]
        [InlineData(null)]
        public void NoBaseAPIKey(string apiKey)
        {
            HttpClient dummyClient = new HttpClient();

            ArgumentException ex = Assert.Throws<ArgumentException>
            (
                () => new ClinicalTrialsAPIClient(dummyClient, "https://www.example.com/", apiKey)
            );

            Assert.Equal("apiKey", ex.ParamName);
            Assert.StartsWith(ClinicalTrialsAPIClient.ARGUMENT_API_KEY_MSG, ex.Message);
        }
    }
}
