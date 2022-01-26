﻿using System.Collections.Generic;
using System.Linq;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Xunit;

using NCI.Test.IO;

namespace NCI.OCPL.ClinicalTrialSearchPrint.Test
{
    public class CTSPrintRequestHandler_GetFields
    {
        public static IEnumerable<object[]> NoTrialIDs_Data = new[]
        {
            new object[] {new CTSPrintRequestHandler_GetFields_TrialIDs_Missing() },
            new object[] {new CTSPrintRequestHandler_GetFields_TrialIDs_Null() },
            new object[] {new CTSPrintRequestHandler_GetFields_TrialIDs_Empty() }
        };

        /// <summary>
        /// Tests for when the required trial_ids field is missing.
        /// </summary>
        /// <param name="data"></param>
        [Theory]
        [MemberData(nameof(NoTrialIDs_Data))]
        public void NoTrialIDs(CTSPrintRequestHandler_GetFields_Base data)
        {
            string expectedField = "trial_ids";

            var handler = new CTSPrintRequestHandler();

            MissingFieldException ex = Assert.Throws<MissingFieldException>(
                () => handler.GetFields(data.RequestData)
            );

            Assert.Equal(expectedField, ex.FieldName);
        }

        public static IEnumerable<object[]> TrialIDsPresent_Data = new[]
        {
            new object[] {new CTSPrintRequestHandler_GetFields_TrialIDs_Single() },
            new object[] {new CTSPrintRequestHandler_GetFields_TrialIDs_Multiple() }
        };

        /// <summary>
        /// Successful load of trials_id field.
        /// </summary>
        [Theory]
        [MemberData(nameof(TrialIDsPresent_Data))]
        public void TrialIDsPresent(CTSPrintRequestHandler_GetFields_Base data)
        {
            var handler = new CTSPrintRequestHandler();

            var actual = handler.GetFields(data.RequestData);

            Assert.Equal(data.ExpectedTrials, actual.TrialIDs);
        }

        public static IEnumerable<object[]> NoLinkTemplate_Data = new[]
        {
            new object[] {new CTSPrintRequestHandler_GetFields_LinkTemplate_Missing() },
            new object[] {new CTSPrintRequestHandler_GetFields_LinkTemplate_Null() }
        };

        /// <summary>
        /// Fail when the link template is missing.
        /// </summary>
        [Theory]
        [MemberData(nameof(NoLinkTemplate_Data))]
        public void NoLinkTemplate(CTSPrintRequestHandler_GetFields_Base data)
        {
            string expectedField = "link_template";

            var handler = new CTSPrintRequestHandler();

            MissingFieldException ex = Assert.Throws<MissingFieldException>(
                () => handler.GetFields(data.RequestData)
            );

            Assert.Equal(expectedField, ex.FieldName);
        }

        public static IEnumerable<object[]> LinkTemplatePresent_Data = new[]
        {
            new object[] {new CTSPrintRequestHandler_GetFields_LinkTemplate_Present() }
        };

        /// <summary>
        /// Succeed when the link template is present.
        /// </summary>
        [Theory]
        [MemberData(nameof(LinkTemplatePresent_Data))]
        public void LinkTemplatePresent(CTSPrintRequestHandler_GetFields_Base data)
        {
            var handler = new CTSPrintRequestHandler();

            var actual = handler.GetFields(data.RequestData);

            Assert.Equal(data.ExpectedLinkTemplate, actual.LinkTemplate);
        }

        public static IEnumerable<object[]> NewSearchLinkMissing_Data = new[]
        {
            new  object[] {new CTSPrintRequestHandler_GetFields_NewSearchLink_Null() },
            new  object[] {new CTSPrintRequestHandler_GetFields_NewSearchLink_Empty() },
            new  object[] {new CTSPrintRequestHandler_GetFields_NewSearchLink_Missing() },
        };

        /// <summary>
        /// Recover appropriately when the new_search_link field is omitted.
        /// </summary>
        [Theory]
        [MemberData(nameof(NewSearchLinkMissing_Data))]
        public void NewSearchLinkMissing(CTSPrintRequestHandler_GetFields_Base data)
        {
            var handler = new CTSPrintRequestHandler();

            var actual = handler.GetFields(data.RequestData);

            Assert.Equal(data.ExpectedNewSearchLink, actual.NewSearchLink);
        }

        public static IEnumerable<object[]> NewSearchLinkPresent_Data = new[]
        {
            new  object[] {new CTSPrintRequestHandler_GetFields_NewSearchLink_Present() }
        };

        /// <summary>
        /// Success when the new_search_link field is present.
        /// </summary>
        [Theory]
        [MemberData(nameof(NewSearchLinkPresent_Data))]
        public void NewSearchLinkPresent(CTSPrintRequestHandler_GetFields_Base data)
        {
            var handler = new CTSPrintRequestHandler();

            var actual = handler.GetFields(data.RequestData);

            Assert.Equal(data.ExpectedNewSearchLink, actual.NewSearchLink);
        }

        public static IEnumerable<object[]> SearchCriteriaMissing_Data = new[]
        {
            new object[] { new CTSPrintRequestHandler_GetFields_SearchCriteria_Null() },
            new object[] { new CTSPrintRequestHandler_GetFields_SearchCriteria_Empty() },
            new object[] { new CTSPrintRequestHandler_GetFields_SearchCriteria_Missing() }
        };

        /// <summary>
        /// Respond appropriately when the search_criteria field is omitted.
        /// </summary>
        [Theory]
        [MemberData(nameof(SearchCriteriaMissing_Data))]
        public void SearchCriteriaMissing(CTSPrintRequestHandler_GetFields_Base data)
        {
            var handler = new CTSPrintRequestHandler();

            var actual = handler.GetFields(data.RequestData);

            Assert.Null(actual.SearchCriteria);
        }

        public static IEnumerable<object[]> SearchCriteriaPresent_Data = new[]
        {
            new object[] { new CTSPrintRequestHandler_GetFields_SearchCriteria_Simple() },
            new object[] { new CTSPrintRequestHandler_GetFields_SearchCriteria_Complex() }
        };

        /// <summary>
        /// Success when the search_criteria field is present.
        /// </summary>
        [Theory]
        [MemberData(nameof(SearchCriteriaPresent_Data))]
        public void SearchCriteriaPresent(CTSPrintRequestHandler_GetFields_Base data)
        {
            var handler = new CTSPrintRequestHandler();

            var actual = handler.GetFields(data.RequestData);

            Assert.Equal(data.ExpectedSearchCriteria, actual.SearchCriteria, new JTokenEqualityComparer());
        }
    }
}