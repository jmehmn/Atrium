using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.Extensions.DependencyInjection;
using Coti.Web.Core.Filters;
using Coti.Web.Models.Responses;
using System;
using System.Linq;

namespace Coti.Web.StartUp
{
    public abstract class MVC
    {
        public static void ConfigureServices(IServiceCollection services)
        {
            IMvcBuilder mvc = services.AddControllersWithViews(o =>
            {
                var policy = new AuthorizationPolicyBuilder()
                    .RequireAuthenticatedUser()
                    .Build();

                o.Filters.Add(new AuthorizeFilter(policy));

                o.Filters.Add(typeof(ModelBindAttribute));
            });

            mvc.SetCompatibilityVersion(CompatibilityVersion.Version_3_0);

            Action<ApiBehaviorOptions> setUpApiBehavior = apiBehaviorOptions =>
            {
                apiBehaviorOptions.InvalidModelStateResponseFactory = ErrorResponseFactory;
            };
            mvc.ConfigureApiBehaviorOptions(setUpApiBehavior);
        }

        public static void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            //app.UseMvc();
        }

        private static IActionResult ErrorResponseFactory(ActionContext arg)
        {
            ErrorResponse err = new ErrorResponse(arg.ModelState.Values
                        .SelectMany(e => e.Errors)
                        .Select(e => e.ErrorMessage));

            var result = new BadRequestObjectResult(err);

            result.ContentTypes.Add("application/json");

            return result;
        }
    }
}