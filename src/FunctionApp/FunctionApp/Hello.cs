using System;
using FunctionApp.Bussines.Queries;
using FunctionApp.Bussines.Services;
using FunctionApp.DataAccess.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Enums;
using Microsoft.OpenApi.Models;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using Azure.Identity;
using Azure.Security.KeyVault.Secrets;
using Microsoft.Extensions.Logging;

namespace FunctionApp {
    public class Hello {
        private readonly IQueryService _queries;
        public Hello(IQueryService queries) => _queries = queries;

        [FunctionName(nameof(Run))]
        [OpenApiOperation(operationId: "Run", tags: new[] { "name" })]
        [OpenApiSecurity("function_key", SecuritySchemeType.OpenIdConnect, Name = "code", In = OpenApiSecurityLocationType.Query)]
        [OpenApiParameter(name: "name", In = ParameterLocation.Query, Required = true, Type = typeof(string), Description = "The **Name** parameter")]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, "application/json", typeof(IEnumerable<Item>), Description = "The OK response")]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "items/{id?}")]
            HttpRequest request, ILogger logger, string id) {
            logger.LogDebug("Cosmos DB");
            logger.LogDebug($"Run processing {id}");
            return new OkObjectResult(await _queries.QueryAsync(new ItemPageQuery(id)));
        }

        [FunctionName(nameof(Secret))]
        [OpenApiOperation(operationId: "Secret", tags: new[] { "name" })]
        [OpenApiSecurity("function_key", SecuritySchemeType.OpenIdConnect, Name = "code", In = OpenApiSecurityLocationType.Query)]
        [OpenApiParameter(name: "name", In = ParameterLocation.Query, Required = true, Type = typeof(string), Description = "The **Name** parameter")]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, "application/json", typeof(IEnumerable<Item>), Description = "The OK response")]
        public async Task<IActionResult> Secret(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "secrets")]
            HttpRequest request) {
            var client = new SecretClient(new Uri(Environment.GetEnvironmentVariable("KEY_VAULT") ?? string.Empty), new DefaultAzureCredential());
            return new OkObjectResult(await client.GetSecretAsync("Cosmos-PrimaryKey"));
        }
    }
}

