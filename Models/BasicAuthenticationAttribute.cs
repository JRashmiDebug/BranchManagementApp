using System;
using System.Net;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace BranchManagementApp.Models
{
    public class BasicAuthenticationAttribute : AuthorizeAttribute
    {
        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            // Check for Authorization header
            var authHeader = httpContext.Request.Headers["Authorization"];
            if (string.IsNullOrEmpty(authHeader) || !authHeader.StartsWith("Basic "))
            {
                return false;
            }

            // Decode the username and password
            var encodedCredentials = authHeader.Substring(6); // Remove "Basic " prefix
            var decodedCredentials = Encoding.UTF8.GetString(Convert.FromBase64String(encodedCredentials));

            // Split the credentials (username:password)
            var credentials = decodedCredentials.Split(':');
            if (credentials.Length != 2)
            {
                return false;
            }

            var username = credentials[0];
            var password = credentials[1];

            // Validate credentials (replace with secure logic)
            var adminUsername = System.Configuration.ConfigurationManager.AppSettings["AdminUsername"];
            var adminPassword = System.Configuration.ConfigurationManager.AppSettings["AdminPassword"];

            return username == adminUsername && password == adminPassword;
        }

        protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
        {
            // Return 401 Unauthorized with Basic Authentication header
            filterContext.HttpContext.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
            filterContext.HttpContext.Response.AddHeader("WWW-Authenticate", "Basic realm=\"BranchManagementApp\"");
            filterContext.Result = new HttpUnauthorizedResult();
        }
    }
}