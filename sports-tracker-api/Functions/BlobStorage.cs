using System;
using System.IO;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Azure.Storage.Blobs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace SportsTracker.Api.Functions
{
    public class BlobStorage
    {
        private readonly ILogger<BlobStorage> _logger;

        public BlobStorage(ILogger<BlobStorage> log)
        {
            _logger = log;
        }
        
        [FunctionName("CreateFile")]
        [OpenApiOperation(operationId: "CreateFile")]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "text/plain", bodyType: typeof(string), Description = "The OK response")]
        public async Task<IActionResult> CreateFile(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = null)] HttpRequest req)
        {
            _logger.LogInformation("C# HTTP trigger function processed a request.");

            // Create blob client
            var connectionStr = Environment.GetEnvironmentVariable("StorageAccountConnectionString");
            const string containerName = "testcontainer";
            var blobContainerClient = new BlobContainerClient(connectionStr, containerName);
            await blobContainerClient.CreateIfNotExistsAsync();

            // Get a reference to a blob in a container 
            const string blobName = "1.json";
            var blobClient = blobContainerClient.GetBlobClient(blobName);

            // Upload the content
            var testObj = new TestObject() { TestProperty = DateTime.Now.ToLongDateString() };
            var byteArray = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(testObj));
            await using var memoryStream = new MemoryStream(byteArray);
            await blobClient.UploadAsync(memoryStream, overwrite: true);

            return new OkObjectResult("Completed");
        }

        public class TestObject
        {
            public string TestProperty { get; set; }
        }
    }
}

