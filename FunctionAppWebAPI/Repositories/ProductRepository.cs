using FunctionAppWebAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FunctionAppWebAPI.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private readonly List<Product> products = new()
        {
            new Product() { Id = Guid.NewGuid().ToString("n"), Name = "Avengers End Game", IsCompleted = false, CreatedAt = DateTime.UtcNow }
        };

        public async Task CreateProductAsync(ProductCreateModel model)
        {
            var product = new Product { Name = model.Name };
            products.Add(product);
            await Task.CompletedTask;
        }

        public async Task<List<Product>> GetProductsAsync()
        {
            return await Task.FromResult(products.ToList());
        }

        public async Task<Product> GetProductByIdAsync(string id)
        {
            var product = products.FirstOrDefault(product => product.Id == id);
            return await Task.FromResult(product);
        }

        public async Task<Product> UpdateProductAsync(Product product)
        {
            var index = products.FindIndex(existingItem => existingItem.Id == product.Id);
            products[index] = product;

            return await Task.FromResult(product);
        }

        public async Task DeleteProductAsync(Product product)
        {
            products.Remove(product);
            await Task.CompletedTask;
        }
    }
}
