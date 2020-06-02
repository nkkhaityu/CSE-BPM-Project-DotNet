using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;

namespace DemoBPM.Common.Security
{
    public class SEAuthorizeAttribute : AuthorizationFilterAttribute
    {
        public override Task OnAuthorizationAsync(HttpActionContext actionContext, CancellationToken cancellationToken)
        {
            // Allow Anonymous Attribute
            var actionDescriptor = actionContext.ActionDescriptor;
            var allowAnonymous = actionDescriptor.GetCustomAttributes<System.Web.Http.AllowAnonymousAttribute>(false);
            if (allowAnonymous.Count > 0)
            {
                return Task.FromResult<object>(null);
            }

            //
            if (SessionExtensions._session.Count <= 0)
            {
                actionContext.Response = actionContext.Request.CreateResponse(HttpStatusCode.Unauthorized);
                return Task.FromResult<object>(null);
            }

            return Task.CompletedTask;
        }

    }
}