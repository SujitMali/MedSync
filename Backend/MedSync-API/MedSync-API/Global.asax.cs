using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using System.Net;

namespace MedSync_API
{
    public class WebApiApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {

            // In Global.asax.cs -> Application_Start()

            // Try this more flexible combination of modern protocols
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12
                                                   | SecurityProtocolType.Tls11
                                                   | SecurityProtocolType.Tls;

            AreaRegistration.RegisterAllAreas();
            GlobalConfiguration.Configure(WebApiConfig.Register);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }

        protected void Application_BeginRequest(object sender, EventArgs e)
        {
            HttpContext.Current.Response.AddHeader("Access-Control-Allow-Origin", "*");
            if (HttpContext.Current.Request.HttpMethod == "OPTIONS")
            {
                HttpContext.Current.Response.AddHeader("Access-Control-Allow-Methods",
                "GET, POST, PUT, DELETE");
                HttpContext.Current.Response.AddHeader("Access-Control-Allow-Headers",
                "Content-Type, Accept, Authorization");
                HttpContext.Current.Response.End();
            }
        }

    }
}
