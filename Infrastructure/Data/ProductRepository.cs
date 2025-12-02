using Core.Entities;
using Core.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data;

public class ProductRepository(StoreContext context) : IProductRepository
{
    public async Task<IReadOnlyList<string>> GetBrandsAsync()
    {
        return await context.Products.Select(product => product.Brand).Distinct().ToListAsync();
    }

    public async Task<IReadOnlyList<string>> GetTypesAsync()
    {
        return await context.Products.Select(product => product.Type).Distinct().ToListAsync();
    }
}