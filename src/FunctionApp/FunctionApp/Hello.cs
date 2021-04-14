using System.Collections.Generic;
using FunctionApp.DataAccess.Containers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Enums;
using Microsoft.OpenApi.Models;
using System.Net;
using System.Threading.Tasks;
using FunctionApp.DataAccess.Models;

namespace FunctionApp {
    public class Hello {
        private readonly IItemContainer _container;
        public Hello(IItemContainer container) => _container = container;

        [FunctionName(nameof(Run))]
        [OpenApiOperation(operationId: "Run", tags: new[] { "name" })]
        [OpenApiSecurity("function_key", SecuritySchemeType.OpenIdConnect, Name = "code", In = OpenApiSecurityLocationType.Query)]
        [OpenApiParameter(name: "name", In = ParameterLocation.Query, Required = true, Type = typeof(string), Description = "The **Name** parameter")]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, "application/json", typeof(IEnumerable<Item>), Description = "The OK response")]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "items/{id?}")]
            HttpRequest request, string id) {
            var items = _container.GetAsync(id);
            var counts = _container.GetCountAsync();
            await Task.WhenAll(items, counts);
            return new OkObjectResult(new {
                count = await counts,
                items = await items
            });
        }
    }
}

