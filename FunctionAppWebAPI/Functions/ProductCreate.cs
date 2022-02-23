using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using FunctionAppWebAPI.Models;
using FunctionAppWebAPI.Repositories;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace FunctionAppWebAPI.Functions
{
    public class ProductCreate
    {
        private readonly IProductRepository _productRepository;
        private readonly IConfiguration _configuration;

        public ProductCreate(IConfiguration configuration, IProductRepository productRepository)
        {
            _configuration = configuration;
            _productRepository = productRepository;
        }

        [Function("ProductCreate")]
        public async Task<HttpResponseData> Run([HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "Product")] HttpRequestData req,
            FunctionContext executionContext)
        {
            HttpResponseData response;
            var logger = executionContext.GetLogger("ProductCreate");
            logger.LogInformation("C# HTTP trigger function processed a request.");

            try
            {
                var request = await new StreamReader(req.Body).ReadToEndAsync();

                var product = JsonConvert.DeserializeObject<ProductCreateModel>(request);

                await _productRepository.CreateProductAsync(product);

                response = req.CreateResponse(HttpStatusCode.Created);
                response.Headers.Add("Content-Type", "text/plain; charset=utf-8");
            }
            catch (Exception ex)
            {
                logger.LogError($"Exception thrown: {ex.Message}");
                response = req.CreateResponse(HttpStatusCode.InternalServerError);
            }

            return response;
        }
    }
}
