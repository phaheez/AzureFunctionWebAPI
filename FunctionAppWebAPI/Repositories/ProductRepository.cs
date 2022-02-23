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
            new Product() { Id = Guid.NewGuid().ToString("n"), Name = "Avengers End Game", IsCompleted = false, CreatedAt = DateTime.UtcNow },
            new Product() { Id = Guid.NewGuid().ToString("n"), Name = "Wonder Woman", IsCompleted = false, CreatedAt = DateTime.UtcNow },
            new Product() { Id = Guid.NewGuid().ToString("n"), Name = "Gloceries", IsCompleted = false, CreatedAt = DateTime.UtcNow }
        };

        public async Task CreateProductAsync(ProductCreateModel model)
        {
            var product = new Product { Name = model.Name };
            products.Add(product);
            await Task.CompletedTask;
        }

        public async Task DeleteProductAsync(string id)
        {
            var index = products.FindIndex(existingProduct => existingProduct.Id == id);
            products.RemoveAt(index);
            await Task.CompletedTask;
        }

        public async Task<Product> GetProductByIdAsync(string id)
        {
            var product = products.Where(product => product.Id == id).SingleOrDefault();
            return await Task.FromResult(product);
        }

        public async Task<List<Product>> GetProductsAsync()
        {
            return await Task.FromResult(products.ToList());
        }

        public async Task UpdateProductAsync(string id, ProductUpdateModel model)
        {
            var product = products.FirstOrDefault(p => p.Id == id);
            product.Name = model.Name;
            product.IsCompleted = model.IsCompleted;
            await Task.CompletedTask;
        }
    }
}
