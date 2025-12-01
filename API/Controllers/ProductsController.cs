using Core.Entities;
using Core.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ProductsController(IProductRepository productRepository) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<Product>>> GetProducts(string? brand, string? type, string? sort)
    {
        return Ok(await productRepository.GetProductsAsync(brand, type, sort));
    }

    [HttpGet("{id:int}")] // api/products/1
    public async Task<ActionResult<Product>> GetProduct(int id)
    {
        var product = await productRepository.GetProductByIdAsync(id);
        return product == null ? NotFound() : product;
    }

    [HttpGet("brands")]
    public async Task<ActionResult<IReadOnlyList<string>>> GetBrands()
    {
        return Ok(await productRepository.GetBrandsAsync());
    }

    [HttpGet("types")]
    public async Task<ActionResult<IReadOnlyList<string>>> GetTypes()
    {
        return Ok(await productRepository.GetTypesAsync());
    }

    [HttpPost]
    public async Task<ActionResult<Product>> CreateProduct(Product product)
    {
        productRepository.AddProduct(product);

        if (await productRepository.SaveChangesAsync())
            return CreatedAtAction("GetProduct", new { id = product.Id }, product);

        await productRepository.SaveChangesAsync();
        return BadRequest("Error creating the product");
    }

    [HttpPut("{id:int}")] // api/products/1
    public async Task<ActionResult> UpdateProduct(int id, Product product)
    {
        if (product.Id != id || !_DoesProductExists(id))
            return BadRequest("Cannot update this product!");

        productRepository.UpdateProduct(product);
        if (await productRepository.SaveChangesAsync()) return NoContent();

        return BadRequest("Error updating the product");
    }

    [HttpDelete("{id:int}")] // api/products/1
    public async Task<ActionResult> DeleteProduct(int id)
    {
        var productToRemove = await productRepository.GetProductByIdAsync(id);

        if (productToRemove == null) return NotFound();

        productRepository.DeleteProduct(productToRemove);
        if (await productRepository.SaveChangesAsync()) return NoContent();

        return BadRequest("Error deleting the product");
    }

    private bool _DoesProductExists(int id)
    {
        return productRepository.ProductExists(id);
    }
}