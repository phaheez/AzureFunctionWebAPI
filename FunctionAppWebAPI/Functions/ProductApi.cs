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
    public class ProductApi
    {
        private readonly IProductRepository _productRepository;
        private readonly IConfiguration _configuration;

        public ProductApi(IConfiguration configuration, IProductRepository productRepository)
        {
            _configuration = configuration;
            _productRepository = productRepository;
        }

        // Create New Product
        [Function("CreateProduct")]
        public async Task<HttpResponseData> CreateProduct([HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "product")] HttpRequestData req,
            FunctionContext executionContext)
        {
            HttpResponseData response;
            var logger = executionContext.GetLogger("CreateProduct");
            logger.LogInformation("Create a new product list item");

            try
            {
                var request = await new StreamReader(req.Body).ReadToEndAsync();

                var product = JsonConvert.DeserializeObject<ProductCreateModel>(request);

                await _productRepository.CreateProductAsync(product);

                response = req.CreateResponse(HttpStatusCode.Created);
            }
            catch (Exception ex)
            {
                logger.LogError($"Exception thrown: {ex.Message}");
                response = req.CreateResponse(HttpStatusCode.InternalServerError);
            }

            return response;
        }

        // Get All Products
        [Function("GetProducts")]
        public async Task<HttpResponseData> GetProducts([HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "product")] HttpRequestData req,
            FunctionContext executionContext)
        {
            HttpResponseData response;
            var logger = executionContext.GetLogger("GetProducts");
            logger.LogInformation("Getting product list items");

            try
            {
                var products = await _productRepository.GetProductsAsync();

                response = req.CreateResponse(HttpStatusCode.OK);
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

        // Get Product By Id
        [Function("GetProductById")]
        public async Task<HttpResponseData> GetProductById([HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "product/{id}")] HttpRequestData req,
            FunctionContext executionContext, string id)
        {
            HttpResponseData response;
            var logger = executionContext.GetLogger("GetProductById");
            logger.LogInformation("Getting single product item");

            try
            {
                var product = await _productRepository.GetProductByIdAsync(id);
                if (product == null)
                {
                    response = req.CreateResponse(HttpStatusCode.NotFound);
                }
                else
                {
                    response = req.CreateResponse(HttpStatusCode.OK);
                    //response.Headers.Add("Content-Type", "application/json; charset=utf-8");
                    await response.WriteAsJsonAsync(product);
                }
            }
            catch (Exception ex)
            {
                logger.LogError($"Exception thrown: {ex.Message}");
                response = req.CreateResponse(HttpStatusCode.InternalServerError);
            }

            return response;
        }

        // Update Product
        [Function("UpdateProduct")]
        public async Task<HttpResponseData> UpdateProduct([HttpTrigger(AuthorizationLevel.Anonymous, "put", Route = "product/{id}")] HttpRequestData req,
            FunctionContext executionContext, string id)
        {
            HttpResponseData response;
            var logger = executionContext.GetLogger("UpdateProduct");
            logger.LogInformation("Update product item");

            try
            {
                var existingProduct = await _productRepository.GetProductByIdAsync(id);
                if (existingProduct == null)
                {
                    response = req.CreateResponse(HttpStatusCode.NotFound);
                }
                else
                {
                    var request = await new StreamReader(req.Body).ReadToEndAsync();

                    var productModel = JsonConvert.DeserializeObject<ProductUpdateModel>(request);

                    existingProduct.IsCompleted = productModel.IsCompleted;
                    if (!string.IsNullOrWhiteSpace(productModel.Name))
                    {
                        existingProduct.Name = productModel.Name;
                    }

                    var updatedProduct = await _productRepository.UpdateProductAsync(existingProduct);

                    response = req.CreateResponse(HttpStatusCode.OK);
                    await response.WriteAsJsonAsync(updatedProduct);
                }
            }
            catch (Exception ex)
            {
                logger.LogError($"Exception thrown: {ex.Message}");
                response = req.CreateResponse(HttpStatusCode.InternalServerError);
            }

            return response;
        }

        // Delete Product
        [Function("DeleteProduct")]
        public async Task<HttpResponseData> DeleteProduct([HttpTrigger(AuthorizationLevel.Anonymous, "delete", Route = "product/{id}")] HttpRequestData req,
            FunctionContext executionContext, string id)
        {
            HttpResponseData response;
            var logger = executionContext.GetLogger("DeleteProduct");
            logger.LogInformation("Deleting single product item");

            try
            {
                var product = await _productRepository.GetProductByIdAsync(id);
                if (product == null)
                {
                    response = req.CreateResponse(HttpStatusCode.NotFound);
                }
                else
                {
                    await _productRepository.DeleteProductAsync(product);

                    response = req.CreateResponse(HttpStatusCode.OK);
                }
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
