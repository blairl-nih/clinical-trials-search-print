﻿using Newtonsoft.Json.Linq;

namespace NCI.OCPL.ClinicalTrialSearchPrint.Test
{
    // Attempt to inject markup into the new_search_link field.
    class CTSPrintRequestHandler_GetFields_NewSearchLink_InjectMarkup
        : CTSPrintRequestHandler_GetFields_Base
    {
        public override JObject RequestData =>
            JObject.Parse(@"
{
    ""trial_ids"": [""NCI-2015-01906""],
    ""link_template"": ""/about-cancer/treatment/clinical-trials/search/v?t=C3208&st=C3471&a=24&loc=1&z=20850&zp=100&rl=2&id=<TRIAL_ID>"",
    ""new_search_link"": ""/about-cancer/treatment/clinical-trials/search/advanced\""> </a> <p>Evil markup</p>"",
    ""search_criteria"": null
}
");

        public override string ExpectedErrorFieldName => "new_search_link";
    }
}
