using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NCI.OCPL.ClinicalTrialSearchPrint
{
    public class CTSPrintRequestHandler : IHttpHandler
    {
        public bool IsReusable => false;

        public void ProcessRequest(HttpContext context)
        {
            HttpRequest request = context.Request;

            if (request.HttpMethod == "POST")
            {
                context.Response.StatusCode = 200;
                context.Response.Write("Looks like a POST request.");
            }
            else if (request.HttpMethod == "GET")
            {
                context.Response.StatusCode = 200;
                context.Response.Write("Looks like a GET request.");
            }
            else
            {
                context.Response.StatusCode = 400;
                context.Response.Status = "I don't know what you're trying to do there.";
            }
        }
    }
}