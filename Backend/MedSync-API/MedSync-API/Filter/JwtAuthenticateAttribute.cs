using MedSync_ClassLibraries.Helpers;
using System;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using System.Web.Http.Controllers;

namespace MedSync_API.Filter
{
    public class JwtAuthenticateAttribute : AuthorizeAttribute
    {

        public override void OnAuthorization(HttpActionContext actionContext)
        {
            try
            {
                string token = null;
                var authHeader = actionContext.Request.Headers.Authorization;


                if (authHeader != null && authHeader.Scheme.Equals("Bearer", StringComparison.OrdinalIgnoreCase))
                    token = authHeader.Parameter;


                if (string.IsNullOrEmpty(token) && HttpContext.Current?.Request?.Cookies["JwtToken"] != null)
                    token = HttpContext.Current.Request.Cookies["JwtToken"].Value;


                if (string.IsNullOrEmpty(token))
                {
                    actionContext.Response = actionContext.Request.CreateResponse(
                        HttpStatusCode.Unauthorized,
                        new
                        {
                            success = false,
                            message = "Missing JWT token.",
                            data = (object)null
                        }
                    );
                    return;
                }

                var principal = JwtTokenManager.ValidateToken(token);

                if (principal == null || !principal.Identity.IsAuthenticated)
                {
                    actionContext.Response = actionContext.Request.CreateResponse(
                        HttpStatusCode.Unauthorized,
                        new
                        {
                            success = false,
                            message = "Invalid or expired token.",
                            data = (object)null
                        }
                    );
                    return;
                }

                HttpContext.Current.User = principal;
                actionContext.RequestContext.Principal = principal;
            }
            catch (Exception ex)
            {
                DbErrorLogger.LogError(ex, createdBy: 1);
                actionContext.Response = actionContext.Request.CreateResponse(
                    HttpStatusCode.Unauthorized,
                    new
                    {
                        success = false,
                        message = "Authorization failed: " + ex.Message,
                        data = (object)null
                    }
                );
            }
        }
    
    
    }
}