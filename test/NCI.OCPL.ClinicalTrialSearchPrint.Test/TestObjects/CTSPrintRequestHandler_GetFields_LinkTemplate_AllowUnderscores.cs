using Newtonsoft.Json.Linq;

namespace NCI.OCPL.ClinicalTrialSearchPrint.Test
{
    /// <summary>
    /// Test data for allowing underscores in the link template (e.g. trial type = "basic_science").
    /// </summary>
    class CTSPrintRequestHandler_GetFields_LinkTemplate_AllowUnderscores
        : CTSPrintRequestHandler_GetFields_Base
    {
        public override JObject RequestData =>
            JObject.Parse(@"
{
    ""trial_ids"": [""NCI-2015-01906""],
    ""link_template"": ""/about-cancer/treatment/clinical-trials/search/v?loc=0&rl=2&tt=basic_science&id=<TRIAL_ID>"",
    ""new_search_link"": ""/about-cancer/treatment/clinical-trials/search/advanced"",
    ""search_criteria"": null
}
");

        public override string[] ExpectedTrials => new string[] { "NCI-2021-08584" };

        public override string ExpectedLinkTemplate => "/about-cancer/treatment/clinical-trials/search/v?loc=0&rl=2&tt=basic_science&id=<TRIAL_ID>";
    }
}
