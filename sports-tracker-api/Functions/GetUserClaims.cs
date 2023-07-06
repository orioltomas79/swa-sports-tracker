using System.Net;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;
using Microsoft.Extensions.Logging;

namespace SportsTracker.Api.Functions
{
    public class GetUserClaims
    {
        private readonly ILogger<GetUserClaims> _logger;

        public GetUserClaims(ILogger<GetUserClaims> log)
        {
            _logger = log;
        }

        [FunctionName("GetUserClaims")]
        [OpenApiOperation(operationId: "GET_User_Claims", tags: new[] { "users" })]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(UserClaims), Description = "The OK response")]
        public IActionResult Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = null)] HttpRequest req)
        {
            _logger.LogInformation("GetUserClaims function processed a request.");

            var claimsPrincipal = StaticWebAppsAuth.Parse(req);

            var userClaims = new UserClaims()
            {
                Name = claimsPrincipal.Identity!.Name,
                AuthType = claimsPrincipal.Identity.AuthenticationType
            };
            
            return new OkObjectResult(userClaims);
        }

        public class UserClaims
        {
            public string Name { get; set; }
            public string AuthType { get; set; }
        }
    }
}

