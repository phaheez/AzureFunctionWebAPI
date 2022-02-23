using FunctionAppWebAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FunctionAppWebAPI.Repositories
{
    public interface IProductRepository
    {
        Task<List<Product>> GetProductsAsync();
        Task<Product> GetProductByIdAsync(string id);
        Task CreateProductAsync(ProductCreateModel model);
        Task UpdateProductAsync(string id, ProductUpdateModel product);
        Task DeleteProductAsync(string id);
    }
}
