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

            builder.EntitySet<tbUser>("tbUser");
            builder.EntityType<tbUser>().Collection.Function("GetUserRole").Returns<sp_GetUserRole_Result>();
            builder.EntityType<tbUser>().Collection.Action("Login").Returns<string>().Parameter<tbUser>("user");
            builder.EntityType<tbUser>().Collection.Action("Logout").Returns<string>();

            builder.EntitySet<tbRequestNVQ>("tbRequestNVQS");

            config.MapODataServiceRoute("odata", "odata", model: builder.GetEdmModel());
        }
    }
}
