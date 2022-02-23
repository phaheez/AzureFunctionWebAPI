using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using FunctionAppWebAPI.Models;
using FunctionAppWebAPI.Repositories;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace FunctionAppWebAPI.Functions
{
    public static class ProductsGet
    {
        private static readonly IProductRepository productRepo = new ProductRepository();

        [Function("GetProducts")]
        public static async Task<HttpResponseData> RunAsync([HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "products")] HttpRequestData req,
            FunctionContext executionContext)
        {
            HttpResponseData response;
            var logger = executionContext.GetLogger("ProductsGet");
            logger.LogInformation("C# HTTP trigger function processed a request.");

            //var query = System.Web.HttpUtility.ParseQueryString(req.Url.Query);
            //string name = query["name"];

            //string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            //dynamic data = JsonConvert.DeserializeObject(requestBody);
            //name ??= data?.name;

            //string responseMessage = string.IsNullOrEmpty(name)
            //    ? "This HTTP triggered function executed successfully. Pass a name in the query string or in the request body for a personalized response"
            //    : $"Hello, {name}. This HTTP triggered function executed successfully.";

            //var response = req.CreateResponse(HttpStatusCode.OK);
            //response.Headers.Add("Content-Type", "text/plain; charset=utf-8");

            //response.WriteString(responseMessage);

            //return response;

            try
            {
                var products = await productRepo.GetProductsAsync();

                response = req.CreateResponse(HttpStatusCode.Created);
                //response.Headers.Add("Content-Type", "application/json; charset=utf-8");
                await response.WriteAsJsonAsync(products);
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
