using System;
using Newtonsoft.Json.Linq;

namespace NCI.OCPL.ClinicalTrialSearchPrint.Test
{
    public abstract class CTSPrintRequestHandler_GetFields_Base
    {
        public abstract JObject RequestData { get; }

        // The Expected.... fields don't need to be overridden unless a test
        // case uses that particular value.

        /// <summary>
        /// The list of trials which is expected be returned.
        /// </summary>
        public virtual string[] ExpectedTrials => new string[] { "This is the default value." };

        /// <summary>
        /// The link template which is expected to be returned.
        /// </summary>
        public virtual string ExpectedLinkTemplate => "This is the defaualt value.";

        /// <summary>
        /// The new search link which is expected to be returned.
        /// </summary>
        public virtual string ExpectedNewSearchLink => "This is the default value.";

        /// <summary>
        /// The search criteria which are expected be returned.
        /// </summary>
        public virtual JObject ExpectedSearchCriteria => JObject.Parse("{\"default-key\": \"This is the default value.\"}");

        /// <summary>
        /// The name of a field which is expected to cause a problem.
        /// </summary>
        public virtual string ExpectedErrorFieldName => "This is the default value.";
    }
}
