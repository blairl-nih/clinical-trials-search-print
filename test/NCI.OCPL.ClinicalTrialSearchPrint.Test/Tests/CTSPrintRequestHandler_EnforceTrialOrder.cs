using System.IO;

using Newtonsoft.Json.Linq;
using Xunit;

using NCI.Test.IO;

namespace NCI.OCPL.ClinicalTrialSearchPrint.Test
{
    public class CTSPrintRequestHandler_EnforceTrialOrder
    {
        /// <summary>
        /// Verify a list of trials is rearranged into the
        /// expected order.
        /// </summary>
        [Fact]
        public void CorrectlyOrdered()
        {
            // The order we want them in.
            string[] trialIDOrder = { "NCI-2018-03694", "NCI-2020-00544", "NCI-2018-01575" };

            // Trials as returned by the CTS API.
            JObject trials = (JObject)TestFileTools.GetTestFileAsJSON(this.GetType(), Path.Combine(new string[] { "TestData", "TrialOrder", "out-of-order-return.json" }));

            JObject expected = (JObject)TestFileTools.GetTestFileAsJSON(this.GetType(), Path.Combine(new string[] { "TestData", "TrialOrder", "out-of-order-expected.json" }));

            CTSPrintRequestHandler handler = new CTSPrintRequestHandler();
            JObject actual = handler.EnforceTrialOrder(trials, trialIDOrder);

            Assert.Equal(expected, actual, new JTokenEqualityComparer());
        }

        /// <summary>
        /// Verify the method recovers gracefully (no crashing) when a trial is missing
        /// from the data set.
        /// </summary>
        [Fact]
        public void MissingTrialdID()
        {
            // The order we want them in.
            // Trial NCI-1234-56789 does not exist in the data.
            string[] trialIDOrder = { "NCI-1234-56789", "NCI-2018-03694", "NCI-2020-00544", "NCI-2018-01575" };

            // Trials as returned by the CTS API.
            JObject trials = (JObject)TestFileTools.GetTestFileAsJSON(this.GetType(), Path.Combine(new string[] { "TestData", "TrialOrder", "out-of-order-return.json" }));

            JObject expected = (JObject)TestFileTools.GetTestFileAsJSON(this.GetType(), Path.Combine(new string[] { "TestData", "TrialOrder", "out-of-order-expected.json" }));

            CTSPrintRequestHandler handler = new CTSPrintRequestHandler();
            JObject actual = handler.EnforceTrialOrder(trials, trialIDOrder);

            Assert.Equal(expected, actual, new JTokenEqualityComparer());
        }
    }
}
