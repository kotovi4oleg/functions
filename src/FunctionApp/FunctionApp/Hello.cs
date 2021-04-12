using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using FunctionApp.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Cosmos;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Enums;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json;

namespace FunctionApp
{
    public class Hello {
        private readonly CosmosClient _cosmos;
        private const string Database = "Functions";
        private const string Container = "Items";
        public Hello(CosmosClient cosmos) => _cosmos = cosmos;


        [FunctionName("Create")]
        [OpenApiOperation(operationId: "Create", tags: new[] { "name" })]
        [OpenApiSecurity("function_key", SecuritySchemeType.ApiKey, Name = "code", In = OpenApiSecurityLocationType.Query)]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "text/plain", bodyType: typeof(string), Description = "The OK response")]
        public async Task<IActionResult> Create(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = null)] HttpRequest req,
            ILogger log) {
            var database = await _cosmos.CreateDatabaseIfNotExistsAsync(Database);
            var container = await database.Database.CreateContainerIfNotExistsAsync(Container, "/id");

            var item = new Item {
                Id = Guid.NewGuid().ToString("N"),
                Completed = false,
                Description = "My Cosmos Item",
                Name = "Item #1"
            };

            try {
                var result = await container.Container.CreateItemAsync(item, new PartitionKey(item.Id));
            }
            catch (CosmosException e) {
                
            }

            return new OkObjectResult(item.Id);
        }

        [FunctionName("Hello")]
        [OpenApiOperation(operationId: "Run", tags: new[] { "name" })]
        [OpenApiSecurity("function_key", SecuritySchemeType.ApiKey, Name = "code", In = OpenApiSecurityLocationType.Query)]
        [OpenApiParameter(name: "name", In = ParameterLocation.Query, Required = true, Type = typeof(string), Description = "The **Name** parameter")]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "text/plain", bodyType: typeof(string), Description = "The OK response")]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = null)] HttpRequest req,
            ILogger log) {

            var container = _cosmos.GetContainer(Database, Container);

            var iterator = container.GetItemQueryIterator<Item>("SELECT * FROM c");

            var results = new List<Item>();

            while (iterator.HasMoreResults) {
                foreach (var item in await iterator.ReadNextAsync()) {
                    results.Add(item);
                }   
            }

            return new OkObjectResult(results);
        }
    }
}

