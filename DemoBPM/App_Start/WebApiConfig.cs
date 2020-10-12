using DemoBPM.Database;
using Microsoft.AspNet.OData.Builder;
using Microsoft.AspNet.OData.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;

namespace DemoBPM
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // Web API configuration and services

            // Web API routes
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );

            var builder = new ODataConventionModelBuilder();

            config.Count().Filter().OrderBy().Expand().Select().MaxTop(null);

            builder.EntitySet<tbUser>("tbUser");
            builder.EntityType<tbUser>().Collection.Function("GetCurrentUser").Returns<tbUser>();
            builder.EntityType<tbUser>().Collection.Function("GetUserRole").Returns<sp_GetUserRole_Result>();
            builder.EntityType<tbUser>().Collection.Action("Login").Returns<string>().Parameter<tbUser>("user");
            builder.EntityType<tbUser>().Collection.Action("Logout").Returns<string>();

            builder.EntitySet<tbRole>("tbRole");
            builder.EntitySet<tbRequest>("tbRequest");
            builder.EntitySet<tbStep>("tbStep");

            builder.EntitySet<tbStepInstance>("tbStepInstance");
            builder.EntityType<tbStepInstance>().Collection.Function("GetStepInstanceDetails").Returns<sp_GetStepInstanceDetails_Result>();

            builder.EntitySet<tbRequestInstance>("tbRequestInstance");
            builder.EntityType<tbRequestInstance>().Collection.Function("GetRequestInstance").Returns<sp_GetRequestInstance_Result>();

            builder.EntitySet<tbInputField>("tbInputField");

            builder.EntitySet<tbInputFieldInstance>("tbInputFieldInstance");
            builder.EntityType<tbInputFieldInstance>().Collection.Function("GetInputFieldInstance").Returns<sp_GetInputFieldInstance_Result>();

            config.MapODataServiceRoute("odata", "odata", model: builder.GetEdmModel());
        }
    }
}
